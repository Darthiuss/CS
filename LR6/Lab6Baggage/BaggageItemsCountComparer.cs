namespace Lab6Baggage
{
    public class BaggageItemsCountComparer : IComparer<Baggage>
    {
        public int Compare(Baggage? x, Baggage? y)
        {
            if (x == null || y == null)
            {
                return 0;
            }

            return x.ItemsCount.CompareTo(y.ItemsCount);
        }
    }
}
