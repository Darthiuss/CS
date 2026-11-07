using System.Globalization;

namespace Lab6Baggage
{
    public class Baggage : IComparable<Baggage>
    {
        private string _fullName = string.Empty;
        private int _itemsCount;
        private double _totalWeight;

        public string FullName
        {
            get { return _fullName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("ПІБ пасажира не може бути пустим.", nameof(FullName));
                }
                _fullName = value.Trim();
            }
        }

        public int ItemsCount
        {
            get { return _itemsCount; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Кількість речей має бути більшою за нуль.", nameof(ItemsCount));
                }
                _itemsCount = value;
            }
        }

        public double TotalWeight
        {
            get { return _totalWeight; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException("Загальна вага має бути коректним числом.", nameof(TotalWeight));
                }
                if (value <= 0)
                {
                    throw new ArgumentException("Загальна вага має бути більшою за нуль.", nameof(TotalWeight));
                }
                _totalWeight = value;
            }
        }

        public Baggage(string fullName, int itemsCount, double totalWeight)
        {
            // Конструктор викликає сеттери, що містять валідацію
            FullName = fullName;
            ItemsCount = itemsCount;
            TotalWeight = totalWeight;
        }

        // Реалізація інтерфейсу IComparable (природний порядок - за ПІБ пасажира)
        public int CompareTo(Baggage? other)
        {
            if (other == null)
            {
                return 1;
            }
            return string.Compare(this.FullName, other.FullName, StringComparison.OrdinalIgnoreCase);
        }

        public override string ToString()
        {
            string weight = TotalWeight.ToString("0.##", CultureInfo.InvariantCulture);
            return $"ПІБ: {FullName}, Кількість речей: {ItemsCount}, Загальна вага: {weight} кг";
        }
    }
}
