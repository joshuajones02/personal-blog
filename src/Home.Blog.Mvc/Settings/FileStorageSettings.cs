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
            Endpoint = GetRequired<string>("fileStorage:endpointInternal");
            SecretKey = GetRequired<string>("fileStorage:secretKey");
            IsSecure = Get<bool>("fileStorage:isSecure", @default: false);
            PublicEndpoint = Get<string>("fileStorage:endpointPublic");
            _connectionString = $"endpoint={Endpoint};accessKey={AccessKey};secretKey={SecretKey};secure={IsSecure.ToString().ToLower()}";

            if (!string.IsNullOrEmpty(PublicEndpoint))
            {
                _connectionString += $";publicEndpoint={PublicEndpoint}";
            }
        }
    }

    public bool IsSecure { get; init; }
    public string? AccessKey { get; init; }
    public string Bucket { get; init; }
    public string? Endpoint { get; init; }
    public string? PublicEndpoint { get; init; }
    public string? SecretKey { get; init; }

    public string ConnectionString => _connectionString!;


    private static FileStorageSettings? _instance;
    public static FileStorageSettings Instance =>
        _instance = _instance ?? new FileStorageSettings();
}
