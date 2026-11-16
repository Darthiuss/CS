using System.ComponentModel;

namespace SensorMonitor.Core.Analytics;


public sealed record FactorySummary(
    [property: DisplayName("Завод")] string FactoryName,
    [property: DisplayName("Датчиків")] int SensorsCount,
    [property: DisplayName("Вимірювань")] int ReadingsCount,
    [property: DisplayName("Середнє значення")] double AverageValue,
    [property: DisplayName("Максимум")] double MaxValue,
    [property: DisplayName("Warning")] int WarningCount,
    [property: DisplayName("Critical")] int CriticalCount,
    [property: DisplayName("Не Normal, %")] double AbnormalPercent);

public sealed record SensorTypeStatistics(
    [property: DisplayName("Тип датчика")] string SensorType,
    [property: DisplayName("Вимірювань")] int ReadingsCount,
    [property: DisplayName("Мінімум")] double MinValue,
    [property: DisplayName("Максимум")] double MaxValue,
    [property: DisplayName("Середнє")] double AverageValue,
    [property: DisplayName("Станд. відхилення")] double StdDev,
    [property: DisplayName("Не Normal")] int AbnormalCount);

public sealed record AnomalyRecord(
    [property: DisplayName("Завод")] string FactoryName,
    [property: DisplayName("Датчик")] string SensorId,
    [property: DisplayName("Тип")] string SensorType,
    [property: DisplayName("Час (UTC)")] DateTime Timestamp,
    [property: DisplayName("Значення")] double Value,
    [property: DisplayName("Статус")] string Status,
    [property: DisplayName("Середнє датчика")] double Mean,
    [property: DisplayName("z-оцінка")] double ZScore);

public sealed record ProblematicSensor(
    [property: DisplayName("Завод")] string FactoryName,
    [property: DisplayName("Датчик")] string SensorId,
    [property: DisplayName("Тип")] string SensorType,
    [property: DisplayName("Вимірювань")] int ReadingsCount,
    [property: DisplayName("Warning")] int WarningCount,
    [property: DisplayName("Critical")] int CriticalCount,
    [property: DisplayName("Не Normal, %")] double AbnormalPercent);
