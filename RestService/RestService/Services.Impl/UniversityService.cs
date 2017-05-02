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
        private readonly IContextInfoAccessorService context;

        public UniversityService()
        {
            this.dbContext = new PowerGridEntities();
            this.context = new ContextInfoAccessorService();
        }

        List<UniversityModel> IUniversityService.GetAllUniversities()
        {
            var universities = this.dbContext.University.WhereActiveUniversity();
            return new UniversityModelMapping().Map(universities).ToList();
        }

        UniversityModel IUniversityService.GetUniversityByID(int universityID)
        {
            var university = this.dbContext.University.WhereActiveUniversity(data => data.UniversityID == universityID);
            return new UniversityModelMapping().Map(university).FirstOrDefault();
        }

        ResponseModel IUniversityService.AddUniversity(UniversityModel model)
        {
            var university = new University();
            university.UniversityName = model.UniversityName;
            university.UniversityDesc = model.UniversityDesc;
            university.UniversityAddress = model.UniversityAddress;
            university.CreatedBy = this.context.Current.UserId;
            university.CreatedOn = DateTime.UtcNow;
            university.ModifiedBy = this.context.Current.UserId;
            university.ModifiedOn = DateTime.UtcNow;
            university.IsActive = true;
            university.IsDeleted = false;

            this.dbContext.University.Add(university);
            this.dbContext.SaveChanges();
            return new ResponseModel { Message = "University added successfully", Status_Code = (int)StatusCode.Ok };
        }

        ResponseModel IUniversityService.DeleteUniversity(int universityId)
        {
            var data = this.dbContext.University.WhereActiveUniversity(f => f.UniversityID == universityId).FirstOrDefault();
            if (data == null)
            {
                return new ResponseModel { Message = "Invalid University", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                data.IsDeleted = true;
                data.ModifiedBy = this.context.Current.UserId;
                data.ModifiedOn = DateTime.UtcNow;
                this.dbContext.SaveChanges();
                return new ResponseModel { Message = "University deleted successfully", Status_Code = (int)StatusCode.Ok };
            }
        }

        ResponseModel IUniversityService.UpdateUniversity(UniversityModel model)
        {
            var data = this.dbContext.University.WhereActiveUniversity(f => f.UniversityID == model.UniversityID).FirstOrDefault();

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

                data.ModifiedBy = this.context.Current.UserId;
                data.ModifiedOn = DateTime.UtcNow;
            }

            this.dbContext.SaveChanges();
            return new ResponseModel { Message = "University details Updated", Status_Code = (int)StatusCode.Ok };
        }

        ResponseModel IUniversityService.AddCampusesToUniversity(int universityId, List<int> campusIds)
        {
            var campuses = this.dbContext.Campus.WhereActiveCampus(c => campusIds.Any(i => i == c.CampusID));

            if (campuses.Count() != campusIds.Count() || campuses.Count() == 0)
            {
                return new ResponseModel { Status_Code = (int)StatusCode.Error, Message = "Campus does not exists for given ids" };
            }

            var university = this.dbContext.University.WhereActiveUniversity(u => u.UniversityID == universityId);

            if (university == null)
            {
                return new ResponseModel { Status_Code = (int)StatusCode.Error, Message = string.Format("University does not exists for id - {0}", universityId) };
            }

            var newCampusToAdd = campuses.Where(c => c.UniversityID != universityId);

            foreach (var campus in newCampusToAdd)
            {
                campus.UniversityID = universityId;
            }

            this.dbContext.SaveChanges();

            return new ResponseModel { Status_Code = (int)StatusCode.Ok, Message = "Campuses added to university successfully." };
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