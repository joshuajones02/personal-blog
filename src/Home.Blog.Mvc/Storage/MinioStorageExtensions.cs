namespace Home.Blog.Mvc.Storage;

using System;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Piranha;

/// <summary>
/// Extension methods for registering MinIO storage with Piranha CMS
/// </summary>
public static class MinioStorageExtensions
{
    /// <summary>
    /// Adds the MinIO storage service to Piranha CMS.
    /// </summary>
    /// <param name="serviceBuilder">The service builder</param>
    /// <param name="endpoint">The MinIO endpoint (e.g., "localhost:9000")</param>
    /// <param name="accessKey">The access key</param>
    /// <param name="secretKey">The secret key</param>
    /// <param name="bucketName">The bucket name (default: "uploads")</param>
    /// <param name="naming">The naming strategy (default: UniqueFileNames)</param>
    /// <param name="secure">Whether to use HTTPS (default: false)</param>
    /// <param name="scope">The service lifetime (default: Singleton)</param>
    /// <returns>The updated service builder</returns>
    public static PiranhaServiceBuilder UseMinioStorage(
        this PiranhaServiceBuilder serviceBuilder,
        string endpoint,
        string accessKey,
        string secretKey,
        string bucketName = "uploads",
        MinioStorageNaming naming = MinioStorageNaming.UniqueFileNames,
        bool secure = false,
        ServiceLifetime scope = ServiceLifetime.Singleton)
    {
        // Register MinIO client as singleton
        serviceBuilder.Services.AddSingleton<IMinioClient>(sp =>
        {
            var client = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey);

            if (secure)
            {
                client = client.WithSSL();
            }

            return client.Build();
        });

        // Register the storage service
        serviceBuilder.Services.Add(new ServiceDescriptor(
            typeof(IStorage),
            sp => new MinioStorage(
                sp.GetRequiredService<IMinioClient>(),
                bucketName,
                naming,
                endpoint,
                secure),
            scope));

        return serviceBuilder;
    }

    /// <summary>
    /// Adds the MinIO storage service to Piranha CMS using a connection string.
    /// Connection string format: "endpoint=localhost:9000;accessKey=minioadmin;secretKey=minioadmin;secure=false"
    /// </summary>
    /// <param name="serviceBuilder">The service builder</param>
    /// <param name="connectionString">The MinIO connection string</param>
    /// <param name="bucketName">The bucket name (default: "uploads")</param>
    /// <param name="naming">The naming strategy (default: UniqueFileNames)</param>
    /// <param name="scope">The service lifetime (default: Singleton)</param>
    /// <returns>The updated service builder</returns>
    public static PiranhaServiceBuilder UseMinioStorage(
        this PiranhaServiceBuilder serviceBuilder,
        string connectionString,
        string bucketName = "uploads",
        MinioStorageNaming naming = MinioStorageNaming.UniqueFileNames,
        ServiceLifetime scope = ServiceLifetime.Singleton)
    {
        var connectionParams = ParseConnectionString(connectionString);

        return UseMinioStorage(
            serviceBuilder,
            connectionParams.endpoint,
            connectionParams.accessKey,
            connectionParams.secretKey,
            bucketName,
            naming,
            connectionParams.secure,
            scope);
    }

    /// <summary>
    /// Parses a MinIO connection string.
    /// </summary>
    /// <param name="connectionString">The connection string</param>
    /// <returns>Parsed connection parameters</returns>
    private static (string endpoint, string accessKey, string secretKey, bool secure) ParseConnectionString(string connectionString)
    {
        string? endpoint = null;
        string? accessKey = null;
        string? secretKey = null;
        bool secure = false;

        var parts = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries);
        foreach (var part in parts)
        {
            var keyValue = part.Split('=', 2);
            if (keyValue.Length == 2)
            {
                var key = keyValue[0].Trim().ToLowerInvariant();
                var value = keyValue[1].Trim();

                switch (key)
                {
                    case "endpoint":
                        endpoint = value;
                        break;
                    case "accesskey":
                        accessKey = value;
                        break;
                    case "secretkey":
                        secretKey = value;
                        break;
                    case "secure":
                        secure = bool.TryParse(value, out bool secureValue) && secureValue;
                        break;
                }
            }
        }

        if (string.IsNullOrWhiteSpace(endpoint))
            throw new ArgumentException("MinIO connection string must contain 'endpoint'");
        if (string.IsNullOrWhiteSpace(accessKey))
            throw new ArgumentException("MinIO connection string must contain 'accessKey'");
        if (string.IsNullOrWhiteSpace(secretKey))
            throw new ArgumentException("MinIO connection string must contain 'secretKey'");

        return (endpoint, accessKey, secretKey, secure);
    }
}