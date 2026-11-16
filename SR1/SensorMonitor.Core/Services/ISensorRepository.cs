using SensorMonitor.Core.Dto;
using SensorMonitor.Core.Entities;

namespace SensorMonitor.Core.Services;

public interface ISensorRepository
{
    Task InitializeAsync(CancellationToken cancellationToken = default);

    Task SaveFactoryAsync(FactoryDto factory, CancellationToken cancellationToken = default);

    Task<List<ReadingEntity>> GetReadingsAsync(CancellationToken cancellationToken = default);
}
