using System.Collections.ObjectModel;

namespace DeaneryEvents.Core;

public class Journal
{
    public ObservableCollection<string> Entries { get; } = new();

    public void Write(string message)
    {
        Entries.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
    }

    public void Clear() => Entries.Clear();
}
