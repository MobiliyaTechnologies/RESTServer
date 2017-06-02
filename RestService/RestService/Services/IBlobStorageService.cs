namespace RestService.Services
{
    using RestService.Models;

    /// <summary>
    /// Provides blob storage operations.
    /// </summary>
    public interface IBlobStorageService
    {
        /// <summary>
        /// Uploads the blob to blob storage, override blob if exist already.
        /// Create public container if not exists already.
        /// </summary>
        /// <param name="blobStorageModel">The BLOB storage model.</param>
        void UploadBlob(BlobStorageModel blobStorageModel);

        /// <summary>
        /// Rename existing blob.
        /// </summary>
        /// <param name="oldBlob">The old BLOB.</param>
        /// <param name="newBlobName">New name of the BLOB.</param>
        void RenameBlob(BlobStorageModel oldBlob, string newBlobName);

        /// <summary>
        /// Deletes the BLOB if exists.
        /// </summary>
        /// <param name="blobStorageModel">The BLOB storage model.</param>
        void DeleteBlob(BlobStorageModel blobStorageModel);

        /// <summary>
        /// Gets the BLOB uri.
        /// </summary>
        /// <param name="blobStorageModel">The BLOB storage model.</param>
        /// <returns>
        /// The blob uri.
        /// </returns>
        string GetBlobUri(BlobStorageModel blobStorageModel);
    }
}
