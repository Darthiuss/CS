namespace SensorMonitor.Core.Entities;

public class ReadingEntity
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public double Value { get; set; }
    public string Status { get; set; } = ReadingStatus.Normal;

    public int SensorEntityId { get; set; }
    public SensorEntity Sensor { get; set; } = null!;
}
