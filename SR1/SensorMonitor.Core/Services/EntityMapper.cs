using SensorMonitor.Core.Dto;
using SensorMonitor.Core.Entities;

namespace SensorMonitor.Core.Services;

public static class EntityMapper
{
    public static FactoryEntity ToEntity(FactoryDto dto) => new()
    {
        FactoryId = dto.FactoryId,
        FactoryName = dto.FactoryName,
        Sensors = dto.Sensors.Select(s => new SensorEntity
        {
            SensorId = s.SensorId,
            Type = s.Type,
            Readings = s.Readings.Select(r => new ReadingEntity
            {
                Timestamp = r.Timestamp,
                Value = r.Value,
                Status = r.Status
            }).ToList()
        }).ToList()
    };
}
