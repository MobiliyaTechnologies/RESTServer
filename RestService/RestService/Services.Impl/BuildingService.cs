namespace RestService.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using RestService.Entities;
    using RestService.Enums;
    using RestService.Filters;
    using RestService.Mappings;
    using RestService.Models;
    using RestService.Utilities;

    public class BuildingService : IBuildingService, IDisposable
    {
        private readonly PowerGridEntities dbContext;
        private readonly IContextInfoAccessorService context;
        private readonly IMeterService meterService;

        public BuildingService()
        {
            this.dbContext = new PowerGridEntities();
            this.context = new ContextInfoAccessorService();
            this.meterService = new MeterService();
        }

        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        List<BuildingModel> IBuildingService.GetAllBuildings()
        {
            var buildings = this.context.Current.RoleType == UserRole.Student ? this.dbContext.Building.WhereActiveBuilding() : this.dbContext.Building.WhereActiveAccessibleBuilding();

            var buildingModels = new BuildingModelMapping().Map(buildings).ToList();

            this.LinkConsumptionWithBuilding(buildingModels);
            return buildingModels;
        }
      
        BuildingModel IBuildingService.GetBuildingByID(int buildingID)
        {
            var buildings = this.dbContext.Building.WhereActiveAccessibleBuilding(data => data.BuildingID == buildingID);
            var buildingModels = new BuildingModelMapping().Map(buildings).ToList();

            this.LinkConsumptionWithBuilding(buildingModels);
            return buildingModels.FirstOrDefault();
        }

        List<BuildingModel> IBuildingService.GetBuildingsByPremise(int premiseID)
        {
            var buildings = this.dbContext.Building.WhereActiveAccessibleBuilding(b => b.Premise.PremiseID == premiseID);
            var buildingModels = new BuildingModelMapping().Map(buildings).ToList();

            this.LinkConsumptionWithBuilding(buildingModels);
            return buildingModels;
        }

        BuildingModel IBuildingService.GetBuildingByLocation(decimal latitude, decimal longitude)
        {
            var buildings = this.dbContext.Building.WhereActiveAccessibleBuilding(b => b.Latitude == latitude && b.Longitude == longitude);
            var buildingModels = new BuildingModelMapping().Map(buildings).ToList();

            this.LinkConsumptionWithBuilding(buildingModels);
            return buildingModels.FirstOrDefault();
        }

        ResponseModel IBuildingService.AddBuilding(BuildingModel model)
        {
            var building = new Building();
            building.BuildingName = model.BuildingName;
            building.BuildingDesc = model.BuildingDesc;
            building.PremiseID = model.PremiseID;
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
            var data = this.dbContext.Building.WhereActiveAccessibleBuilding(f => f.BuildingID == buildingId).FirstOrDefault();
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
            var data = this.dbContext.Building.WhereActiveAccessibleBuilding(f => f.BuildingID == model.BuildingID).FirstOrDefault();

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

        private void LinkConsumptionWithBuilding(List<BuildingModel> buildingModels)
        {
            foreach (var buildingModel in buildingModels)
            {
                buildingModel.MonthlyConsumption = this.meterService.GetMonthlyConsumptionPerBuildings(buildingModel.BuildingID);
            }
        }
    }
}