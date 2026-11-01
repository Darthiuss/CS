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

        public void SortByItemsCount()
        {
            var sorted = Items.OrderBy(b => b, new BaggageItemsCountComparer()).ToList();
            UpdateVisibleCollection(sorted);
        }

        public void FilterByWeight(double weight)
        {
            _currentFilter = weight;
            var filtered = _allItems
                .Where(b => b.TotalWeight > weight)
                .OrderBy(b => b, new BaggageItemsCountComparer())
                .ToList();
            UpdateVisibleCollection(filtered);
        }

        public void ResetFilter()
        {
            _currentFilter = null;
            UpdateVisibleCollection(_allItems);
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
