using SensorMonitor.Core.Entities;

namespace SensorMonitor.Tests;

internal static class TestData
{
    private static readonly DateTime Start = new(2026, 9, 1, 10, 0, 0, DateTimeKind.Utc);

    public static FactoryEntity NewFactory(string id, string name) =>
        new() { FactoryId = id, FactoryName = name };

    public static SensorEntity NewSensor(FactoryEntity factory, string id, string type)
    {
        var sensor = new SensorEntity { SensorId = id, Type = type, Factory = factory };
        factory.Sensors.Add(sensor);
        return sensor;
    }

    public static ReadingEntity AddReading(
        SensorEntity sensor, double value, string status = ReadingStatus.Normal, int minute = 0)
    {
        var reading = new ReadingEntity
        {
            Sensor = sensor,
            Value = value,
            Status = status,
            Timestamp = Start.AddMinutes(minute)
        };
        sensor.Readings.Add(reading);
        return reading;
    }

    public static List<ReadingEntity> All(params SensorEntity[] sensors) =>
        sensors.SelectMany(s => s.Readings).ToList();
}
