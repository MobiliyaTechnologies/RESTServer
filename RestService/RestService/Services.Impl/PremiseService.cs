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

    public sealed class PremiseService : IPremiseService, IDisposable
    {
        private readonly PowerGridEntities dbContext;
        private readonly IContextInfoAccessorService context;
        private readonly IMeterService meterService;

        public PremiseService()
        {
            this.dbContext = new PowerGridEntities();
            this.context = new ContextInfoAccessorService();
            this.meterService = new MeterService();
        }

        List<PremiseModel> IPremiseService.GetAllPremise()
        {
            var premise = this.context.Current.RoleType == UserRole.Student ? this.dbContext.Premise.WhereActivePremise() : this.dbContext.Premise.WhereActiveAccessiblePremise();

            var premiseModels = new PremiseModelMapping().Map(premise).ToList();

            this.LinkConsumptionWithPremise(premiseModels);
            return premiseModels;
        }

        PremiseModel IPremiseService.GetPremiseByID(int premiseID)
        {
            var premise = this.dbContext.Premise.WhereActivePremise(c => c.PremiseID == premiseID);
            var premiseModels = new PremiseModelMapping().Map(premise).ToList();

            this.LinkConsumptionWithPremise(premiseModels);
            return premiseModels.FirstOrDefault();
        }

        PremiseModel IPremiseService.GetPremiseByLocation(decimal latitude, decimal longitude)
        {
            var premise = this.dbContext.Premise.WhereActiveAccessiblePremise(c => c.Latitude == latitude && c.Longitude == longitude);
            var premiseModels = new PremiseModelMapping().Map(premise).ToList();

            this.LinkConsumptionWithPremise(premiseModels);
            return premiseModels.FirstOrDefault();
        }

        ResponseModel IPremiseService.AddPremise(PremiseModel model)
        {
            var hasOrganization = this.dbContext.Organization.WhereActiveOrganization(u => u.OrganizationID == model.OrganizationID).Any();

            if (!hasOrganization)
            {
                return new ResponseModel { Status_Code = (int)StatusCode.Error, Message = string.Format("Organization does not exist for id - {0}", model.OrganizationID) };
            }

            var userRole = this.dbContext.Role.WhereActiveAccessibleRole().First();

            // create premise with role
            var premise = new Premise();
            premise.PremiseName = model.PremiseName;
            premise.PremiseDesc = model.PremiseDesc;
            premise.OrganizationID = model.OrganizationID;
            premise.Role.Add(userRole);
            premise.CreatedBy = this.context.Current.UserId;
            premise.CreatedOn = DateTime.UtcNow;
            premise.ModifiedBy = this.context.Current.UserId;
            premise.ModifiedOn = DateTime.UtcNow;
            premise.IsActive = true;
            premise.IsDeleted = false;

            this.dbContext.Premise.Add(premise);

            this.dbContext.SaveChanges();
            return new ResponseModel { Message = "Premise added successfully", Status_Code = (int)StatusCode.Ok };
        }

        ResponseModel IPremiseService.DeletePremise(int premiseID)
        {
            var data = this.dbContext.Premise.WhereActivePremise(f => f.PremiseID == premiseID).FirstOrDefault();

            if (data == null)
            {
                return new ResponseModel { Message = "Invalid Premise", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                data.IsActive = false;
                data.IsDeleted = true;
                data.ModifiedBy = this.context.Current.UserId;
                data.ModifiedOn = DateTime.UtcNow;
                this.dbContext.SaveChanges();
                return new ResponseModel { Message = "Premise deleted successfully", Status_Code = (int)StatusCode.Ok };
            }
        }

        ResponseModel IPremiseService.UpdatePremise(PremiseModel model)
        {
            var data = this.dbContext.Premise.WhereActiveAccessiblePremise(c => c.PremiseID == model.PremiseID).FirstOrDefault();

            if (data == null)
            {
                return new ResponseModel { Message = "Invalid Premise", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(model.PremiseName))
                {
                    data.PremiseName = model.PremiseName;
                }

                if (!string.IsNullOrWhiteSpace(model.PremiseDesc))
                {
                    data.PremiseDesc = model.PremiseDesc;
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
            return new ResponseModel { Message = "Premise details Updated", Status_Code = (int)StatusCode.Ok };
        }

        ResponseModel IPremiseService.AssignRolesToPremise(List<int> roleIds, int premiseID)
        {
            var premise = this.dbContext.Premise.WhereActivePremise(c => c.PremiseID == premiseID).FirstOrDefault();

            if (premise == null)
            {
                return new ResponseModel { Status_Code = (int)StatusCode.Error, Message = "Premise does not exist" };
            }

            foreach (var roleId in roleIds)
            {
                var role = this.dbContext.Role.WhereActiveRole(r => r.Id == roleId).FirstOrDefault();

                if (role == null)
                {
                    return new ResponseModel { Status_Code = (int)StatusCode.Error, Message = string.Format("Role does not exist for role id - {0}", roleId) };
                }

                premise.Role.Add(role);
            }

            this.dbContext.SaveChanges();

            return new ResponseModel { Status_Code = (int)StatusCode.Error, Message = "Role assigned to premise successfully" };
        }

        ResponseModel IPremiseService.AddBuildingsToPremise(int premiseID, List<int> buildingIds)
        {
            var premise = this.dbContext.Premise.WhereActivePremise(c => c.PremiseID == premiseID);

            if (premise == null)
            {
                return new ResponseModel(StatusCode.Error, "Premise does not exists.");
            }

            var buildings = this.dbContext.Building.WhereActiveBuilding(b => buildingIds.Any(id => id == b.BuildingID));

            if (buildings.Count() != buildingIds.Count() || buildingIds.Count() == 0)
            {
                return new ResponseModel(StatusCode.Error, "Buildings does not exists for given ids");
            }

            foreach (var building in buildings)
            {
                building.PremiseID = premiseID;
            }

            this.dbContext.SaveChanges();
            return new ResponseModel(StatusCode.Ok, "Buildings added to premise");
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

        private void LinkConsumptionWithPremise(List<PremiseModel> premiseModels)
        {
            foreach (var premiseModel in premiseModels)
            {
                var monthlyConsumptionPrediction = this.meterService.GetMonthlyConsumptionPredictionPerPremise(premiseModel.PremiseID);
                premiseModel.MonthlyConsumption = monthlyConsumptionPrediction.Consumption;
                premiseModel.MonthlyPrediction = monthlyConsumptionPrediction.Prediction;
            }
        }
    }
}