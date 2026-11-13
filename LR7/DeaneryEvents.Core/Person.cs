namespace DeaneryEvents.Core;

public abstract class Person : IPerson
{
    protected Person(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Ім'я не може бути порожнім.", nameof(name));

        Name = name.Trim();
    }

    public string Name { get; }

    public abstract string GetInfo();

    public override string ToString() => Name;
}
