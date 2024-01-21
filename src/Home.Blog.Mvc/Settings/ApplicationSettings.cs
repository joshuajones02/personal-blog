namespace Home.Blog.Mvc.Settings;

public class ApplicationSettings
{
    public ApplicationSettings()
    {
        BlobStorageConnectionString = EnvironmentExtensions.GetRequiredEnvironmentVariable<string>("connectionString:blobstorage");
        DatabaseConnectionString = EnvironmentExtensions.GetRequiredEnvironmentVariable<string>("connectionString:database");
    }
        
    public string BlobStorageConnectionString { get; init; }

    public string DatabaseConnectionString { get; init; }

    private static ApplicationSettings? _instance;
    public static ApplicationSettings Instance =>
        _instance = _instance ?? new ApplicationSettings();
}
