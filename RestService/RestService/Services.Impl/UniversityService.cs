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

    public sealed class UniversityService : IUniversityService, IDisposable
    {
        private readonly PowerGridEntities dbContext;

        public UniversityService()
        {
            this.dbContext = new PowerGridEntities();
        }

        List<UniversityModel> IUniversityService.GetAllUniversities()
        {
            var universities = this.dbContext.University;
            return new UniversityModelMapping().Map(universities).ToList();
        }

        UniversityModel IUniversityService.GetUniversityByID(UniversityModel model)
        {
            var university = (from data in this.dbContext.University
                              where data.UniversityID == model.UniversityID
                              select new UniversityModel
                              {
                                  UniversityID = data.UniversityID,
                                  UniversityName = data.UniversityName,
                                  UniversityDesc = data.UniversityDesc,
                                  UniversityAddress = data.UniversityAddress,
                                  CreatedBy = data.CreatedBy ?? default(int),
                                  CreatedOn = data.CreatedOn ?? default(DateTime),
                                  ModifiedBy = data.ModifiedBy ?? default(int),
                                  ModifiedOn = data.ModifiedOn ?? default(DateTime),
                                  IsActive = data.IsActive,
                                  IsDeleted = data.IsDeleted
                              }).FirstOrDefault();
            return university;
        }

        ResponseModel IUniversityService.AddUniversity(UniversityModel model, int userId)
        {
            var university = new University();
            university.UniversityName = model.UniversityName;
            university.UniversityDesc = model.UniversityDesc;
            university.UniversityAddress = model.UniversityAddress;
            university.CreatedBy = userId;
            university.CreatedOn = DateTime.UtcNow;
            university.ModifiedBy = userId;
            university.ModifiedOn = DateTime.UtcNow;
            university.IsActive = true;
            university.IsDeleted = false;

            this.dbContext.University.Add(university);
            this.dbContext.SaveChanges();
            return new ResponseModel { Message = "University added successfully", Status_Code = (int)StatusCode.Ok };
        }

        ResponseModel IUniversityService.DeleteUniversity(UniversityModel model, int userId)
        {
            var data = this.dbContext.University.FirstOrDefault(f => f.UniversityID == model.UniversityID);
            if (data == null)
            {
                return new ResponseModel { Message = "Invalid University", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                data.IsActive = false;
                data.IsDeleted = true;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;
                this.dbContext.SaveChanges();
                return new ResponseModel { Message = "University deleted successfully", Status_Code = (int)StatusCode.Ok };
            }
        }

        ResponseModel IUniversityService.UpdateUniversity(UniversityModel model, int userId)
        {
            var data = this.dbContext.University.FirstOrDefault(f => f.UniversityID == model.UniversityID);

            if (data == null)
            {
                return new ResponseModel { Message = "Invalid University", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(model.UniversityName))
                {
                    data.UniversityName = model.UniversityName;
                }

                if (!string.IsNullOrWhiteSpace(model.UniversityDesc))
                {
                    data.UniversityDesc = model.UniversityDesc;
                }

                if (!string.IsNullOrWhiteSpace(model.UniversityAddress))
                {
                    data.UniversityAddress = model.UniversityAddress;
                }

                data.IsActive = model.IsActive;
                data.IsDeleted = model.IsDeleted;
                data.ModifiedBy = userId;
                data.ModifiedOn = DateTime.UtcNow;
            }

            this.dbContext.SaveChanges();
            return new ResponseModel { Message = "University details Updated", Status_Code = (int)StatusCode.Ok };
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