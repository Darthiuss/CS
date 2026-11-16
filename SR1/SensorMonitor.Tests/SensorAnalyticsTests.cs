using SensorMonitor.Core.Analytics;
using SensorMonitor.Core.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SensorMonitor.Tests;

[TestClass]
public class SensorAnalyticsTests
{
    // ---------- 1. GetFactorySummaries ----------

    [TestMethod]
    public void GetFactorySummaries_GroupsByFactoryAndCalculatesAggregates()
    {
        var factoryA = TestData.NewFactory("FAC-A", "Завод A");
        var factoryB = TestData.NewFactory("FAC-B", "Завод B");
        var s1 = TestData.NewSensor(factoryA, "S1", "Temperature");
        var s2 = TestData.NewSensor(factoryB, "S2", "Pressure");

        TestData.AddReading(s1, 10, ReadingStatus.Normal);
        TestData.AddReading(s1, 20, ReadingStatus.Warning);
        TestData.AddReading(s1, 30, ReadingStatus.Critical);
        TestData.AddReading(s2, 5, ReadingStatus.Normal);

        var result = TestData.All(s1, s2).GetFactorySummaries();

        Assert.AreEqual(2, result.Count);

        var a = result.Single(r => r.FactoryName == "Завод A");
        Assert.AreEqual(1, a.SensorsCount);
        Assert.AreEqual(3, a.ReadingsCount);
        Assert.AreEqual(20.0, a.AverageValue, 0.001);
        Assert.AreEqual(30.0, a.MaxValue, 0.001);
        Assert.AreEqual(1, a.WarningCount);
        Assert.AreEqual(1, a.CriticalCount);
        Assert.AreEqual(66.67, a.AbnormalPercent, 0.001);

        var b = result.Single(r => r.FactoryName == "Завод B");
        Assert.AreEqual(1, b.ReadingsCount);
        Assert.AreEqual(0, b.WarningCount);
        Assert.AreEqual(0, b.CriticalCount);
        Assert.AreEqual(0.0, b.AbnormalPercent, 0.001);
    }

    [TestMethod]
    public void GetFactorySummaries_CountsDistinctSensors()
    {
        var factory = TestData.NewFactory("FAC-A", "Завод A");
        var s1 = TestData.NewSensor(factory, "S1", "Temperature");
        var s2 = TestData.NewSensor(factory, "S2", "Temperature");
        TestData.AddReading(s1, 1);
        TestData.AddReading(s1, 2);
        TestData.AddReading(s2, 3);

        var result = TestData.All(s1, s2).GetFactorySummaries();

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(2, result[0].SensorsCount);
        Assert.AreEqual(3, result[0].ReadingsCount);
    }

    [TestMethod]
    public void GetFactorySummaries_EmptyInput_ReturnsEmptyList()
    {
        var result = new List<ReadingEntity>().GetFactorySummaries();

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetFactorySummaries_NullInput_Throws()
    {
        Assert.ThrowsException<ArgumentNullException>(
            () => ((IEnumerable<ReadingEntity>)null!).GetFactorySummaries());
    }

    // ---------- 2. GetSensorTypeStatistics ----------

    [TestMethod]
    public void GetSensorTypeStatistics_CalculatesMinMaxAverageAndStdDev()
    {
        var factory = TestData.NewFactory("FAC-A", "Завод A");
        var temp = TestData.NewSensor(factory, "S1", "Temperature");
        var pressure = TestData.NewSensor(factory, "S2", "Pressure");

        TestData.AddReading(temp, 10);
        TestData.AddReading(temp, 20, ReadingStatus.Warning);
        TestData.AddReading(temp, 30);
        TestData.AddReading(pressure, 7);

        var result = TestData.All(temp, pressure).GetSensorTypeStatistics();

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Pressure", result[0].SensorType); // сортування за назвою типу

        var t = result.Single(r => r.SensorType == "Temperature");
        Assert.AreEqual(3, t.ReadingsCount);
        Assert.AreEqual(10.0, t.MinValue, 0.001);
        Assert.AreEqual(30.0, t.MaxValue, 0.001);
        Assert.AreEqual(20.0, t.AverageValue, 0.001);
        Assert.AreEqual(8.16, t.StdDev, 0.001); // sqrt(200/3) ≈ 8.165
        Assert.AreEqual(1, t.AbnormalCount);

        var p = result.Single(r => r.SensorType == "Pressure");
        Assert.AreEqual(1, p.ReadingsCount);
        Assert.AreEqual(0.0, p.StdDev, 0.001); // одне значення — відхилення 0
    }

    [TestMethod]
    public void GetSensorTypeStatistics_MergesSensorsOfSameTypeAcrossFactories()
    {
        var f1 = TestData.NewFactory("F1", "Завод 1");
        var f2 = TestData.NewFactory("F2", "Завод 2");
        var s1 = TestData.NewSensor(f1, "S1", "Temperature");
        var s2 = TestData.NewSensor(f2, "S2", "Temperature");
        TestData.AddReading(s1, 10);
        TestData.AddReading(s2, 30);

        var result = TestData.All(s1, s2).GetSensorTypeStatistics();

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(2, result[0].ReadingsCount);
        Assert.AreEqual(20.0, result[0].AverageValue, 0.001);
    }

    [TestMethod]
    public void GetSensorTypeStatistics_EmptyInput_ReturnsEmptyList()
    {
        Assert.AreEqual(0, new List<ReadingEntity>().GetSensorTypeStatistics().Count);
    }

    // ---------- 3. GetStatisticalAnomalies ----------

    [TestMethod]
    public void GetStatisticalAnomalies_DetectsSpike()
    {
        var factory = TestData.NewFactory("FAC-A", "Завод A");
        var sensor = TestData.NewSensor(factory, "S1", "Temperature");
        for (int i = 0; i < 9; i++)
            TestData.AddReading(sensor, 50, minute: i);
        TestData.AddReading(sensor, 100, ReadingStatus.Critical, minute: 9);

        // mean = 55, σ = 15, z(100) = 3
        var result = TestData.All(sensor).GetStatisticalAnomalies(2.0);

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(100.0, result[0].Value, 0.001);
        Assert.AreEqual(3.0, result[0].ZScore, 0.001);
        Assert.AreEqual(55.0, result[0].Mean, 0.001);
        Assert.AreEqual("S1", result[0].SensorId);
        Assert.AreEqual("Завод A", result[0].FactoryName);
    }

    [TestMethod]
    public void GetStatisticalAnomalies_HigherThresholdFiltersOutSpike()
    {
        var factory = TestData.NewFactory("FAC-A", "Завод A");
        var sensor = TestData.NewSensor(factory, "S1", "Temperature");
        for (int i = 0; i < 9; i++)
            TestData.AddReading(sensor, 50, minute: i);
        TestData.AddReading(sensor, 100, minute: 9);

        var result = TestData.All(sensor).GetStatisticalAnomalies(3.5);

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetStatisticalAnomalies_ConstantValues_ReturnsEmpty()
    {
        var factory = TestData.NewFactory("FAC-A", "Завод A");
        var sensor = TestData.NewSensor(factory, "S1", "Temperature");
        for (int i = 0; i < 6; i++)
            TestData.AddReading(sensor, 42, minute: i);

        Assert.AreEqual(0, TestData.All(sensor).GetStatisticalAnomalies().Count);
    }

    [TestMethod]
    public void GetStatisticalAnomalies_TooFewReadings_ReturnsEmpty()
    {
        var factory = TestData.NewFactory("FAC-A", "Завод A");
        var sensor = TestData.NewSensor(factory, "S1", "Temperature");
        TestData.AddReading(sensor, 10);
        TestData.AddReading(sensor, 1000);

        Assert.AreEqual(0, TestData.All(sensor).GetStatisticalAnomalies(0.5).Count);
    }

    [TestMethod]
    public void GetStatisticalAnomalies_EvaluatesEachSensorSeparately()
    {
        var factory = TestData.NewFactory("FAC-A", "Завод A");
        var normal = TestData.NewSensor(factory, "S1", "Temperature");
        var spiky = TestData.NewSensor(factory, "S2", "Temperature");
        for (int i = 0; i < 9; i++)
        {
            TestData.AddReading(normal, 100, minute: i); // значення 100 — норма для S1
            TestData.AddReading(spiky, 50, minute: i);
        }
        TestData.AddReading(normal, 100, minute: 9);
        TestData.AddReading(spiky, 100, minute: 9);

        var result = TestData.All(normal, spiky).GetStatisticalAnomalies(2.0);

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("S2", result[0].SensorId);
    }

    [TestMethod]
    public void GetStatisticalAnomalies_NonPositiveSigma_Throws()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new List<ReadingEntity>().GetStatisticalAnomalies(0));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new List<ReadingEntity>().GetStatisticalAnomalies(-1));
    }

    // ---------- 4. GetProblematicSensors ----------

    [TestMethod]
    public void GetProblematicSensors_FiltersByShareAndSortsDescending()
    {
        var factory = TestData.NewFactory("FAC-A", "Завод A");
        var half = TestData.NewSensor(factory, "S1", "Temperature");
        var healthy = TestData.NewSensor(factory, "S2", "Pressure");
        var bad = TestData.NewSensor(factory, "S3", "Vibration");

        TestData.AddReading(half, 1, ReadingStatus.Normal);
        TestData.AddReading(half, 2, ReadingStatus.Normal);
        TestData.AddReading(half, 3, ReadingStatus.Warning);
        TestData.AddReading(half, 4, ReadingStatus.Critical);

        for (int i = 0; i < 5; i++)
            TestData.AddReading(healthy, 1, ReadingStatus.Normal);

        TestData.AddReading(bad, 1, ReadingStatus.Warning);
        TestData.AddReading(bad, 2, ReadingStatus.Warning);

        var result = TestData.All(half, healthy, bad).GetProblematicSensors(0.3);

        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("S3", result[0].SensorId);
        Assert.AreEqual(100.0, result[0].AbnormalPercent, 0.001);

        Assert.AreEqual("S1", result[1].SensorId);
        Assert.AreEqual(4, result[1].ReadingsCount);
        Assert.AreEqual(1, result[1].WarningCount);
        Assert.AreEqual(1, result[1].CriticalCount);
        Assert.AreEqual(50.0, result[1].AbnormalPercent, 0.001);
    }

    [TestMethod]
    public void GetProblematicSensors_ZeroThreshold_StillExcludesFullyHealthySensors()
    {
        var factory = TestData.NewFactory("FAC-A", "Завод A");
        var healthy = TestData.NewSensor(factory, "S1", "Pressure");
        TestData.AddReading(healthy, 1);
        TestData.AddReading(healthy, 2);

        Assert.AreEqual(0, TestData.All(healthy).GetProblematicSensors(0).Count);
    }

    [TestMethod]
    public void GetProblematicSensors_StatusComparisonIsCaseInsensitive()
    {
        var factory = TestData.NewFactory("FAC-A", "Завод A");
        var sensor = TestData.NewSensor(factory, "S1", "Temperature");
        TestData.AddReading(sensor, 1, "normal");
        TestData.AddReading(sensor, 2, "WARNING");

        var result = TestData.All(sensor).GetProblematicSensors(0.5);

        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(1, result[0].WarningCount);
        Assert.AreEqual(50.0, result[0].AbnormalPercent, 0.001);
    }

    [TestMethod]
    public void GetProblematicSensors_ShareOutOfRange_Throws()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new List<ReadingEntity>().GetProblematicSensors(-0.1));
        Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new List<ReadingEntity>().GetProblematicSensors(1.1));
    }
}
