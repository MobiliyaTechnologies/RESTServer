﻿namespace RestService.Services.Impl
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
        private readonly string csvFileType = ".csv";

        public PiServerService()
        {
            this.context = new ContextInfoAccessorService();
            this.dbContext = new PowerGridEntities();
            this.blobStorageService = new BlobStorageService();
        }

        List<PiServerModel> IPiServerService.GetAllPiServers()
        {
            var piServer = this.dbContext.PiServer.WhereActiveAccessiblePiServer();
            return new PiServerModelMapping().Map(piServer).ToList();
        }

        PiServerModel IPiServerService.GetPiServerByID(int piServerID)
        {
            var piServer = this.dbContext.PiServer.WhereActiveAccessiblePiServer(data => data.PiServerID == piServerID);

            return new PiServerModelMapping().Map(piServer).FirstOrDefault();
        }

        PiServerModel IPiServerService.GetPiServerByName(string piServerName)
        {
            var piServer = this.dbContext.PiServer.WhereActiveAccessiblePiServer(data => data.PiServerName.Equals(piServerName, StringComparison.InvariantCultureIgnoreCase));

            return new PiServerModelMapping().Map(piServer).FirstOrDefault();
        }

        ResponseModel IPiServerService.AddPiServer(PiServerModel model)
        {
            var hasAuthorizeCampus = this.dbContext.Campus.WhereActiveAccessibleCampus(c => c.CampusID == model.CampusID).Any();

            if (!hasAuthorizeCampus)
            {
                return new ResponseModel(StatusCode.Error, "User does not have access to campus.");
            }

            var piServer = new PiServer();
            piServer.PiServerName = model.PiServerName;
            piServer.PiServerDesc = model.PiServerDesc;
            piServer.CampusID = model.CampusID;
            piServer.PiServerURL = model.PiServerURL;
            piServer.CreatedBy = this.context.Current.UserId;
            piServer.CreatedOn = DateTime.UtcNow;
            piServer.ModifiedBy = this.context.Current.UserId;
            piServer.ModifiedOn = DateTime.UtcNow;
            piServer.IsActive = true;
            piServer.IsDeleted = false;

            this.dbContext.PiServer.Add(piServer);
            this.dbContext.SaveChanges();

            var blobStorageModel = new BlobStorageModel
            {
                BlobName = ApiConfiguration.BlobPrefix + model.PiServerName + this.csvFileType,
                BlobType = model.CampusScheduleFileType,
                Blob = model.CampusScheduleFile,
                StorageContainer = ApiConfiguration.BlobContainer
            };

            this.blobStorageService.UploadBlob(blobStorageModel);
            return new ResponseModel { Message = "Pi Server added successfully", Status_Code = (int)StatusCode.Ok };
        }

        ResponseModel IPiServerService.DeletePiServer(int piServerId)
        {
            var data = this.dbContext.PiServer.WhereActiveAccessiblePiServer(f => f.PiServerID == piServerId).FirstOrDefault();
            if (data == null)
            {
                return new ResponseModel { Message = "Pi Server does not exists or user does not have a permission.", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                data.IsDeleted = true;
                data.ModifiedBy = this.context.Current.UserId;
                data.ModifiedOn = DateTime.UtcNow;
                this.dbContext.SaveChanges();

                var blobStorageModel = new BlobStorageModel
                {
                    BlobName = ApiConfiguration.BlobPrefix + data.PiServerName + this.csvFileType,
                    StorageContainer = ApiConfiguration.BlobContainer
                };
                this.blobStorageService.DeleteBlob(blobStorageModel);

                return new ResponseModel { Message = "Pi Server deleted successfully", Status_Code = (int)StatusCode.Ok };
            }
        }

        ResponseModel IPiServerService.UpdatePiServer(PiServerModel model)
        {
            var data = this.dbContext.PiServer.WhereActiveAccessiblePiServer(f => f.PiServerID == model.PiServerID).FirstOrDefault();
            var piServerName = string.Empty;

            if (data == null)
            {
                return new ResponseModel { Message = "Pi Server does not exists or user does not have a permission.", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(model.PiServerName) && !data.PiServerName.Equals(model.PiServerName))
                {
                    piServerName = data.PiServerName;
                    data.PiServerName = model.PiServerName;
                }

                if (!string.IsNullOrWhiteSpace(model.PiServerDesc))
                {
                    data.PiServerDesc = model.PiServerDesc;
                }

                if (!string.IsNullOrWhiteSpace(model.PiServerURL))
                {
                    data.PiServerURL = model.PiServerURL;
                }

                data.ModifiedBy = this.context.Current.UserId;
                data.ModifiedOn = DateTime.UtcNow;
            }

            this.dbContext.SaveChanges();

            if (piServerName != string.Empty && model.CampusScheduleFile != null)
            {
                var blobStorageModel = new BlobStorageModel
                {
                    BlobName = ApiConfiguration.BlobPrefix + piServerName + this.csvFileType,
                    BlobType = model.CampusScheduleFileType,
                    Blob = model.CampusScheduleFile,
                    StorageContainer = ApiConfiguration.BlobContainer
                };
                this.blobStorageService.DeleteBlob(blobStorageModel);

                blobStorageModel.BlobName = ApiConfiguration.BlobPrefix + data.PiServerName + this.csvFileType;
                this.blobStorageService.UploadBlob(blobStorageModel);
            }
           else if (piServerName != string.Empty && model.CampusScheduleFile == null)
            {
                var blobStorageModel = new BlobStorageModel
                {
                    BlobName = ApiConfiguration.BlobPrefix + piServerName + this.csvFileType,
                    StorageContainer = ApiConfiguration.BlobContainer
                };
                var newBlobName = ApiConfiguration.BlobPrefix + data.PiServerName + this.csvFileType;

                this.blobStorageService.RenameBlob(blobStorageModel, newBlobName);
            }
            else if (piServerName == string.Empty && model.CampusScheduleFile != null)
            {
                var blobStorageModel = new BlobStorageModel
                {
                    BlobName = ApiConfiguration.BlobPrefix + data.PiServerName + this.csvFileType,
                    BlobType = model.CampusScheduleFileType,
                    Blob = model.CampusScheduleFile,
                    StorageContainer = ApiConfiguration.BlobContainer
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