namespace RestService.Services.Impl
{
    using System;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using RestService.Models;
    using RestService.Utilities;

    public class BlobStorageService : IBlobStorageService
    {
        private readonly CloudBlobClient cloudBlobClient;

        public BlobStorageService()
        {
            var storageAccount = CloudStorageAccount.Parse(ApiConfiguration.BlobStorageConnectionString);
            this.cloudBlobClient = storageAccount.CreateCloudBlobClient();
        }

        void IBlobStorageService.DeleteBlob(BlobStorageModel blobStorageModel)
        {
            var container = this.cloudBlobClient.GetContainerReference(blobStorageModel.StorageContainer);

            if (container.Exists())
            {
                var blockBlob = container.GetBlockBlobReference(blobStorageModel.BlobName);
                blockBlob.DeleteIfExists();
            }
        }

        void IBlobStorageService.RenameBlob(BlobStorageModel oldBlob, string newBlobName)
        {
            var container = this.cloudBlobClient.GetContainerReference(oldBlob.StorageContainer);

            var blockBlob = container.GetBlockBlobReference(oldBlob.BlobName);

            if (blockBlob.Exists())
            {
                var newBlob = container.GetBlockBlobReference(newBlobName);

                newBlob.Properties.ContentType = blockBlob.Properties.ContentType;
                newBlob.StartCopy(blockBlob);

                blockBlob.Delete();
            }
            else
            {
                throw new InvalidOperationException("Blob does not exist.");
            }
        }

        void IBlobStorageService.UploadBlob(BlobStorageModel blobStorageModel)
        {
            var container = this.cloudBlobClient.GetContainerReference(blobStorageModel.StorageContainer);

            if (container.CreateIfNotExists())
            {
                if (blobStorageModel.IsPublicContainer)
                {
                    container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }
            }

            var blockBlob = container.GetBlockBlobReference(blobStorageModel.BlobName);

            blockBlob.Properties.ContentType = blobStorageModel.BlobType;
            blockBlob.UploadFromStream(blobStorageModel.Blob);
        }

        string IBlobStorageService.GetBlobUri(BlobStorageModel blobStorageModel)
        {
            var container = this.cloudBlobClient.GetContainerReference(blobStorageModel.StorageContainer);
            var blockBlob = container.GetBlockBlobReference(blobStorageModel.BlobName);

            return blockBlob.Uri.AbsoluteUri;
        }
    }
}