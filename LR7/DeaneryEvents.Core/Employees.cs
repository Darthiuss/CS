namespace DeaneryEvents.Core;

public abstract class Employee : Person
{
    protected Employee(string name, string position, Journal journal) : base(name)
    {
        ArgumentNullException.ThrowIfNull(journal);
        Position = position;
        Log = journal;
    }

    public string Position { get; }

    protected Journal Log { get; }

    public override string GetInfo() => $"{Position}: {Name}";
}

public class Teacher : Employee, INotifiable
{
    public Teacher(string name, Journal journal) : base(name, "Викладач", journal)
    {
    }

    public void Subscribe(GradingSystem system)
    {
        ArgumentNullException.ThrowIfNull(system);
        system.AcademicDebtWarning -= OnAcademicDebtWarning;
        system.AcademicDebtWarning += OnAcademicDebtWarning;
    }

    public void Unsubscribe(GradingSystem system)
    {
        ArgumentNullException.ThrowIfNull(system);
        system.AcademicDebtWarning -= OnAcademicDebtWarning;
    }

    public void Watch(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        student.Graduated -= OnGraduated;
        student.Graduated += OnGraduated;
    }

    public void Unwatch(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        student.Graduated -= OnGraduated;
    }

    private void OnAcademicDebtWarning(object? sender, AcademicDebtEventArgs e)
    {
        Log.Write($"{Position} {Name}: Провести консультацію зі студентом {e.Student.Name} " +
                  $"(середній бал {e.Average:F1})");
    }

    private void OnGraduated(object? sender, GraduatedEventArgs e)
    {
        Log.Write($"{Position} {Name}: Вітаю студента {e.Student.Name} із випуском!");
    }
}

public class DepartmentHead : Employee, INotifiable
{
    public DepartmentHead(string name, Journal journal) : base(name, "Зав. кафедри", journal)
    {
    }

    public void Subscribe(GradingSystem system)
    {
        ArgumentNullException.ThrowIfNull(system);
        system.AcademicDebtWarning -= OnAcademicDebtWarning;
        system.AcademicDebtWarning += OnAcademicDebtWarning;
        system.ExcellentStudent -= OnExcellentStudent;
        system.ExcellentStudent += OnExcellentStudent;
    }

    public void Unsubscribe(GradingSystem system)
    {
        ArgumentNullException.ThrowIfNull(system);
        system.AcademicDebtWarning -= OnAcademicDebtWarning;
        system.ExcellentStudent -= OnExcellentStudent;
    }

    public void Watch(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        student.Graduated -= OnGraduated;
        student.Graduated += OnGraduated;
    }

    public void Unwatch(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        student.Graduated -= OnGraduated;
    }

    private void OnAcademicDebtWarning(object? sender, AcademicDebtEventArgs e)
    {
        Log.Write($"{Position} {Name}: Взяти на контроль студента {e.Student.Name} " +
                  $"(середній бал {e.Average:F1})");
    }

    private void OnExcellentStudent(object? sender, ExcellentStudentEventArgs e)
    {
        Log.Write($"{Position} {Name}: Студент {e.Student.Name} має відмінний результат " +
                  $"({e.Average:F1}) — рекомендувати до підвищеної стипендії");
    }

    private void OnGraduated(object? sender, GraduatedEventArgs e)
    {
        Log.Write($"{Position} {Name}: Підписати наказ про випуск студента {e.Student.Name}");
    }
}
