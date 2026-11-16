using SensorMonitor.Core.Dto;
using SensorMonitor.Core.Entities;

namespace SensorMonitor.Core.Services;


public sealed class SensorCatalog
{
    private readonly IFileLoader<FactoryDto> _loader;
    private readonly ISensorRepository _repository;

    public SensorCatalog(IFileLoader<FactoryDto> loader, ISensorRepository repository)
    {
        _loader = loader ?? throw new ArgumentNullException(nameof(loader));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<ImportProgress> ImportAsync(
        string filePath,
        IProgress<ImportProgress>? progress = null,
        CancellationToken cancellationToken = default)
    {
        await _repository.InitializeAsync(cancellationToken);

        int factories = 0, sensors = 0, readings = 0;

        await foreach (var factory in _loader.LoadAsync(filePath, cancellationToken)
                           .WithCancellation(cancellationToken))
        {
            await _repository.SaveFactoryAsync(factory, cancellationToken);

            factories++;
            sensors += factory.Sensors.Count;
            readings += factory.Sensors.Sum(s => s.Readings.Count);
            progress?.Report(new ImportProgress(factories, sensors, readings));
        }

        return new ImportProgress(factories, sensors, readings);
    }

    public async Task<List<ReadingEntity>> GetReadingsAsync(CancellationToken cancellationToken = default)
    {
        await _repository.InitializeAsync(cancellationToken);
        return await _repository.GetReadingsAsync(cancellationToken);
    }
}
