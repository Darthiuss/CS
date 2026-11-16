namespace SensorMonitor.Core.Services;

public interface IFileLoader<T>
{
    IAsyncEnumerable<T> LoadAsync(string filePath, CancellationToken cancellationToken = default);
}
