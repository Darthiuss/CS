using Microsoft.EntityFrameworkCore;
using SensorMonitor.Core.Data;
using SensorMonitor.Core.Dto;
using SensorMonitor.Core.Entities;

namespace SensorMonitor.Core.Services;

public sealed class SqliteSensorRepository : ISensorRepository
{
    private readonly AppDbContext _context;

    public SqliteSensorRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
        => await _context.Database.EnsureCreatedAsync(cancellationToken);

    public async Task SaveFactoryAsync(FactoryDto factory, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(factory);

        await _context.Factories
            .Where(f => f.FactoryId == factory.FactoryId)
            .ExecuteDeleteAsync(cancellationToken);

        _context.Factories.Add(EntityMapper.ToEntity(factory));
        await _context.SaveChangesAsync(cancellationToken);

        _context.ChangeTracker.Clear();
    }

    public Task<List<ReadingEntity>> GetReadingsAsync(CancellationToken cancellationToken = default)
        => _context.Readings
            .AsNoTracking()
            .Include(r => r.Sensor)
            .ThenInclude(s => s.Factory)
            .OrderBy(r => r.Timestamp)
            .ToListAsync(cancellationToken);
}
