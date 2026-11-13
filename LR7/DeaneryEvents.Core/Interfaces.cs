namespace DeaneryEvents.Core;

public interface IPerson
{
    string Name { get; }
    string GetInfo();
}

public interface IGradable
{
    IReadOnlyList<int> Grades { get; }
    double AverageScore { get; }
    void AddGrade(int grade);
}

public interface IGraduatable
{
    bool IsGraduated { get; }
    void Graduate();
}

public interface IReportable
{
    string GetReport();
}

public interface INotifiable
{
    void Subscribe(GradingSystem system);
    void Unsubscribe(GradingSystem system);
    void Watch(Student student);
    void Unwatch(Student student);
}
