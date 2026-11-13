namespace DeaneryEvents.Core;

public delegate void StudentMethod();

public static class GradingRules
{
    public const int MinGrade = 0;
    public const int MaxGrade = 100;
    public const double PassingScore = 60;
    public const double ExcellentScore = 90;
}

public class Student : Person, IGradable, IGraduatable, IReportable
{
    private readonly List<int> _grades = new();
    private readonly Journal _journal;

    public Student(string name, Journal journal) : base(name)
    {
        ArgumentNullException.ThrowIfNull(journal);
        _journal = journal;
    }

    // події

    public event EventHandler<GradeEventArgs>? GradeAdded;
    public event EventHandler<GraduatedEventArgs>? Graduated;

    public IReadOnlyList<int> Grades => _grades;

    public double AverageScore => _grades.Count == 0 ? 0 : _grades.Average();

    public void AddGrade(int grade)
    {
        if (grade < GradingRules.MinGrade || grade > GradingRules.MaxGrade)
            throw new ArgumentOutOfRangeException(nameof(grade),
                $"Оцінка має бути в межах {GradingRules.MinGrade}–{GradingRules.MaxGrade}.");

        if (IsGraduated)
            throw new InvalidOperationException("Студент уже випустився — оцінки не додаються.");

        _grades.Add(grade);
        OnGradeAdded(new GradeEventArgs(this, grade));
    }

    public bool IsGraduated { get; private set; }

    public void Graduate()
    {
        if (IsGraduated)
            throw new InvalidOperationException("Студент уже випущений.");

        if (_grades.Count == 0 || AverageScore < GradingRules.PassingScore)
            throw new InvalidOperationException(
                $"Не можна випустити студента зі середнім балом нижче {GradingRules.PassingScore}.");

        IsGraduated = true;
        OnGraduated(new GraduatedEventArgs(this, AverageScore, DateTime.Now));
    }

    public string GetReport()
    {
        string status = IsGraduated ? "випущений" : "навчається";
        return $"Звіт: {Name}, оцінок: {_grades.Count}, середній бал: {AverageScore:F1}, статус: {status}";
    }

    public override string GetInfo() => $"Студент: {Name}";

    public void ShowInfo() => _journal.Write(GetInfo());

    public void ShowGrades()
    {
        string list = _grades.Count == 0 ? "оцінок ще немає" : string.Join(", ", _grades);
        _journal.Write($"Оцінки студента {Name}: {list}");
    }

    public void ShowReport() => _journal.Write(GetReport());

    protected virtual void OnGradeAdded(GradeEventArgs e) => GradeAdded?.Invoke(this, e);
    protected virtual void OnGraduated(GraduatedEventArgs e) => Graduated?.Invoke(this, e);

    public override string ToString()
    {
        string suffix = IsGraduated ? " (випущений)" : string.Empty;
        return $"{Name} — середній бал: {AverageScore:F1}{suffix}";
    }
}
