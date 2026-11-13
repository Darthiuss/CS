using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using DeaneryEvents.Core;

namespace DeaneryEvents.Wpf;

public partial class MainWindow : Window
{
    private readonly Journal _journal = new();
    private readonly GradingSystem _system;
    private readonly Teacher _teacher;
    private readonly DepartmentHead _head;
    private readonly ObservableCollection<Student> _students = new();

    public MainWindow()
    {
        InitializeComponent();

        _system = new GradingSystem(_journal);
        _teacher = new Teacher("Кудін В.І.", _journal);
        _head = new DepartmentHead("Петренко О.М.", _journal);

        _teacher.Subscribe(_system);
        _head.Subscribe(_system);

        StudentsList.ItemsSource = _students;
        LogList.ItemsSource = _journal.Entries;
        _journal.Entries.CollectionChanged += Entries_CollectionChanged;

        AddStudent("Іваненко Андрій");
        AddStudent("Коваль Марія");
        _journal.Write("Система запущена. Додайте оцінки студентам, щоб побачити події.");
    }

    private void Entries_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (LogList.Items.Count > 0)
            LogList.ScrollIntoView(LogList.Items[LogList.Items.Count - 1]);
    }

    private void AddStudent(string name)
    {
        var student = new Student(name, _journal);
        _system.Register(student);

        if (TeacherCheck.IsChecked == true) _teacher.Watch(student);
        if (HeadCheck.IsChecked == true) _head.Watch(student);

        _students.Add(student);
    }

    private Student? GetSelectedStudent()
    {
        if (StudentsList.SelectedItem is Student student)
            return student;

        MessageBox.Show("Спочатку виберіть студента у списку.", "Увага",
            MessageBoxButton.OK, MessageBoxImage.Information);
        return null;
    }

    private void AddStudentButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            AddStudent(NameBox.Text);
            _journal.Write($"Додано студента: {NameBox.Text.Trim()}");
            NameBox.Clear();
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private void AddGradeButton_Click(object sender, RoutedEventArgs e)
    {
        Student? student = GetSelectedStudent();
        if (student is null) return;

        if (!int.TryParse(GradeBox.Text, out int grade))
        {
            MessageBox.Show("Введіть оцінку цілим числом.", "Помилка",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            student.AddGrade(grade);
        }
        catch (Exception ex) when (ex is ArgumentOutOfRangeException or InvalidOperationException)
        {
            MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        GradeBox.Clear();
        StudentsList.Items.Refresh();
    }

    private void GraduateButton_Click(object sender, RoutedEventArgs e)
    {
        Student? student = GetSelectedStudent();
        if (student is null) return;

        try
        {
            student.Graduate();
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(ex.Message, "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        StudentsList.Items.Refresh();
    }

    private void PipelineButton_Click(object sender, RoutedEventArgs e)
    {
        Student? student = GetSelectedStudent();
        if (student is null) return;

        StudentMethod pipeline = StudentOperations.CreatePipeline(
            student, IncludeGradesCheck.IsChecked == true);

        _journal.Write($"Багатоадресний делегат: методів у ланцюжку — {pipeline.GetInvocationList().Length}");
        pipeline();
    }

    private void ClearButton_Click(object sender, RoutedEventArgs e) => _journal.Clear();

    private void TeacherCheck_Click(object sender, RoutedEventArgs e)
    {
        bool on = TeacherCheck.IsChecked == true;
        if (on)
        {
            _teacher.Subscribe(_system);
            foreach (Student s in _students) _teacher.Watch(s);
        }
        else
        {
            _teacher.Unsubscribe(_system);
            foreach (Student s in _students) _teacher.Unwatch(s);
        }

        _journal.Write($"Викладач {(on ? "підписаний на події" : "відписаний від подій")}");
    }

    private void HeadCheck_Click(object sender, RoutedEventArgs e)
    {
        bool on = HeadCheck.IsChecked == true;
        if (on)
        {
            _head.Subscribe(_system);
            foreach (Student s in _students) _head.Watch(s);
        }
        else
        {
            _head.Unsubscribe(_system);
            foreach (Student s in _students) _head.Unwatch(s);
        }

        _journal.Write($"Зав. кафедри {(on ? "підписаний на події" : "відписаний від подій")}");
    }
}
