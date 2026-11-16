namespace SensorMonitor.Core.Dto;

public class SensorDto
{
    public string SensorId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public List<ReadingDto> Readings { get; set; } = new();
}
