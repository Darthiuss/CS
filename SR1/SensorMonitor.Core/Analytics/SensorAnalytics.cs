using SensorMonitor.Core.Entities;

namespace SensorMonitor.Core.Analytics;

public static class SensorAnalytics
{
    public const int MinReadingsForStatistics = 3;

    public static IReadOnlyList<FactorySummary> GetFactorySummaries(this IEnumerable<ReadingEntity> readings)
    {
        ArgumentNullException.ThrowIfNull(readings);

        return readings
            .GroupBy(r => new { r.Sensor.Factory.FactoryId, r.Sensor.Factory.FactoryName })
            .Select(g =>
            {
                int total = g.Count();
                int abnormal = g.Count(IsAbnormal);

                return new FactorySummary(
                    g.Key.FactoryName,
                    g.Select(r => r.Sensor.SensorId).Distinct().Count(),
                    total,
                    Math.Round(g.Average(r => r.Value), 2),
                    g.Max(r => r.Value),
                    g.Count(r => IsStatus(r, ReadingStatus.Warning)),
                    g.Count(r => IsStatus(r, ReadingStatus.Critical)),
                    Math.Round(100.0 * abnormal / total, 2));
            })
            .OrderBy(s => s.FactoryName)
            .ToList();
    }

    public static IReadOnlyList<SensorTypeStatistics> GetSensorTypeStatistics(this IEnumerable<ReadingEntity> readings)
    {
        ArgumentNullException.ThrowIfNull(readings);

        return readings
            .GroupBy(r => r.Sensor.Type)
            .Select(g =>
            {
                var values = g.Select(r => r.Value).ToList();

                return new SensorTypeStatistics(
                    g.Key,
                    values.Count,
                    values.Min(),
                    values.Max(),
                    Math.Round(values.Average(), 2),
                    Math.Round(StdDev(values), 2),
                    g.Count(IsAbnormal));
            })
            .OrderBy(s => s.SensorType)
            .ToList();
    }

  
    public static IReadOnlyList<AnomalyRecord> GetStatisticalAnomalies(
        this IEnumerable<ReadingEntity> readings, double sigmaThreshold = 2.0)
    {
        ArgumentNullException.ThrowIfNull(readings);
        if (sigmaThreshold <= 0)
            throw new ArgumentOutOfRangeException(nameof(sigmaThreshold), "Поріг σ має бути додатним.");

        return readings
            .GroupBy(r => (r.Sensor.Factory.FactoryId, r.Sensor.SensorId))
            .SelectMany(g =>
            {
                var values = g.Select(r => r.Value).ToList();
                if (values.Count < MinReadingsForStatistics)
                    return Enumerable.Empty<AnomalyRecord>();

                double mean = values.Average();
                double sd = StdDev(values);
                if (sd == 0)
                    return Enumerable.Empty<AnomalyRecord>();

                return g
                    .Select(r => new { Reading = r, Z = (r.Value - mean) / sd })
                    .Where(x => Math.Abs(x.Z) > sigmaThreshold)
                    .Select(x => new AnomalyRecord(
                        x.Reading.Sensor.Factory.FactoryName,
                        x.Reading.Sensor.SensorId,
                        x.Reading.Sensor.Type,
                        x.Reading.Timestamp,
                        x.Reading.Value,
                        x.Reading.Status,
                        Math.Round(mean, 2),
                        Math.Round(x.Z, 2)));
            })
            .OrderByDescending(a => Math.Abs(a.ZScore))
            .ToList();
    }

    
    public static IReadOnlyList<ProblematicSensor> GetProblematicSensors(
        this IEnumerable<ReadingEntity> readings, double minAbnormalShare = 0.3)
    {
        ArgumentNullException.ThrowIfNull(readings);
        if (minAbnormalShare < 0 || minAbnormalShare > 1)
            throw new ArgumentOutOfRangeException(nameof(minAbnormalShare), "Частка має бути в діапазоні 0..1.");

        return readings
            .GroupBy(r => new
            {
                r.Sensor.Factory.FactoryId,
                r.Sensor.Factory.FactoryName,
                r.Sensor.SensorId,
                r.Sensor.Type
            })
            .Select(g => new
            {
                g.Key,
                Total = g.Count(),
                Warnings = g.Count(r => IsStatus(r, ReadingStatus.Warning)),
                Criticals = g.Count(r => IsStatus(r, ReadingStatus.Critical)),
                Abnormal = g.Count(IsAbnormal)
            })
            .Select(x => new { x.Key, x.Total, x.Warnings, x.Criticals, Share = (double)x.Abnormal / x.Total })
            .Where(x => x.Share >= minAbnormalShare && x.Share > 0)
            .OrderByDescending(x => x.Share)
            .ThenByDescending(x => x.Criticals)
            .Select(x => new ProblematicSensor(
                x.Key.FactoryName,
                x.Key.SensorId,
                x.Key.Type,
                x.Total,
                x.Warnings,
                x.Criticals,
                Math.Round(100.0 * x.Share, 2)))
            .ToList();
    }

    private static bool IsStatus(ReadingEntity reading, string status)
        => string.Equals(reading.Status, status, StringComparison.OrdinalIgnoreCase);

    private static bool IsAbnormal(ReadingEntity reading)
        => !IsStatus(reading, ReadingStatus.Normal);

    private static double StdDev(IReadOnlyCollection<double> values)
    {
        if (values.Count < 2)
            return 0;

        double mean = values.Average();
        return Math.Sqrt(values.Sum(v => (v - mean) * (v - mean)) / values.Count);
    }
}
