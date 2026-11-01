namespace Lab6Baggage
{
    public interface IFileSaver
    {
        Task SaveAsync<T>(string filePath, List<T> data);
    }
}
