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

    public class BuildingService : IBuildingService, IDisposable
    {
        private readonly PowerGridEntities dbContext;
        private readonly IContextInfoAccessorService context;

        public BuildingService()
        {
            this.dbContext = new PowerGridEntities();
            this.context = new ContextInfoAccessorService();
        }

        List<BuildingModel> IBuildingService.GetAllBuildings()
        {
            var building = this.dbContext.Building.Where(b => b.Campus.Role.Any(r => r.Id == this.context.Current.RoleId));
            return new BuildingModelMapping().Map(building).ToList();
        }

        BuildingModel IBuildingService.GetBuildingByID(int buildingID)
        {
            var building = this.dbContext.Building.FirstOrDefault(data => data.BuildingID == buildingID && data.Campus.Role.Any(r => r.Id == this.context.Current.RoleId));
            return new BuildingModelMapping().Map(building);
        }

        List<BuildingModel> IBuildingService.GetBuildingsByCampus(int campusId)
        {
            var buildings = this.dbContext.Building.Where(b => b.Campus.CampusID == campusId && b.Campus.Role.Any(r => r.Id == this.context.Current.RoleId));

            return new BuildingModelMapping().Map(buildings).ToList();
        }

        ResponseModel IBuildingService.AddBuilding(BuildingModel model)
        {
            var building = new Building();
            building.BuildingName = model.BuildingName;
            building.BuildingDesc = model.BuildingDesc;
            building.CampusID = model.CampusID;
            building.CreatedBy = this.context.Current.UserId;
            building.CreatedOn = DateTime.UtcNow;
            building.ModifiedBy = this.context.Current.UserId;
            building.ModifiedOn = DateTime.UtcNow;
            building.IsActive = true;
            building.IsDeleted = false;

            this.dbContext.Building.Add(building);
            this.dbContext.SaveChanges();
            return new ResponseModel { Message = "Building added successfully", Status_Code = (int)StatusCode.Ok };
        }

        ResponseModel IBuildingService.DeleteBuilding(int buildingId)
        {
            var data = this.dbContext.Building.FirstOrDefault(f => f.BuildingID == buildingId);
            if (data == null)
            {
                return new ResponseModel { Message = "Invalid Building", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                data.IsActive = false;
                data.IsDeleted = true;
                data.ModifiedBy = this.context.Current.UserId;
                data.ModifiedOn = DateTime.UtcNow;
                this.dbContext.SaveChanges();
                return new ResponseModel { Message = "Building deleted successfully", Status_Code = (int)StatusCode.Ok };
            }
        }

        ResponseModel IBuildingService.UpdateBuilding(BuildingModel model)
        {
            var data = this.dbContext.Building.FirstOrDefault(f => f.BuildingID == model.BuildingID && f.Campus.Role.Any(r => r.Id == this.context.Current.RoleId));

            if (data == null)
            {
                return new ResponseModel { Message = "Building does not exists or user does not have permission to access it.", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(model.BuildingName))
                {
                    data.BuildingName = model.BuildingName;
                }

                if (!string.IsNullOrWhiteSpace(model.BuildingDesc))
                {
                    data.BuildingDesc = model.BuildingDesc;
                }

                data.IsActive = model.IsActive;
                data.ModifiedBy = this.context.Current.UserId;
                data.ModifiedOn = DateTime.UtcNow;
            }

            this.dbContext.SaveChanges();
            return new ResponseModel { Message = "Building details Updated", Status_Code = (int)StatusCode.Ok };
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