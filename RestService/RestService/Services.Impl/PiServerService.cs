namespace RestService.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Enums;
    using RestService.Mappings;
    using RestService.Models;
    using RestService.Utilities;

    public sealed class PiServerService : IPiServerService, IDisposable
    {
        private readonly PowerGridEntities dbContext;
        private readonly IContextInfoAccessorService context;
        private readonly IBlobStorageService blobStorageService;
        private readonly IApplicationConfigurationService applicationConfigurationService;

        public PiServerService()
        {
            this.context = new ContextInfoAccessorService();
            this.dbContext = new PowerGridEntities();
            this.blobStorageService = new BlobStorageService();
            this.applicationConfigurationService = new ApplicationConfigurationService();
        }

        List<PiServerModel> IPiServerService.GetAllPiServers()
        {
            var piServer = this.dbContext.PiServer.WhereActivePiServer();
            return new PiServerModelMapping().Map(piServer).ToList();
        }

        PiServerModel IPiServerService.GetPiServerByID(int piServerID)
        {
            var piServer = this.dbContext.PiServer.WhereActivePiServer(data => data.PiServerID == piServerID);

            return new PiServerModelMapping().Map(piServer).FirstOrDefault();
        }

        PiServerModel IPiServerService.GetPiServerByName(string piServerName)
        {
            var piServer = this.dbContext.PiServer.WhereActivePiServer(data => data.PiServerName.Equals(piServerName, StringComparison.InvariantCultureIgnoreCase));

            return new PiServerModelMapping().Map(piServer).FirstOrDefault();
        }

        ResponseModel IPiServerService.UpdatePiServer(PiServerModel model)
        {
            var data = this.dbContext.PiServer.WhereActivePiServer(f => f.PiServerID == model.PiServerID).FirstOrDefault();

            if (data == null)
            {
                return new ResponseModel { Message = "Pi Server does not exists or user does not have a permission.", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(model.PiServerDesc))
                {
                    data.PiServerDesc = model.PiServerDesc;

                    data.ModifiedBy = this.context.Current.UserId;
                    data.ModifiedOn = DateTime.UtcNow;

                    this.dbContext.SaveChanges();
                }
            }

            if (model.RoomScheduleFile != null && !string.IsNullOrWhiteSpace(model.RoomScheduleFileType))
            {
                var blobStorageModel = new BlobStorageModel
                {
                    BlobName = ApiConfiguration.BlobPrefix + data.PiServerName + ApiConstant.CsvFileType,
                    BlobType = model.RoomScheduleFileType,
                    Blob = model.RoomScheduleFile,
                    StorageContainer = ApiConfiguration.BlobPrivateContainer
                };
                this.blobStorageService.UploadBlob(blobStorageModel);
            }

            return new ResponseModel { Message = "Pi Server details Updated", Status_Code = (int)StatusCode.Ok };
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            if (this.dbContext != null)
            {
                this.dbContext.Dispose();
            }
        }
    }
}