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

        public PiServerService()
        {
            this.dbContext = new PowerGridEntities();
        }

        List<PiServerModel> IPiServerService.GetAllPiServers()
        {
            var piServer = this.dbContext.PiServer;
            return new PiServerModelMapping().Map(piServer).ToList();
        }

        PiServerModel IPiServerService.GetPiServerByID(PiServerModel model)
        {
            var piServer = (from data in this.dbContext.PiServer
                          where data.PiServerID == model.PiServerID
                          select new PiServerModel
                          {
                              PiServerID = data.PiServerID,
                              PiServerName = data.PiServerName,
                              PiServerDesc = data.PiServerDesc,
                              CampusID = data.CampusID,
                              PiServerURL = data.PiServerURL,
                              CreatedBy = data.CreatedBy ?? default(int),
                              CreatedOn = data.CreatedOn ?? default(DateTime),
                              ModifiedBy = data.ModifiedBy ?? default(int),
                              ModifiedOn = data.ModifiedOn ?? default(DateTime),
                              IsActive = data.IsActive,
                              IsDeleted = data.IsDeleted
                          }).FirstOrDefault();
            return piServer;
        }

        ResponseModel IPiServerService.AddPiServer(PiServerModel model, int userId)
        {
            var piServer = new PiServer();
            piServer.PiServerName = model.PiServerName;
            piServer.PiServerDesc = model.PiServerDesc;
            piServer.CampusID = model.CampusID;
            piServer.PiServerURL = model.PiServerURL;
            piServer.CreatedBy = userId;
            piServer.CreatedOn = DateTime.UtcNow;
            piServer.ModifiedBy = userId;
            piServer.ModifiedOn = DateTime.UtcNow;
            piServer.IsActive = true;
            piServer.IsDeleted = false;

            this.dbContext.PiServer.Add(piServer);
            this.dbContext.SaveChanges();
            return new ResponseModel { Message = "Pi Server added successfully", Status_Code = (int)StatusCode.Ok };
        }

        ResponseModel IPiServerService.DeletePiServer(PiServerModel model, int userId)
        {
            var data = this.dbContext.PiServer.FirstOrDefault(f => f.PiServerID == model.PiServerID);
            if (data == null)
            {
                return new ResponseModel { Message = "Invalid Pi Server", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                data.IsActive = false;
                data.IsDeleted = true;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;
                this.dbContext.SaveChanges();
                return new ResponseModel { Message = "Pi Server deleted successfully", Status_Code = (int)StatusCode.Ok };
            }
        }

        ResponseModel IPiServerService.UpdatePiServer(PiServerModel model, int userId)
        {
            var data = this.dbContext.PiServer.FirstOrDefault(f => f.PiServerID == model.PiServerID);

            if (data == null)
            {
                return new ResponseModel { Message = "Invalid Pi Server", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(model.PiServerName))
                {
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

                data.CampusID = model.CampusID;
                data.IsActive = model.IsActive;
                data.IsDeleted = model.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;
            }

            this.dbContext.SaveChanges();
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