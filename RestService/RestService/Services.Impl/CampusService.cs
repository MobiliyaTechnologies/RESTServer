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

        public CampusService()
        {
            this.dbContext = new PowerGridEntities();
        }

        List<CampusModel> ICampusService.GetAllCampus()
        {
            var campus = this.dbContext.Campus;
            return new CampusModelMapping().Map(campus).ToList();
        }

        CampusModel ICampusService.GetCampusByID(CampusModel model)
        {
            var Campus = (from data in this.dbContext.Campus
                              where data.CampusID == model.CampusID
                              select new CampusModel
                              {
                                  CampusID = data.CampusID,
                                  CampusName = data.CampusName,
                                  CampusDesc = data.CampusDesc,
                                  UniversityID = data.UniversityID,
                                  CreatedBy = data.CreatedBy ?? default(int),
                                  CreatedOn = data.CreatedOn ?? default(DateTime),
                                  ModifiedBy = data.ModifiedBy ?? default(int),
                                  ModifiedOn = data.ModifiedOn ?? default(DateTime),
                                  IsActive = data.IsActive,
                                  IsDeleted = data.IsDeleted
                              }).FirstOrDefault();
            return Campus;
        }

        ResponseModel ICampusService.AddCampus(CampusModel model, int userId)
        {
            var campus = new Campus();
            campus.CampusName = model.CampusName;
            campus.CampusDesc = model.CampusDesc;
            campus.UniversityID = model.UniversityID;
            campus.CreatedBy = userId;
            campus.CreatedOn = DateTime.UtcNow;
            campus.ModifiedBy = userId;
            campus.ModifiedOn = DateTime.UtcNow;
            campus.IsActive = true;
            campus.IsDeleted = false;

            this.dbContext.Campus.Add(campus);
            this.dbContext.SaveChanges();
            return new ResponseModel { Message = "Campus added successfully", Status_Code = (int)StatusCode.Ok };
        }

        ResponseModel ICampusService.DeleteCampus(CampusModel model, int userId)
        {
            var data = this.dbContext.Campus.FirstOrDefault(f => f.CampusID == model.CampusID);
            if (data == null)
            {
                return new ResponseModel { Message = "Invalid Campus", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                data.IsActive = false;
                data.IsDeleted = true;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;
                this.dbContext.SaveChanges();
                return new ResponseModel { Message = "Campus deleted successfully", Status_Code = (int)StatusCode.Ok };
            }
        }

        ResponseModel ICampusService.UpdateCampus(CampusModel model, int userId)
        {
            var data = this.dbContext.Campus.FirstOrDefault(f => f.CampusID == model.CampusID);

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

                data.UniversityID = model.UniversityID;
                data.IsActive = model.IsActive;
                data.IsDeleted = model.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;
            }

            this.dbContext.SaveChanges();
            return new ResponseModel { Message = "Campus details Updated", Status_Code = (int)StatusCode.Ok };
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