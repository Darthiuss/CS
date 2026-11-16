using System.Runtime.CompilerServices;
using System.Text.Json;

namespace SensorMonitor.Core.Services;

public sealed class JsonFileReader<T> : IFileLoader<T>
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public async IAsyncEnumerable<T> LoadAsync(
        string filePath,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("Шлях до файлу не вказано.", nameof(filePath));

        cancellationToken.ThrowIfCancellationRequested();

        await using var stream = new FileStream(
            filePath, FileMode.Open, FileAccess.Read, FileShare.Read,
            bufferSize: 4096, useAsync: true);

        await foreach (var item in JsonSerializer
                           .DeserializeAsyncEnumerable<T>(stream, Options, cancellationToken)
                           .WithCancellation(cancellationToken))
        {
            if (item is not null)
                yield return item;
        }
    }
}
