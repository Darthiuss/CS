namespace SensorMonitor.Core.Dto;

public class ReadingDto
{
    public DateTime Timestamp { get; set; }
    public double Value { get; set; }
    public string Status { get; set; } = string.Empty;
}
