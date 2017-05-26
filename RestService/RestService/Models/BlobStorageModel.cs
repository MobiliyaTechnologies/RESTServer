namespace RestService.Models
{
    using System.IO;

    public class BlobStorageModel
    {
        public string BlobName { get; set; }

        public string BlobType { get; set; }

        public Stream Blob { get; set; }

        public string StorageContainer { get; set; }
    }
}