using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lab6Baggage.Tests
{
    [TestClass]
    public class BaggageCatalogTests
    {
        private class InMemoryStorage : IFileSaver, IFileLoader
        {
            public object Stored;

            public Task SaveAsync<T>(string filePath, List<T> data)
            {
                Stored = new List<T>(data);
                return Task.CompletedTask;
            }

            public Task<List<T>> LoadAsync<T>(string filePath)
            {
                return Task.FromResult(Stored as List<T>);
            }
        }

        private InMemoryStorage _storage;
        private BaggageCatalog _catalog;
        private Baggage _ivanenko;   // 3 речі, 20.5 кг
        private Baggage _petrenko;   // 1 річ, 35 кг
        private Baggage _sydorenko;  // 5 речей, 12 кг
        private Baggage _koval;      // 2 речі, 40 кг
        private Baggage _melnyk;     // 4 речі, рівно 20 кг

        [TestInitialize]
        public void TestInitialize()
        {
            _storage = new InMemoryStorage();
            _catalog = new BaggageCatalog(_storage, _storage, "test.json");

            _ivanenko = new Baggage("Іваненко Іван Іванович", 3, 20.5);
            _petrenko = new Baggage("Петренко Петро Петрович", 1, 35);
            _sydorenko = new Baggage("Сидоренко Сергій Семенович", 5, 12);
            _koval = new Baggage("Коваль Корній Кирилович", 2, 40);
            _melnyk = new Baggage("Мельник Марія Михайлівна", 4, 20);

            _catalog.AddBaggage(_ivanenko);
            _catalog.AddBaggage(_petrenko);
            _catalog.AddBaggage(_sydorenko);
            _catalog.AddBaggage(_koval);
            _catalog.AddBaggage(_melnyk);
        }

        [TestMethod]
        public void AddBaggage_AddsItemToCollection()
        {
            var item = new Baggage("Бондаренко Богдан Борисович", 1, 7);
            _catalog.AddBaggage(item);
            Assert.AreEqual(6, _catalog.Items.Count);
            Assert.IsTrue(_catalog.Items.Contains(item));
        }

        [TestMethod]
        public void AddBaggage_WithNull_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _catalog.AddBaggage(null));
        }

        [TestMethod]
        public void DeleteBaggage_RemovesItemFromCollection()
        {
            _catalog.DeleteBaggage(_sydorenko);
            Assert.AreEqual(4, _catalog.Items.Count);
            Assert.IsFalse(_catalog.Items.Contains(_sydorenko));
        }

        [TestMethod]
        public void FilterByWeight_ReturnsHeavierSortedByItemsCount()
        {
            _catalog.FilterByWeight(20);

            Assert.AreEqual(3, _catalog.Items.Count);
            Assert.AreSame(_petrenko, _catalog.Items[0]); // 1 річ
            Assert.AreSame(_koval, _catalog.Items[1]);    // 2 речі
            Assert.AreSame(_ivanenko, _catalog.Items[2]); // 3 речі
        }

        [TestMethod]
        public void FilterByWeight_ExcludesItemsWithEqualWeight()
        {
            _catalog.FilterByWeight(20);
            Assert.IsFalse(_catalog.Items.Contains(_melnyk), "Вага рівно 20 кг не перевищує задану.");
        }

        [TestMethod]
        public void FilterByWeight_WhenNothingMatches_ReturnsEmptyCollection()
        {
            _catalog.FilterByWeight(100);
            Assert.AreEqual(0, _catalog.Items.Count);
        }

        [TestMethod]
        public void ResetFilter_RestoresAllItems()
        {
            _catalog.FilterByWeight(30);
            _catalog.ResetFilter();
            Assert.AreEqual(5, _catalog.Items.Count);
        }

        [TestMethod]
        public void SortByItemsCount_SortsAscending()
        {
            _catalog.SortByItemsCount();

            int[] expected = { 1, 2, 3, 4, 5 };
            int[] actual = _catalog.Items.Select(b => b.ItemsCount).ToArray();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddBaggage_WhenFilterIsActive_ShowsOnlyMatchingItems()
        {
            _catalog.FilterByWeight(20);

            _catalog.AddBaggage(new Baggage("Легкий Лев Левович", 1, 5));
            Assert.AreEqual(3, _catalog.Items.Count, "Легкий багаж не відповідає фільтру.");

            _catalog.AddBaggage(new Baggage("Важкий Віктор Валерійович", 6, 50));
            Assert.AreEqual(4, _catalog.Items.Count, "Важкий багаж відповідає фільтру.");
        }

        [TestMethod]
        public void DeleteBaggage_WhenFilterIsActive_RemovesFromAllItems()
        {
            _catalog.FilterByWeight(20);
            _catalog.DeleteBaggage(_petrenko);
            _catalog.ResetFilter();

            Assert.AreEqual(4, _catalog.Items.Count);
            Assert.IsFalse(_catalog.Items.Contains(_petrenko));
        }

        [TestMethod]
        public async Task SaveAsync_SavesAllItems_EvenWhenFilterIsActive()
        {
            _catalog.FilterByWeight(20);

            await _catalog.SaveAsync();

            var saved = _storage.Stored as List<Baggage>;
            Assert.IsNotNull(saved);
            Assert.AreEqual(5, saved.Count);
        }

        [TestMethod]
        public async Task LoadAsync_ReplacesCatalogContent()
        {
            await _catalog.SaveAsync();
            var other = new BaggageCatalog(_storage, _storage, "test.json");

            bool loaded = await other.LoadAsync();

            Assert.IsTrue(loaded);
            Assert.AreEqual(5, other.Items.Count);
        }

        [TestMethod]
        public async Task LoadAsync_WhenNothingStored_ReturnsFalseAndKeepsData()
        {
            var emptyStorage = new InMemoryStorage();
            var catalog = new BaggageCatalog(emptyStorage, emptyStorage, "test.json");
            catalog.AddBaggage(_ivanenko);

            bool loaded = await catalog.LoadAsync();

            Assert.IsFalse(loaded);
            Assert.AreEqual(1, catalog.Items.Count);
        }
    }
}
