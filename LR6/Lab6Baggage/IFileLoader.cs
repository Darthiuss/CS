namespace Lab6Baggage
{
    public interface IFileLoader
    {
        Task<List<T>?> LoadAsync<T>(string filePath);
    }
}
