namespace DeaneryEvents.Core;

public static class StudentOperations
{
    public static StudentMethod CreatePipeline(Student student, bool includeGrades = true)
    {
        ArgumentNullException.ThrowIfNull(student);

        StudentMethod chain = student.ShowInfo;
        chain += student.ShowGrades;
        chain += student.ShowReport;

        if (!includeGrades)
            chain -= student.ShowGrades;

        return chain;
    }
}
