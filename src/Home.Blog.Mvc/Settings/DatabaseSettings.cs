namespace Home.Blog.Mvc.Settings;

public interface IDatabaseSettings
{
    string ConnectionString { get; }
}

public class DatabaseSettings : BaseSettings, IDatabaseSettings
{
    public DatabaseSettings()
    {
        ConnectionString = GetRequired<string>("connectionString:database");
    }

    public string ConnectionString { get; init; }

    private static DatabaseSettings? _instance;
    public static DatabaseSettings Instance =>
        _instance = _instance ?? new DatabaseSettings();
}