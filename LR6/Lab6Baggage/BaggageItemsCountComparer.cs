namespace Lab6Baggage
{
    public class BaggageItemsCountComparer : IComparer<Baggage>
    {
        public int Compare(Baggage? x, Baggage? y)
        {
            // Перевіряємо на null, щоб уникнути винятків
            if (x == null || y == null)
            {
                return 0;
            }

            // Порівнюємо кількість речей
            return x.ItemsCount.CompareTo(y.ItemsCount);
        }
    }
}
