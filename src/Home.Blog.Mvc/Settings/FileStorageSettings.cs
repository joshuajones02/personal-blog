namespace Home.Blog.Mvc.Settings;

public interface IFileStorageSettings
{
    string ConnectionString { get; }
}

internal class FileStorageSettings : BaseSettings, IFileStorageSettings
{
    private readonly string? _connectionString;

    public FileStorageSettings()
    {
        _connectionString = Get<string>("connectionString:fileStorage");
        Bucket = GetRequired<string>("fileStorage:bucket");

        if (string.IsNullOrEmpty(_connectionString))
        {
            AccessKey = GetRequired<string>("fileStorage:accessKey");
            Endpoint = GetRequired<string>("fileStorage:endpoint");
            SecretKey = GetRequired<string>("fileStorage:secretKey");
            IsSecure = Get<bool>("fileStorage:isSecure", @default: false);
            _connectionString = $"endpoint={Endpoint};accessKey={AccessKey};secretKey={SecretKey};secure={IsSecure.ToString().ToLower()}";
        }
    }

    public bool IsSecure { get; init; }
    public string? AccessKey { get; init; }
    public string Bucket { get; init; }
    public string? Endpoint { get; init; }
    public string? SecretKey { get; init; }

    public string ConnectionString => _connectionString!;


    private static FileStorageSettings? _instance;
    public static FileStorageSettings Instance =>
        _instance = _instance ?? new FileStorageSettings();
}
