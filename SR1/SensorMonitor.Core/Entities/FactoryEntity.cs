namespace SensorMonitor.Core.Entities;

public class FactoryEntity
{
    public int Id { get; set; }
    public string FactoryId { get; set; } = string.Empty;
    public string FactoryName { get; set; } = string.Empty;

    public ICollection<SensorEntity> Sensors { get; set; } = new List<SensorEntity>();
}
