using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SensorMonitor.Core.Data;
using SensorMonitor.Core.Dto;
using SensorMonitor.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SensorMonitor.Tests;

[TestClass]
public class ImportAndDatabaseTests
{
    private const string Json = """
    [
      { "factoryId": "FAC-01", "factoryName": "Завод 1", "sensors": [
          { "sensorId": "SN-1", "type": "Temperature", "readings": [
              { "timestamp": "2026-09-01T10:00:00Z", "value": 75.4, "status": "Normal" },
              { "timestamp": "2026-09-01T10:05:00Z", "value": 92.1, "status": "Warning" } ] } ] },
      { "factoryId": "FAC-02", "factoryName": "Завод 2", "sensors": [
          { "sensorId": "SN-2", "type": "Pressure", "readings": [
              { "timestamp": "2026-09-01T10:00:00Z", "value": 5.1, "status": "Normal" } ] } ] }
    ]
    """;

    private static async Task<string> WriteTempJsonAsync()
    {
        string path = Path.Combine(Path.GetTempPath(), $"sensors_{Guid.NewGuid():N}.json");
        await File.WriteAllTextAsync(path, Json);
        return path;
    }

    private static AppDbContext CreateContext(SqliteConnection connection)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(connection).Options;
        return new AppDbContext(options);
    }

    // ---------- Асинхронне читання JSON ----------

    [TestMethod]
    public async Task JsonFileReader_StreamsAllFactories()
    {
        string path = await WriteTempJsonAsync();
        try
        {
            var reader = new JsonFileReader<FactoryDto>();
            var factories = new List<FactoryDto>();

            await foreach (var factory in reader.LoadAsync(path))
                factories.Add(factory);

            Assert.AreEqual(2, factories.Count);
            Assert.AreEqual("FAC-01", factories[0].FactoryId);
            Assert.AreEqual(2, factories[0].Sensors[0].Readings.Count);
            Assert.AreEqual("Warning", factories[0].Sensors[0].Readings[1].Status);
            Assert.AreEqual(92.1, factories[0].Sensors[0].Readings[1].Value, 0.001);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public async Task JsonFileReader_CancelledToken_ThrowsOperationCanceled()
    {
        string path = await WriteTempJsonAsync();
        try
        {
            var reader = new JsonFileReader<FactoryDto>();
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            bool cancelled = false;
            try
            {
                await foreach (var _ in reader.LoadAsync(path, cts.Token))
                {
                }
            }
            catch (OperationCanceledException)
            {
                cancelled = true;
            }

            Assert.IsTrue(cancelled);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public async Task JsonFileReader_MissingFile_ThrowsFileNotFound()
    {
        var reader = new JsonFileReader<FactoryDto>();
        bool thrown = false;

        try
        {
            await foreach (var _ in reader.LoadAsync(Path.Combine(Path.GetTempPath(), "no_such_file.json")))
            {
            }
        }
        catch (FileNotFoundException)
        {
            thrown = true;
        }

        Assert.IsTrue(thrown);
    }

    // ---------- Імпорт у БД ----------

    [TestMethod]
    public async Task Import_SavesAllEntities_AndReImportDoesNotDuplicate()
    {
        string path = await WriteTempJsonAsync();
        using var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        try
        {
            await using var context = CreateContext(connection);
            var catalog = new SensorCatalog(new JsonFileReader<FactoryDto>(), new SqliteSensorRepository(context));

            var first = await catalog.ImportAsync(path);
            var second = await catalog.ImportAsync(path); // повторний імпорт замінює дані

            Assert.AreEqual(2, first.Factories);
            Assert.AreEqual(2, first.Sensors);
            Assert.AreEqual(3, first.Readings);
            Assert.AreEqual(first, second);

            Assert.AreEqual(2, await context.Factories.CountAsync());
            Assert.AreEqual(2, await context.Sensors.CountAsync());
            Assert.AreEqual(3, await context.Readings.CountAsync());

            var readings = await catalog.GetReadingsAsync();
            Assert.AreEqual(3, readings.Count);
            Assert.IsTrue(readings.All(r => r.Sensor is not null && r.Sensor.Factory is not null));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public async Task DeletingFactory_CascadesToSensorsAndReadings()
    {
        string path = await WriteTempJsonAsync();
        using var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        try
        {
            await using var context = CreateContext(connection);
            var catalog = new SensorCatalog(new JsonFileReader<FactoryDto>(), new SqliteSensorRepository(context));
            await catalog.ImportAsync(path);

            await context.Factories.Where(f => f.FactoryId == "FAC-01").ExecuteDeleteAsync();

            Assert.AreEqual(1, await context.Factories.CountAsync());
            Assert.AreEqual(1, await context.Sensors.CountAsync());
            Assert.AreEqual(1, await context.Readings.CountAsync());
        }
        finally
        {
            File.Delete(path);
        }
    }
}
