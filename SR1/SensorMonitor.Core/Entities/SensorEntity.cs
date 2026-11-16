namespace SensorMonitor.Core.Entities;

public class SensorEntity
{
    public int Id { get; set; }
    public string SensorId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public int FactoryEntityId { get; set; }
    public FactoryEntity Factory { get; set; } = null!;

    public ICollection<ReadingEntity> Readings { get; set; } = new List<ReadingEntity>();
}
