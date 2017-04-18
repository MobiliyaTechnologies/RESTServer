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

        public BuildingService()
        {
            this.dbContext = new PowerGridEntities();
        }

        List<BuildingModel> IBuildingService.GetAllBuildings()
        {
            var building = this.dbContext.Building;
            return new BuildingModelMapping().Map(building).ToList();
        }

        BuildingModel IBuildingService.GetBuildingByID(BuildingModel model)
        {
            var building = (from data in this.dbContext.Building
                          where data.BuildingID == model.BuildingID
                          select new BuildingModel
                          {
                              BuildingID = data.BuildingID,
                              BuildingName = data.BuildingName,
                              BuildingDesc = data.BuildingDesc,
                              CampusID = data.CampusID,
                              CreatedBy = data.CreatedBy ?? default(int),
                              CreatedOn = data.CreatedOn ?? default(DateTime),
                              ModifiedBy = data.ModifiedBy ?? default(int),
                              ModifiedOn = data.ModifiedOn ?? default(DateTime),
                              IsActive = data.IsActive,
                              IsDeleted = data.IsDeleted
                          }).FirstOrDefault();
            return building;
        }

        ResponseModel IBuildingService.AddBuilding(BuildingModel model, int userId)
        {
            var building = new Building();
            building.BuildingName = model.BuildingName;
            building.BuildingDesc = model.BuildingDesc;
            building.CampusID = model.CampusID;
            building.CreatedBy = userId;
            building.CreatedOn = DateTime.UtcNow;
            building.ModifiedBy = userId;
            building.ModifiedOn = DateTime.UtcNow;
            building.IsActive = true;
            building.IsDeleted = false;

            this.dbContext.Building.Add(building);
            this.dbContext.SaveChanges();
            return new ResponseModel { Message = "Building added successfully", Status_Code = (int)StatusCode.Ok };
        }

        ResponseModel IBuildingService.DeleteBuilding(BuildingModel model, int userId)
        {
            var data = this.dbContext.Building.FirstOrDefault(f => f.BuildingID == model.BuildingID);
            if (data == null)
            {
                return new ResponseModel { Message = "Invalid Building", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                data.IsActive = false;
                data.IsDeleted = true;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;
                this.dbContext.SaveChanges();
                return new ResponseModel { Message = "Building deleted successfully", Status_Code = (int)StatusCode.Ok };
            }
        }

        ResponseModel IBuildingService.UpdateBuilding(BuildingModel model, int userId)
        {
            var data = this.dbContext.Building.FirstOrDefault(f => f.BuildingID == model.BuildingID);

            if (data == null)
            {
                return new ResponseModel { Message = "Invalid Building", Status_Code = (int)StatusCode.Error };
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

                data.CampusID = model.CampusID;
                data.IsActive = model.IsActive;
                data.IsDeleted = model.IsDeleted;
                data.ModifiedBy = userId;
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