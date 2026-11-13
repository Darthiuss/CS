namespace DeaneryEvents.Core;

public class GradeEventArgs : EventArgs
{
    public GradeEventArgs(Student student, int grade)
    {
        Student = student;
        Grade = grade;
    }

    public Student Student { get; }
    public int Grade { get; }
}

public class AcademicDebtEventArgs : EventArgs
{
    public AcademicDebtEventArgs(Student student, double average)
    {
        Student = student;
        Average = average;
    }

    public Student Student { get; }
    public double Average { get; }
}

public class GraduatedEventArgs : EventArgs
{
    public GraduatedEventArgs(Student student, double average, DateTime date)
    {
        Student = student;
        Average = average;
        Date = date;
    }

    public Student Student { get; }
    public double Average { get; }
    public DateTime Date { get; }
}

public class ExcellentStudentEventArgs : EventArgs
{
    public ExcellentStudentEventArgs(Student student, double average)
    {
        Student = student;
        Average = average;
    }

    public Student Student { get; }
    public double Average { get; }
}
