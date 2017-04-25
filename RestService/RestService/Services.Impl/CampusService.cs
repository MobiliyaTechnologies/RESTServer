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

    public sealed class CampusService : ICampusService, IDisposable
    {
        private readonly PowerGridEntities dbContext;
        private readonly IContextInfoAccessorService context;

        public CampusService()
        {
            this.dbContext = new PowerGridEntities();
            this.context = new ContextInfoAccessorService();
        }

        List<CampusModel> ICampusService.GetAllCampus()
        {
            var campus = this.dbContext.Campus;
            return new CampusModelMapping().Map(campus).ToList();
        }

        CampusModel ICampusService.GetCampusByID(int campusId)
        {
            var campus = this.dbContext.Campus.FirstOrDefault(c => c.CampusID == campusId);

            return new CampusModelMapping().Map(campus);
        }

        List<CampusModel> ICampusService.GetCampus()
        {
            var roleCampus = this.dbContext.Campus.Where(c => c.Role.Any(r => r.Id == this.context.Current.RoleId));

            return new CampusModelMapping().Map(roleCampus).ToList();
        }

        CampusModel ICampusService.GetCampusByLocation(decimal latitude, decimal longitude)
        {
            var currentUserRole = this.context.Current.RoleId;

            var campus = this.dbContext.Campus.Where(c => c.Role.Any(r => r.Id == currentUserRole) && c.Latitude == latitude && c.Longitude == longitude);

            return new CampusModelMapping().Map(campus).FirstOrDefault();
        }

        ResponseModel ICampusService.AddCampus(CampusModel model)
        {
            var hasUniversity = this.dbContext.University.Any(u => u.UniversityID == model.UniversityID);

            if (!hasUniversity)
            {
                return new ResponseModel { Status_Code = (int)StatusCode.Error, Message = string.Format("University does not exist for id - {0}", model.UniversityID) };
            }

            var userRole = this.dbContext.Role.First(u => u.Id == this.context.Current.RoleId);

            // create campus with role
            var campus = new Campus();
            campus.CampusName = model.CampusName;
            campus.CampusDesc = model.CampusDesc;
            campus.UniversityID = model.UniversityID;
            campus.Role.Add(userRole);
            campus.CreatedBy = this.context.Current.UserId;
            campus.CreatedOn = DateTime.UtcNow;
            campus.ModifiedBy = this.context.Current.UserId;
            campus.ModifiedOn = DateTime.UtcNow;
            campus.IsActive = true;
            campus.IsDeleted = false;

            this.dbContext.Campus.Add(campus);

            this.dbContext.SaveChanges();
            return new ResponseModel { Message = "Campus added successfully", Status_Code = (int)StatusCode.Ok };
        }

        ResponseModel ICampusService.DeleteCampus(int campusId)
        {
            var data = this.dbContext.Campus.FirstOrDefault(f => f.CampusID == campusId);

            if (data == null)
            {
                return new ResponseModel { Message = "Invalid Campus", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                data.IsActive = false;
                data.IsDeleted = true;
                data.ModifiedBy = this.context.Current.UserId;
                data.ModifiedOn = DateTime.UtcNow;
                this.dbContext.SaveChanges();
                return new ResponseModel { Message = "Campus deleted successfully", Status_Code = (int)StatusCode.Ok };
            }
        }

        ResponseModel ICampusService.UpdateCampus(CampusModel model)
        {
            var data = this.dbContext.Campus.FirstOrDefault(c => c.CampusID == model.CampusID && c.Role.Any(r => r.Id == this.context.Current.RoleId));

            if (data == null)
            {
                return new ResponseModel { Message = "Invalid Campus", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(model.CampusName))
                {
                    data.CampusName = model.CampusName;
                }

                if (!string.IsNullOrWhiteSpace(model.CampusDesc))
                {
                    data.CampusDesc = model.CampusDesc;
                }

                data.IsActive = model.IsActive;
                data.ModifiedBy = this.context.Current.UserId;
                data.ModifiedOn = DateTime.UtcNow;
            }

            this.dbContext.SaveChanges();
            return new ResponseModel { Message = "Campus details Updated", Status_Code = (int)StatusCode.Ok };
        }

        ResponseModel ICampusService.AssignRoleToCampus(int roleId, int campusId)
        {
            var campus = this.dbContext.Campus.FirstOrDefault(c => c.CampusID == campusId);

            if (campus == null)
            {
                return new ResponseModel { Status_Code = (int)StatusCode.Error, Message = "Campus does not exist" };
            }

            var role = this.dbContext.Role.FirstOrDefault(r => r.Id == roleId);

            if (role == null)
            {
                return new ResponseModel { Status_Code = (int)StatusCode.Error, Message = string.Format("Role does not exist for role id - {0}", roleId) };
            }

            campus.Role.Add(role);
            this.dbContext.SaveChanges();

            return new ResponseModel { Status_Code = (int)StatusCode.Error, Message = "Role assigned to campus successfully" };
        }

        ResponseModel ICampusService.AddBuildingsToCampus(int campusId, List<int> buildingIds)
        {
            var campus = this.dbContext.Campus.FirstOrDefault(c => c.CampusID == campusId);

            if (campus == null)
            {
                return new ResponseModel(StatusCode.Error, "Campus does not exists.");
            }

            var buildings = this.dbContext.Building.Where(b => buildingIds.Any(id => id == b.BuildingID));

            if (buildings.Count() != buildingIds.Count() || buildingIds.Count() == 0)
            {
                return new ResponseModel(StatusCode.Error, "Buildings does not exists for given ids");
            }

            foreach (var building in buildings)
            {
                building.CampusID = campusId;
            }

            this.dbContext.SaveChanges();
            return new ResponseModel(StatusCode.Ok, "Buildings added to campus");
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