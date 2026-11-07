using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lab6Baggage.Tests
{
    [TestClass]
    public class FileHandlerTests
    {
        private string _testFilePath;

        // Метод, який виконується перед кожним тестом
        [TestInitialize]
        public void TestInitialize()
        {
            _testFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".json");
        }

        // Метод, який виконується після кожного тесту
        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }

        [TestMethod]
        public async Task SaveAsync_Should_WriteDataToFile()
        {
            var items = new List<Baggage>
            {
                new Baggage("Іваненко Іван Іванович", 3, 25.5),
                new Baggage("Петренко Петро Петрович", 1, 8)
            };
            var fileSaver = new JsonFileWriter();

            await fileSaver.SaveAsync(_testFilePath, items);

            Assert.IsTrue(File.Exists(_testFilePath), "Файл не було створено.");
            string fileContent = await File.ReadAllTextAsync(_testFilePath);
            Assert.IsFalse(string.IsNullOrWhiteSpace(fileContent), "Файл порожній.");

            var fileReader = new JsonFileReader();
            var loaded = await fileReader.LoadAsync<Baggage>(_testFilePath);
            Assert.IsNotNull(loaded, "Десеріалізація не вдалася.");
            Assert.AreEqual(2, loaded.Count, "Кількість десеріалізованих об'єктів не збігається.");
        }

        [TestMethod]
        public async Task LoadAsync_Should_ReturnSameData_AsWasSaved()
        {
            var items = new List<Baggage>
            {
                new Baggage("Іваненко Іван Іванович", 3, 25.5),
                new Baggage("Петренко Петро Петрович", 1, 8)
            };
            await new JsonFileWriter().SaveAsync(_testFilePath, items);

            var loaded = await new JsonFileReader().LoadAsync<Baggage>(_testFilePath);

            Assert.IsNotNull(loaded);
            Assert.AreEqual(items[0].FullName, loaded[0].FullName);
            Assert.AreEqual(items[0].ItemsCount, loaded[0].ItemsCount);
            Assert.AreEqual(items[0].TotalWeight, loaded[0].TotalWeight);
            Assert.AreEqual(items[1].FullName, loaded[1].FullName);
            Assert.AreEqual(items[1].ItemsCount, loaded[1].ItemsCount);
            Assert.AreEqual(items[1].TotalWeight, loaded[1].TotalWeight);
        }

        [TestMethod]
        public async Task LoadAsync_WhenFileDoesNotExist_ReturnsNull()
        {
            var loaded = await new JsonFileReader().LoadAsync<Baggage>(_testFilePath);
            Assert.IsNull(loaded, "Для неіснуючого файлу має повертатися null.");
        }

        [TestMethod]
        public async Task SaveAsync_WhenDirectoryDoesNotExist_CreatesDirectory()
        {
            string directory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            string filePath = Path.Combine(directory, "baggage.json");
            try
            {
                var items = new List<Baggage> { new Baggage("Іваненко І.І.", 1, 5) };
                await new JsonFileWriter().SaveAsync(filePath, items);
                Assert.IsTrue(File.Exists(filePath), "Файл у новій директорії не створено.");
            }
            finally
            {
                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory, true);
                }
            }
        }
    }
}
