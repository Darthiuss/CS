using DeaneryEvents.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DeaneryEvents.Tests;

[TestClass]
public class StudentTests
{
    private static Student CreateStudent(Journal? journal = null)
        => new("Тестовий Студент", journal ?? new Journal());

    [TestMethod]
    public void AddGrade_UpdatesAverageScore()
    {
        Student s = CreateStudent();
        s.AddGrade(80);
        s.AddGrade(60);

        Assert.AreEqual(70.0, s.AverageScore, 0.0001);
        Assert.AreEqual(2, s.Grades.Count);
    }

    [TestMethod]
    public void AverageScore_WithoutGrades_IsZero()
    {
        Assert.AreEqual(0.0, CreateStudent().AverageScore);
    }

    [TestMethod]
    [DataRow(-1)]
    [DataRow(101)]
    public void AddGrade_OutOfRange_Throws(int grade)
    {
        Student s = CreateStudent();
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => s.AddGrade(grade));
    }

    [TestMethod]
    public void Constructor_EmptyName_Throws()
    {
        Assert.ThrowsException<ArgumentException>(() => new Student("  ", new Journal()));
    }

    [TestMethod]
    public void Student_Implements_AtLeastThreeInterfaces()
    {
        Type[] interfaces = typeof(Student).GetInterfaces();

        Assert.IsTrue(interfaces.Length >= 3);
        CollectionAssert.IsSubsetOf(
            new[] { typeof(IGradable), typeof(IGraduatable), typeof(IReportable) },
            interfaces);
    }

    [TestMethod]
    public void Graduate_WithPassingAverage_RaisesGraduatedEvent()
    {
        Student s = CreateStudent();
        s.AddGrade(75);
        int raised = 0;
        s.Graduated += (_, e) =>
        {
            raised++;
            Assert.AreSame(s, e.Student);
        };

        s.Graduate();

        Assert.IsTrue(s.IsGraduated);
        Assert.AreEqual(1, raised);
    }

    [TestMethod]
    public void Graduate_WithLowAverage_Throws()
    {
        Student s = CreateStudent();
        s.AddGrade(30);

        Assert.ThrowsException<InvalidOperationException>(() => s.Graduate());
        Assert.IsFalse(s.IsGraduated);
    }

    [TestMethod]
    public void Graduate_Twice_Throws()
    {
        Student s = CreateStudent();
        s.AddGrade(90);
        s.Graduate();

        Assert.ThrowsException<InvalidOperationException>(() => s.Graduate());
    }

    [TestMethod]
    public void AddGrade_AfterGraduation_Throws()
    {
        Student s = CreateStudent();
        s.AddGrade(90);
        s.Graduate();

        Assert.ThrowsException<InvalidOperationException>(() => s.AddGrade(50));
    }

    [TestMethod]
    public void GradeAdded_Event_IsRaisedWithGrade()
    {
        Student s = CreateStudent();
        int received = -1;
        s.GradeAdded += (_, e) => received = e.Grade;

        s.AddGrade(88);

        Assert.AreEqual(88, received);
    }
}

[TestClass]
public class MulticastDelegateTests
{
    [TestMethod]
    public void Pipeline_ContainsThreeMethods_AndWritesThreeEntries()
    {
        var journal = new Journal();
        var s = new Student("Іваненко", journal);
        s.AddGrade(70);

        StudentMethod pipeline = StudentOperations.CreatePipeline(s);
        pipeline();

        Assert.AreEqual(3, pipeline.GetInvocationList().Length);
        Assert.AreEqual(3, journal.Entries.Count);
    }

    [TestMethod]
    public void Pipeline_WithoutGrades_ContainsTwoMethods()
    {
        var journal = new Journal();
        var s = new Student("Іваненко", journal);

        StudentMethod pipeline = StudentOperations.CreatePipeline(s, includeGrades: false);
        pipeline();

        Assert.AreEqual(2, pipeline.GetInvocationList().Length);
        Assert.AreEqual(2, journal.Entries.Count);
    }

    [TestMethod]
    public void Delegate_AddAndRemoveMethods_ChangesInvocationList()
    {
        var s = new Student("Іваненко", new Journal());

        StudentMethod chain = s.ShowInfo;
        chain += s.ShowReport;
        Assert.AreEqual(2, chain.GetInvocationList().Length);

        chain -= s.ShowReport;
        Assert.AreEqual(1, chain.GetInvocationList().Length);
    }
}

[TestClass]
public class GradingSystemEventTests
{
    private Journal _journal = null!;
    private GradingSystem _system = null!;
    private Teacher _teacher = null!;
    private DepartmentHead _head = null!;

    [TestInitialize]
    public void Setup()
    {
        _journal = new Journal();
        _system = new GradingSystem(_journal);
        _teacher = new Teacher("Кудін В.І.", _journal);
        _head = new DepartmentHead("Петренко О.М.", _journal);
    }

    private Student NewRegisteredStudent()
    {
        var s = new Student("Іваненко", _journal);
        _system.Register(s);
        return s;
    }

    private bool JournalContains(string text) => _journal.Entries.Any(e => e.Contains(text));

    [TestMethod]
    public void LowAverage_RaisesAcademicDebtWarning_ForAllSubscribers()
    {
        _teacher.Subscribe(_system);
        _head.Subscribe(_system);
        Student s = NewRegisteredStudent();

        s.AddGrade(40);

        Assert.IsTrue(JournalContains("Провести консультацію"));
        Assert.IsTrue(JournalContains("Взяти на контроль"));
    }

    [TestMethod]
    public void AcademicDebtWarning_IsBroadcast_AllHandlersCalled()
    {
        int calls = 0;
        _system.AcademicDebtWarning += (_, _) => calls++;
        _system.AcademicDebtWarning += (_, _) => calls++;
        _system.AcademicDebtWarning += (_, _) => calls++;
        Student s = NewRegisteredStudent();

        s.AddGrade(10);

        Assert.AreEqual(3, calls);
    }

    [TestMethod]
    public void AverageEqualToThreshold_DoesNotRaiseWarning()
    {
        int calls = 0;
        _system.AcademicDebtWarning += (_, _) => calls++;
        Student s = NewRegisteredStudent();

        s.AddGrade(60);

        Assert.AreEqual(0, calls);
    }

    [TestMethod]
    public void AcademicDebtWarning_PassesStudentAndAverage()
    {
        AcademicDebtEventArgs? args = null;
        _system.AcademicDebtWarning += (_, e) => args = e;
        Student s = NewRegisteredStudent();

        s.AddGrade(50);

        Assert.IsNotNull(args);
        Assert.AreSame(s, args!.Student);
        Assert.AreEqual(50.0, args.Average, 0.0001);
    }

    [TestMethod]
    public void Unsubscribe_TeacherNoLongerNotified()
    {
        _teacher.Subscribe(_system);
        _head.Subscribe(_system);
        _teacher.Unsubscribe(_system);
        Student s = NewRegisteredStudent();

        s.AddGrade(20);

        Assert.IsFalse(JournalContains("Провести консультацію"));
        Assert.IsTrue(JournalContains("Взяти на контроль"));
    }

    [TestMethod]
    public void Subscribe_Twice_DoesNotDuplicateNotifications()
    {
        _teacher.Subscribe(_system);
        _teacher.Subscribe(_system);
        Student s = NewRegisteredStudent();

        s.AddGrade(20);

        Assert.AreEqual(1, _journal.Entries.Count(e => e.Contains("Провести консультацію")));
    }

    [TestMethod]
    public void HighAverage_RaisesExcellentStudent_ForDepartmentHead()
    {
        _head.Subscribe(_system);
        Student s = NewRegisteredStudent();

        s.AddGrade(95);

        Assert.IsTrue(JournalContains("відмінний результат"));
    }

    [TestMethod]
    public void Graduated_IsBroadcast_ToTeacherAndHead()
    {
        _teacher.Watch(NewRegisteredStudentAndGet(out Student s));
        _head.Watch(s);
        s.AddGrade(80);

        s.Graduate();

        Assert.IsTrue(JournalContains("Вітаю студента"));
        Assert.IsTrue(JournalContains("Підписати наказ про випуск"));
        Assert.IsTrue(JournalContains("випустився"));   // повідомлення самої системи
    }

    [TestMethod]
    public void Unregister_StopsTrackingStudent()
    {
        int calls = 0;
        _system.AcademicDebtWarning += (_, _) => calls++;
        Student s = NewRegisteredStudent();
        _system.Unregister(s);

        s.AddGrade(10);

        Assert.AreEqual(0, calls);
    }

    private Student NewRegisteredStudentAndGet(out Student student)
    {
        student = NewRegisteredStudent();
        return student;
    }
}

[TestClass]
public class HierarchyTests
{
    [TestMethod]
    public void Hierarchy_IsBuiltOnPerson()
    {
        var journal = new Journal();
        IPerson[] people =
        {
            new Student("Студент", journal),
            new Teacher("Викладач", journal),
            new DepartmentHead("Завкафедри", journal)
        };

        Assert.IsTrue(people.All(p => p is Person));
        Assert.IsTrue(people[1] is Employee);
        Assert.IsTrue(people[2] is Employee);
        StringAssert.StartsWith(people[0].GetInfo(), "Студент");
        StringAssert.StartsWith(people[1].GetInfo(), "Викладач");
        StringAssert.StartsWith(people[2].GetInfo(), "Зав. кафедри");
    }
}
