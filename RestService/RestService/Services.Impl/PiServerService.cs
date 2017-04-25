namespace RestService.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Enums;
    using RestService.Mappings;
    using RestService.Models;

    public sealed class PiServerService : IPiServerService, IDisposable
    {
        private readonly PowerGridEntities dbContext;
        private readonly IContextInfoAccessorService context;

        public PiServerService()
        {
            this.context = new ContextInfoAccessorService();
            this.dbContext = new PowerGridEntities();
        }

        List<PiServerModel> IPiServerService.GetAllPiServers()
        {
            var piServer = this.dbContext.PiServer.Where(p => p.Campus.Role.Any(r => r.Id == this.context.Current.RoleId));
            return new PiServerModelMapping().Map(piServer).ToList();
        }

        PiServerModel IPiServerService.GetPiServerByID(int piServerID)
        {
            var piServer = this.dbContext.PiServer.FirstOrDefault(data => data.PiServerID == piServerID && data.Campus.Role.Any(r => r.Id == this.context.Current.RoleId));

            return new PiServerModelMapping().Map(piServer);
        }

        PiServerModel IPiServerService.GetPiServerByName(string piServerName)
        {
            var piServer = this.dbContext.PiServer.FirstOrDefault(data => data.PiServerName.Equals(piServerName, StringComparison.InvariantCultureIgnoreCase) && data.Campus.Role.Any(r => r.Id == this.context.Current.RoleId));

            return new PiServerModelMapping().Map(piServer);
        }

        ResponseModel IPiServerService.AddPiServer(PiServerModel model)
        {
            var hasAuthorizeCampus = this.dbContext.Campus.Any(c => c.CampusID == model.CampusID && c.Role.Any(r => r.Id == this.context.Current.RoleId));

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
            return new ResponseModel { Message = "Pi Server added successfully", Status_Code = (int)StatusCode.Ok };
        }

        ResponseModel IPiServerService.DeletePiServer(int piServerId)
        {
            var data = this.dbContext.PiServer.FirstOrDefault(f => f.PiServerID == piServerId && f.Campus.Role.Any(r => r.Id == this.context.Current.RoleId));
            if (data == null)
            {
                return new ResponseModel { Message = "Pi Server does not exists or user does not have a permission.", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                data.IsActive = false;
                data.IsDeleted = true;
                data.ModifiedBy = this.context.Current.UserId;
                data.ModifiedOn = DateTime.UtcNow;
                this.dbContext.SaveChanges();
                return new ResponseModel { Message = "Pi Server deleted successfully", Status_Code = (int)StatusCode.Ok };
            }
        }

        ResponseModel IPiServerService.UpdatePiServer(PiServerModel model)
        {
            var data = this.dbContext.PiServer.FirstOrDefault(f => f.PiServerID == model.PiServerID && f.Campus.Role.Any(r => r.Id == this.context.Current.RoleId));

            if (data == null)
            {
                return new ResponseModel { Message = "Pi Server does not exists or user does not have a permission.", Status_Code = (int)StatusCode.Error };
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

                data.IsActive = model.IsActive;
                data.ModifiedBy = this.context.Current.UserId;
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