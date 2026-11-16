using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using SensorMonitor.Core.Analytics;
using SensorMonitor.Core.Data;
using SensorMonitor.Core.Dto;
using SensorMonitor.Core.Entities;
using SensorMonitor.Core.Services;

namespace SensorMonitor.Wpf;

public partial class MainWindow : Window
{
    private readonly string _dbPath = System.IO.Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "sensors.db");

    private string? _selectedJsonPath;
    private CancellationTokenSource? _cts;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        StatusText.Text = $"База даних: {_dbPath}. Оберіть JSON-файл для імпорту.";
    }

    // ---------- Допоміжні методи ----------

    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={_dbPath}")
            .Options;
        return new AppDbContext(options);
    }

    private SensorCatalog CreateCatalog(AppDbContext context)
        => new(new JsonFileReader<FactoryDto>(), new SqliteSensorRepository(context));

    private void SetBusy(bool busy, bool canCancel = false)
    {
        BrowseButton.IsEnabled = !busy;
        ImportButton.IsEnabled = !busy && !string.IsNullOrEmpty(_selectedJsonPath);
        CancelButton.IsEnabled = busy && canCancel;
        FactorySummaryButton.IsEnabled = !busy;
        TypeStatsButton.IsEnabled = !busy;
        AnomaliesButton.IsEnabled = !busy;
        ProblematicButton.IsEnabled = !busy;

        BusyBar.IsIndeterminate = busy;
        if (!busy)
            BusyBar.Value = 0;
    }

    private static bool TryParseDouble(string? text, out double value)
        => double.TryParse((text ?? string.Empty).Trim().Replace(',', '.'),
            NumberStyles.Float, CultureInfo.InvariantCulture, out value);

    // ---------- Імпорт ----------

    private void BrowseButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Оберіть JSON-файл з телеметрією",
            Filter = "JSON-файли (*.json)|*.json|Усі файли (*.*)|*.*",
            InitialDirectory = System.IO.Path.Combine(AppContext.BaseDirectory, "Data")
        };

        if (dialog.ShowDialog(this) != true)
            return;

        _selectedJsonPath = dialog.FileName;
        PathTextBox.Text = _selectedJsonPath;
        ImportButton.IsEnabled = true;
        StatusText.Text = "Файл обрано. Натисніть «Імпортувати в БД».";
    }

    private async void ImportButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_selectedJsonPath))
            return;

        _cts = new CancellationTokenSource();
        SetBusy(true, canCancel: true);
        StatusText.Text = "Імпорт розпочато…";

        try
        {
            await using var context = CreateContext();
            var catalog = CreateCatalog(context);

            var progress = new Progress<ImportProgress>(p =>
                StatusText.Text = $"Імпортовано: заводів — {p.Factories}, датчиків — {p.Sensors}, вимірювань — {p.Readings}");

            var result = await catalog.ImportAsync(_selectedJsonPath, progress, _cts.Token);

            StatusText.Text = $"Імпорт завершено. Заводів — {result.Factories}, датчиків — {result.Sensors}, вимірювань — {result.Readings}.";
        }
        catch (OperationCanceledException)
        {
            StatusText.Text = "Імпорт скасовано користувачем (вже збережені заводи залишились у БД).";
        }
        catch (Exception ex)
        {
            StatusText.Text = "Помилка імпорту.";
            MessageBox.Show(this, ex.Message, "Помилка імпорту", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            _cts?.Dispose();
            _cts = null;
            SetBusy(false);
        }
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        _cts?.Cancel();
    }

    // ---------- Аналітика ----------

    private async void FactorySummaryButton_Click(object sender, RoutedEventArgs e)
        => await RunAnalyticsAsync("Зведення по заводах", readings => readings.GetFactorySummaries());

    private async void TypeStatsButton_Click(object sender, RoutedEventArgs e)
        => await RunAnalyticsAsync("Статистика за типами датчиків", readings => readings.GetSensorTypeStatistics());

    private async void AnomaliesButton_Click(object sender, RoutedEventArgs e)
    {
        if (!TryParseDouble(SigmaTextBox.Text, out double sigma) || sigma <= 0)
        {
            MessageBox.Show(this, "Вкажіть додатне значення σ (наприклад, 2).", "Некоректний параметр",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        await RunAnalyticsAsync($"Статистичні аномалії (|z| > {sigma:0.##})",
            readings => readings.GetStatisticalAnomalies(sigma));
    }

    private async void ProblematicButton_Click(object sender, RoutedEventArgs e)
    {
        if (!TryParseDouble(ShareTextBox.Text, out double percent) || percent < 0 || percent > 100)
        {
            MessageBox.Show(this, "Вкажіть частку від 0 до 100 %.", "Некоректний параметр",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        await RunAnalyticsAsync($"Проблемні датчики (частка не Normal ≥ {percent:0.##} %)",
            readings => readings.GetProblematicSensors(percent / 100.0));
    }

    private async Task RunAnalyticsAsync(string title, Func<IReadOnlyList<ReadingEntity>, IEnumerable> query)
    {
        SetBusy(true);
        StatusText.Text = "Читання даних із БД…";

        try
        {
            await using var context = CreateContext();
            var catalog = CreateCatalog(context);

            List<ReadingEntity> readings = await catalog.GetReadingsAsync();
            if (readings.Count == 0)
            {
                ResultsGrid.ItemsSource = null;
                SummaryText.Text = string.Empty;
                StatusText.Text = "База даних порожня. Спочатку імпортуйте JSON-файл.";
                return;
            }

            IEnumerable result = query(readings);

            ResultsGrid.ItemsSource = null;
            ResultsGrid.Columns.Clear();
            ResultsGrid.ItemsSource = result;

            int rows = result.Cast<object>().Count();
            SummaryText.Text = $"{title}: рядків — {rows} (вимірювань у БД — {readings.Count})";
            StatusText.Text = "Готово.";
        }
        catch (Exception ex)
        {
            StatusText.Text = "Помилка аналітики.";
            MessageBox.Show(this, ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void ResultsGrid_AutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        if (e.PropertyDescriptor is PropertyDescriptor descriptor)
            e.Column.Header = descriptor.DisplayName;

        if (e.Column is DataGridTextColumn column && column.Binding is Binding binding)
        {
            if (e.PropertyType == typeof(double))
                binding.StringFormat = "N2";
            else if (e.PropertyType == typeof(DateTime))
                binding.StringFormat = "dd.MM.yyyy HH:mm:ss";
        }
    }
}
