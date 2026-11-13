namespace DeaneryEvents.Core;


public class GradingSystem
{
    private readonly Journal _journal;
    private readonly List<Student> _students = new();

    public GradingSystem(Journal journal)
    {
        ArgumentNullException.ThrowIfNull(journal);
        _journal = journal;
    }

    public IReadOnlyList<Student> Students => _students;

    public event EventHandler<AcademicDebtEventArgs>? AcademicDebtWarning;
    public event EventHandler<ExcellentStudentEventArgs>? ExcellentStudent;

    public void Register(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        if (_students.Contains(student)) return;

        _students.Add(student);
        student.GradeAdded += OnGradeAdded;
        student.Graduated += OnGraduated;
    }

    public void Unregister(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);
        if (!_students.Remove(student)) return;

        student.GradeAdded -= OnGradeAdded;
        student.Graduated -= OnGraduated;
    }

    private void OnGradeAdded(object? sender, GradeEventArgs e)
    {
        double average = e.Student.AverageScore;
        _journal.Write($"Система оцінювання: {e.Student.Name} отримав {e.Grade}, середній бал {average:F1}");

        if (average < GradingRules.PassingScore)
            OnAcademicDebtWarning(new AcademicDebtEventArgs(e.Student, average));
        else if (average >= GradingRules.ExcellentScore)
            OnExcellentStudent(new ExcellentStudentEventArgs(e.Student, average));
    }

    private void OnGraduated(object? sender, GraduatedEventArgs e)
    {
        _journal.Write($"Система оцінювання: студент {e.Student.Name} випустився " +
                       $"({e.Date:dd.MM.yyyy}), середній бал {e.Average:F1}");
    }


    protected virtual void OnAcademicDebtWarning(AcademicDebtEventArgs e)
        => AcademicDebtWarning?.Invoke(this, e);
    protected virtual void OnExcellentStudent(ExcellentStudentEventArgs e)
        => ExcellentStudent?.Invoke(this, e);
}
