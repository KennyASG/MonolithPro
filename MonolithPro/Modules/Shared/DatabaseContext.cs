namespace MonolithPro.Shared;

public class DatabaseContext
{
    private readonly LoggerService _logger;

    public DatabaseContext(LoggerService logger)
    {
        _logger = logger;
        _logger.Log("DatabaseContext initialized");
    }

    public void SaveChanges()
    {
        _logger.Log("Changes saved to database (simulated)");
    }

    public T FindById<T>(int id, List<T> collection) where T : class
    {
        _logger.Log($"Finding entity of type {typeof(T).Name} with ID: {id}");
        return collection.ElementAtOrDefault(id - 1);
    }
}