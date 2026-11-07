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

        // Тести LINQ-запитів

        // 1. Пошук за частиною ПІБ
        [TestMethod]
        public void SearchByName_WithNullTerm_ShowsAllItems()
        {
            _catalog.SearchByName(null);
            Assert.AreEqual(5, _catalog.Items.Count);
        }

        [TestMethod]
        public void SearchByName_WithWhiteSpaceTerm_ShowsAllItems()
        {
            _catalog.SearchByName("     ");
            Assert.AreEqual(5, _catalog.Items.Count);
        }

        [TestMethod]
        public void SearchByName_WithFullName_ReturnsSingleItem()
        {
            _catalog.SearchByName("Петренко Петро Петрович");
            Assert.AreEqual(1, _catalog.Items.Count);
            Assert.AreSame(_petrenko, _catalog.Items[0]);
        }

        [TestMethod]
        public void SearchByName_WithFragment_ReturnsAllMatches()
        {
            _catalog.SearchByName("ренко");
            Assert.AreEqual(2, _catalog.Items.Count);
            Assert.AreSame(_petrenko, _catalog.Items[0]);
            Assert.AreSame(_sydorenko, _catalog.Items[1]);
        }

        [TestMethod]
        public void SearchByName_IsCaseInsensitive()
        {
            _catalog.SearchByName("ПЕТРЕНКО");
            Assert.AreEqual(1, _catalog.Items.Count);
            Assert.AreSame(_petrenko, _catalog.Items[0]);
        }

        [TestMethod]
        public void SearchByName_WhenNothingMatches_ReturnsEmptyCollection()
        {
            _catalog.SearchByName("Невідомий");
            Assert.AreEqual(0, _catalog.Items.Count);
        }

        // 2. Унікальні прізвища
        [TestMethod]
        public void GetUniqueSurnames_ReturnsDistinctSortedSurnames()
        {
            _catalog.AddBaggage(new Baggage("Іваненко Олена Петрівна", 1, 8));

            var surnames = _catalog.GetUniqueSurnames();

            string[] expected = { "Іваненко", "Коваль", "Мельник", "Петренко", "Сидоренко" };
            CollectionAssert.AreEqual(expected, surnames);
        }

        [TestMethod]
        public void GetUniqueSurnames_WhenCatalogIsEmpty_ReturnsEmptyList()
        {
            var empty = new BaggageCatalog(_storage, _storage, "test.json");
            Assert.AreEqual(0, empty.GetUniqueSurnames().Count);
        }

        // 3. Три найважчі багажі
        [TestMethod]
        public void FindThreeHeaviest_ReturnsThreeHeaviestInDescendingOrder()
        {
            _catalog.FindThreeHeaviest();

            Assert.AreEqual(3, _catalog.Items.Count);
            Assert.AreSame(_koval, _catalog.Items[0]);    // 40 кг
            Assert.AreSame(_petrenko, _catalog.Items[1]); // 35 кг
            Assert.AreSame(_ivanenko, _catalog.Items[2]); // 20.5 кг
        }

        [TestMethod]
        public void FindThreeHeaviest_WhenFewerThanThreeItems_ReturnsAllItems()
        {
            var small = new BaggageCatalog(_storage, _storage, "test.json");
            small.AddBaggage(_ivanenko);
            small.AddBaggage(_petrenko);

            small.FindThreeHeaviest();

            Assert.AreEqual(2, small.Items.Count);
            Assert.AreSame(_petrenko, small.Items[0]);
        }

        // 4. Середня вага однієї речі: (20.5 + 35 + 12 + 40 + 20) / (3 + 1 + 5 + 2 + 4) = 127.5 / 15
        [TestMethod]
        public void CalculateAverageWeightPerItem_ReturnsCorrectValue()
        {
            Assert.AreEqual(8.5, _catalog.CalculateAverageWeightPerItem(), 0.0001);
        }

        [TestMethod]
        public void CalculateAverageWeightPerItem_WhenCatalogIsEmpty_ReturnsZero()
        {
            var empty = new BaggageCatalog(_storage, _storage, "test.json");
            Assert.AreEqual(0, empty.CalculateAverageWeightPerItem());
        }

        // 5. Групування за кількістю речей
        [TestMethod]
        public void GroupByItemsCount_ReturnsGroupsInDescendingOrder()
        {
            var bondarenko = new Baggage("Бондаренко Богдан Борисович", 3, 7);
            _catalog.AddBaggage(bondarenko);

            var groups = _catalog.GroupByItemsCount();

            int[] expectedKeys = { 5, 4, 3, 2, 1 };
            CollectionAssert.AreEqual(expectedKeys, groups.Select(g => g.Key).ToArray());
            Assert.AreEqual(2, groups[2].Count(), "У групі з 3 речами має бути 2 пасажири.");

            Assert.AreEqual(6, _catalog.Items.Count);
            Assert.AreSame(_sydorenko, _catalog.Items[0]);
            Assert.AreSame(_ivanenko, _catalog.Items[2]);
            Assert.AreSame(bondarenko, _catalog.Items[3]);
            Assert.AreSame(_petrenko, _catalog.Items[5]);
        }

        [TestMethod]
        public void GroupByItemsCount_WhenCatalogIsEmpty_ReturnsNoGroups()
        {
            var empty = new BaggageCatalog(_storage, _storage, "test.json");
            Assert.AreEqual(0, empty.GroupByItemsCount().Count);
        }

        // Статистика відображуваного списку
        [TestMethod]
        public void GetMaxWeight_ReturnsMaximumOfVisibleItems()
        {
            Assert.AreEqual(40, _catalog.GetMaxWeight());
            _catalog.FilterByWeight(20);
            _catalog.DeleteBaggage(_koval);
            Assert.AreEqual(35, _catalog.GetMaxWeight());
        }

        [TestMethod]
        public void GetAverageWeight_ReturnsAverageOfVisibleItems()
        {
            // (20.5 + 35 + 12 + 40 + 20) / 5 = 25.5
            Assert.AreEqual(25.5, _catalog.GetAverageWeight(), 0.0001);
        }

        [TestMethod]
        public void GetMaxAndAverageWeight_WhenCatalogIsEmpty_ReturnZero()
        {
            var empty = new BaggageCatalog(_storage, _storage, "test.json");
            Assert.AreEqual(0, empty.GetMaxWeight());
            Assert.AreEqual(0, empty.GetAverageWeight());
        }
    }
}
