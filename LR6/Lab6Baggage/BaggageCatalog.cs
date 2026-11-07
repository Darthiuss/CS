using System.Collections.ObjectModel;

namespace Lab6Baggage
{
    public class BaggageCatalog
    {
        private readonly IFileSaver _fileSaver;
        private readonly IFileLoader _fileLoader;
        private readonly string _filePath;

        public ObservableCollection<Baggage> Items { get; }

        private List<Baggage> _allItems;

        private double? _currentFilter;

        public BaggageCatalog(IFileSaver fileSaver, IFileLoader fileLoader, string filePath)
        {
            _fileSaver = fileSaver;
            _fileLoader = fileLoader;
            _filePath = filePath;
            Items = new ObservableCollection<Baggage>();
            _allItems = new List<Baggage>();
            _currentFilter = null;
        }

        public void AddBaggage(Baggage item)
        {
            ArgumentNullException.ThrowIfNull(item);

            _allItems.Add(item);

            if (_currentFilter == null || item.TotalWeight > _currentFilter)
            {
                Items.Add(item);
            }
        }

        public void DeleteBaggage(Baggage item)
        {
            Items.Remove(item);
            _allItems.Remove(item);
        }

        // Сортування та фільтрація

        // Сортування за кількістю речей
        public void SortByItemsCount()
        {
            var sorted = Items
                .OrderBy(b => b.ItemsCount)
                .ThenBy(b => b.FullName)
                .ToList();
            UpdateVisibleCollection(sorted);
        }

        // Пасажири, загальна вага багажу яких перевищує задану, відсортовані за кількістю речей
        public void FilterByWeight(double weight)
        {
            _currentFilter = weight;
            var filtered = _allItems
                .Where(b => b.TotalWeight > weight)
                .OrderBy(b => b.ItemsCount)
                .ThenBy(b => b.FullName)
                .ToList();
            UpdateVisibleCollection(filtered);
        }

        public void ResetFilter()
        {
            _currentFilter = null;
            UpdateVisibleCollection(_allItems);
        }

        // LINQ-запити

        // Пошук за частиною ПІБ пасажира
        public void SearchByName(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                ResetFilter();
                return;
            }

            string term = searchTerm.Trim();
            _currentFilter = null;
            var searchResult = _allItems
                .Where(b => b.FullName.Contains(term, StringComparison.OrdinalIgnoreCase))
                .ToList();
            UpdateVisibleCollection(searchResult);
        }

        // Список унікальних прізвищ пасажирів
        public List<string> GetUniqueSurnames()
        {
            return _allItems
                .Select(b => b.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0])
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(s => s, StringComparer.Ordinal)
                .ToList();
        }

        // Три найважчі багажі
        public void FindThreeHeaviest()
        {
            _currentFilter = null;
            var heaviest = _allItems
                .OrderByDescending(b => b.TotalWeight)
                .Take(3)
                .ToList();
            UpdateVisibleCollection(heaviest);
        }

        // Середня вага однієї речі
        public double CalculateAverageWeightPerItem()
        {
            int totalItems = _allItems.Sum(b => b.ItemsCount);
            if (totalItems == 0)
            {
                return 0;
            }
            return _allItems.Sum(b => b.TotalWeight) / totalItems;
        }

        // Групування за кількістю речей
        public List<IGrouping<int, Baggage>> GroupByItemsCount()
        {
            _currentFilter = null;
            var groups = (from b in _allItems
                          group b by b.ItemsCount into g
                          orderby g.Key descending
                          select g).ToList();

            UpdateVisibleCollection(groups.SelectMany(g => g).ToList());
            return groups;
        }

        // Статистика відображуваного списку
        public double GetMaxWeight()
        {
            return Items.Select(b => b.TotalWeight).DefaultIfEmpty(0.0).Max();
        }

        public double GetAverageWeight()
        {
            return Items.Select(b => b.TotalWeight).DefaultIfEmpty(0.0).Average();
        }

        private void UpdateVisibleCollection(List<Baggage> newList)
        {
            Items.Clear();
            foreach (var item in newList)
            {
                Items.Add(item);
            }
        }

        public async Task SaveAsync()
        {
            await _fileSaver.SaveAsync(_filePath, _allItems);
        }

        public async Task<bool> LoadAsync()
        {
            var loaded = await _fileLoader.LoadAsync<Baggage>(_filePath);
            if (loaded == null)
            {
                return false;
            }

            _allItems = loaded;
            _currentFilter = null;
            UpdateVisibleCollection(_allItems);
            return true;
        }
    }
}
