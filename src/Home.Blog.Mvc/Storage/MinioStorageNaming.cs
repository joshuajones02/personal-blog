namespace Home.Blog.Mvc.Storage;

/// <summary>
/// The different naming strategies available for MinIO storage.
/// </summary>
public enum MinioStorageNaming
{
    /// <summary>
    /// Files are named with the media id and filename.
    /// </summary>
    UniqueFileNames,

    /// <summary>
    /// Files are stored in a folder named with the media id.
    /// </summary>
    UniqueFolderNames
}