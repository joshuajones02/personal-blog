namespace Home.Blog.Mvc.Storage;

using System.Threading.Tasks;
using Minio;
using Piranha;

/// <summary>
/// MinIO implementation of IStorage for Piranha CMS
/// </summary>
public class MinioStorage : IStorage
{
    private readonly string _bucketName;
    private readonly IMinioClient _minioClient;
    private readonly MinioStorageNaming _naming;
    private readonly string _endpoint;
    private readonly bool _secure;

    /// <summary>
    /// Creates a new MinIO storage instance.
    /// </summary>
    /// <param name="minioClient">The MinIO client</param>
    /// <param name="bucketName">The bucket name</param>
    /// <param name="naming">How uploaded media files should be named</param>
    /// <param name="endpoint">The MinIO endpoint</param>
    /// <param name="secure">Whether SSL is enabled</param>
    public MinioStorage(
        IMinioClient minioClient,
        string bucketName = "uploads",
        MinioStorageNaming naming = MinioStorageNaming.UniqueFileNames,
        string endpoint = "localhost:9000",
        bool secure = false)
    {
        _minioClient = minioClient;
        _bucketName = bucketName;
        _naming = naming;
        _endpoint = endpoint;
        _secure = secure;
    }

    /// <summary>
    /// Opens a new storage session.
    /// </summary>
    /// <returns>A new open session</returns>
    public Task<IStorageSession> OpenAsync()
    {
        return Task.FromResult<IStorageSession>(
            new MinioStorageSession(_minioClient, _bucketName, _naming));
    }

    /// <summary>
    /// Gets the public URL for the given media object.
    /// </summary>
    /// <param name="media">The media object</param>
    /// <param name="filename">The file name</param>
    /// <returns>The public URL</returns>
    public string GetPublicUrl(Piranha.Models.Media media, string filename)
    {
        if (!string.IsNullOrWhiteSpace(filename))
        {
            var protocol = _secure ? "https" : "http";
            var objectName = GetResourceName(media, filename);
            return $"{protocol}://{_endpoint}/{_bucketName}/{objectName}";
        }

        return null;
    }

    /// <summary>
    /// Gets the resource name for the given media object.
    /// </summary>
    /// <param name="media">The media object</param>
    /// <param name="filename">The file name</param>
    /// <returns>The resource name</returns>
    public string GetResourceName(Piranha.Models.Media media, string filename) =>
        _naming == MinioStorageNaming.UniqueFileNames
            ? $"{media.Id}-{filename}"
            : $"{media.Id}/{filename}";
}