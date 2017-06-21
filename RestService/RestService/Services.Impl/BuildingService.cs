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

        List<BuildingModel> IBuildingService.GetAllBuildings()
        {
            var buildings = this.context.Current.RoleType == UserRole.Student || this.context.Current.RoleType == UserRole.SuperAdmin ? this.dbContext.Building.WhereActiveBuilding() : this.dbContext.Building.WhereActiveAccessibleBuilding();

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

                if (model.Latitude != default(decimal))
                {
                    data.Latitude = model.Latitude;
                }

                if (model.Longitude != default(decimal))
                {
                    data.Longitude = model.Longitude;
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
                var monthlyConsumptionPrediction = this.meterService.GetMonthlyConsumptionPredictionPerBuildings(buildingModel.BuildingID);
                buildingModel.MonthlyConsumption = monthlyConsumptionPrediction.Consumption;
                buildingModel.MonthlyPrediction = monthlyConsumptionPrediction.Prediction;
            }
        }
    }
}