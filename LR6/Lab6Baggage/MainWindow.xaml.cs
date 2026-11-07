using System.Globalization;
using System.IO;
using System.Windows;

namespace Lab6Baggage
{
    public partial class MainWindow : Window
    {
        private readonly BaggageCatalog _catalog;

        public MainWindow()
        {
            InitializeComponent();

            // Визначаємо шлях до файлу
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = Path.Combine(documentsPath, "baggage.json");

            // Ініціалізуємо клас бізнес-логіки
            _catalog = new BaggageCatalog(new JsonFileWriter(), new JsonFileReader(), filePath);

            // Прив'язуємо колекцію до DataGrid
            BaggageDataGrid.ItemsSource = _catalog.Items;

            // Оновлюємо статистику щоразу, коли змінюється відображувана колекція
            _catalog.Items.CollectionChanged += (s, e) => UpdateStats();
            UpdateStats();
        }

        private void UpdateStats()
        {
            double max = _catalog.GetMaxWeight();
            double avg = _catalog.GetAverageWeight();
            StatsLabel.Content = $"Максимальна вага: {max:0.##} кг | Середня вага: {avg:0.##} кг";
        }

        // Розбір дійсного числа: приймає і крапку, і кому як роздільник
        private static bool TryParseWeight(string? text, out double value)
        {
            string normalized = (text ?? string.Empty).Trim().Replace(',', '.');
            return double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!int.TryParse(ItemsCountTextBox.Text, out int itemsCount))
                {
                    throw new FormatException("Кількість речей має бути цілим числом.");
                }
                if (!TryParseWeight(WeightTextBox.Text, out double weight))
                {
                    throw new FormatException("Загальна вага має бути числом.");
                }

                var newItem = new Baggage(FullNameTextBox.Text, itemsCount, weight);
                _catalog.AddBaggage(newItem);

                // Очищуємо поля
                FullNameTextBox.Clear();
                ItemsCountTextBox.Clear();
                WeightTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (BaggageDataGrid.SelectedItem is Baggage selectedItem)
            {
                _catalog.DeleteBaggage(selectedItem);
            }
        }

        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            _catalog.SortByItemsCount();
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FilterWeightTextBox.Text))
            {
                _catalog.ResetFilter();
            }
            else if (TryParseWeight(FilterWeightTextBox.Text, out double weight))
            {
                _catalog.FilterByWeight(weight);
            }
            else
            {
                MessageBox.Show("Будь ласка, введіть коректну вагу.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _catalog.SaveAsync();
                MessageBox.Show("Відомість успішно збережено.", "Збереження", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не вдалося зберегти файл: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool loaded = await _catalog.LoadAsync();
                if (loaded)
                {
                    MessageBox.Show("Відомість успішно завантажено.", "Завантаження", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Файл не знайдено. Спочатку збережіть відомість.", "Завантаження", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не вдалося прочитати файл: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ---------- Обробники LINQ-запитів ----------

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                _catalog.ResetFilter();
            }
            else
            {
                _catalog.SearchByName(SearchTextBox.Text);
            }
        }

        private void UniqueSurnamesButton_Click(object sender, RoutedEventArgs e)
        {
            var surnames = _catalog.GetUniqueSurnames();
            if (surnames.Count == 0)
            {
                MessageBox.Show("Відомість порожня.", "Унікальні прізвища", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string message = $"Кількість унікальних прізвищ: {surnames.Count}{Environment.NewLine}{Environment.NewLine}"
                             + string.Join(Environment.NewLine, surnames);
            MessageBox.Show(message, "Унікальні прізвища", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void HeaviestButton_Click(object sender, RoutedEventArgs e)
        {
            _catalog.FindThreeHeaviest();
        }

        private void AverageWeightPerItemButton_Click(object sender, RoutedEventArgs e)
        {
            double average = _catalog.CalculateAverageWeightPerItem();
            MessageBox.Show($"Середня вага однієї речі: {average:0.##} кг", "Середня вага речі",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void GroupByCountButton_Click(object sender, RoutedEventArgs e)
        {
            var groups = _catalog.GroupByItemsCount();
            if (groups.Count == 0)
            {
                MessageBox.Show("Відомість порожня.", "Групування", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var lines = groups.Select(g =>
                $"Речей: {g.Key} – пасажирів: {g.Count()} ({string.Join(", ", g.Select(b => b.FullName))})");
            MessageBox.Show(string.Join(Environment.NewLine, lines), "Групування за кількістю речей",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
