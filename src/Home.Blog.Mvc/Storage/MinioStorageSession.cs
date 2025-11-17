namespace Home.Blog.Mvc.Storage;

using System;
using System.IO;
using System.Threading.Tasks;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Piranha;
using Piranha.Models;

/// <summary>
/// MinIO implementation of IStorageSession for Piranha CMS
/// </summary>
public class MinioStorageSession : IStorageSession
{
    private readonly IMinioClient _client;
    private readonly MinioStorageNaming _naming;
    private readonly string _bucketName;

    /// <summary>
    /// Creates a new MinIO storage session.
    /// </summary>
    /// <param name="client">The MinIO client</param>
    /// <param name="bucketName">The bucket name</param>
    /// <param name="naming">How uploaded media files should be named</param>
    public MinioStorageSession(
        IMinioClient client,
        string bucketName,
        MinioStorageNaming naming)
    {
        _client = client;
        _bucketName = bucketName;
        _naming = naming;
    }

    /// <summary>
    /// Writes the content for the specified media content to storage.
    /// </summary>
    /// <param name="media">The media file</param>
    /// <param name="filename">The file name</param>
    /// <param name="contentType">The content type</param>
    /// <param name="bytes">The binary data</param>
    /// <returns>The public URL</returns>
    public async Task<string> PutAsync(Piranha.Models.Media media, string filename, string contentType, byte[] bytes)
    {
        try
        {
            var objectName = GetResourceName(media, filename);

            await EnsureBucketExistsAsync();

            using (var stream = new MemoryStream(bytes))
            {
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(objectName)
                    .WithStreamData(stream)
                    .WithObjectSize(stream.Length)
                    .WithContentType(contentType);

                await _client.PutObjectAsync(putObjectArgs);
            }

            return objectName;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to upload file to MinIO: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Gets the content for the specified media.
    /// </summary>
    /// <param name="media">The media file</param>
    /// <param name="filename">The file name</param>
    /// <param name="stream">The output stream</param>
    /// <returns>True if the content was found</returns>
    public async Task<bool> GetAsync(Piranha.Models.Media media, string filename, Stream stream)
    {
        var objectName = GetResourceName(media, filename);

        try
        {
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithCallbackStream((sourceStream) =>
                {
                    sourceStream.CopyTo(stream);
                });

            await _client.GetObjectAsync(getObjectArgs);

            return true;
        }
        catch (ObjectNotFoundException)
        {
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to retrieve file from MinIO: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Deletes the content for the specified media.
    /// </summary>
    /// <param name="media">The media file</param>
    /// <param name="filename">The file name</param>
    public async Task<bool> DeleteAsync(Piranha.Models.Media media, string filename)
    {
        var objectName = GetResourceName(media, filename);

        try
        {
            var removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName);

            await _client.RemoveObjectAsync(removeObjectArgs);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to delete file from MinIO: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Disposes the session.
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Ensures the bucket exists, creating it if necessary.
    /// </summary>
    private async Task EnsureBucketExistsAsync()
    {
        try
        {
            var bucketExistsArgs = new BucketExistsArgs()
                .WithBucket(_bucketName);

            bool found = await _client.BucketExistsAsync(bucketExistsArgs);

            if (!found)
            {
                var makeBucketArgs = new MakeBucketArgs()
                    .WithBucket(_bucketName);

                await _client.MakeBucketAsync(makeBucketArgs);

                // Set the bucket policy to allow public read access
                var policy = @"{
                        ""Version"": ""2012-10-17"",
                        ""Statement"": [
                            {
                                ""Effect"": ""Allow"",
                                ""Principal"": {
                                    ""AWS"": [""*""]
                                },
                                ""Action"": [""s3:GetObject""],
                                ""Resource"": [""arn:aws:s3:::" + _bucketName + @"/*""]
                            }
                        ]
                    }";

                var setPolicyArgs = new SetPolicyArgs()
                    .WithBucket(_bucketName)
                    .WithPolicy(policy);

                await _client.SetPolicyAsync(setPolicyArgs);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to ensure MinIO bucket exists: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Gets the resource name for the given media object.
    /// </summary>
    /// <param name="media">The media object</param>
    /// <param name="filename">The file name</param>
    /// <returns>The resource name</returns>
    private string GetResourceName(Piranha.Models.Media media, string filename) =>
        _naming == MinioStorageNaming.UniqueFileNames
            ? $"{media.Id}-{filename}"
            : $"{media.Id}/{filename}";

    public async Task<string> PutAsync(Media media, string filename, string contentType, Stream stream)
    {
        try
        {
            var objectName = GetResourceName(media, filename);

            await EnsureBucketExistsAsync();

            var putObjectArgs = new PutObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType(contentType);

            await _client.PutObjectAsync(putObjectArgs);

            return objectName;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to upload file to MinIO: {ex.Message}", ex);
        }
    }
}
