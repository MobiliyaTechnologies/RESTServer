namespace RestService.Mappings
{
    using System;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

    public class UniversityModelMapping
    {
        public IQueryable<UniversityModel> Map(IQueryable<University> source)
        {
            return from s in source
                   select new UniversityModel
                   {
                       UniversityID = s.UniversityID,
                       UniversityName = s.UniversityName,
                       UniversityDesc = s.UniversityDesc,
                       UniversityAddress = s.UniversityAddress,
                       IsActive = s.IsActive,
                       CreatedBy = s.CreatedBy ?? default(int),
                       CreatedOn = s.CreatedOn ?? default(DateTime),
                       ModifiedBy = s.ModifiedBy ?? default(int),
                       ModifiedOn = s.ModifiedOn ?? default(DateTime),
                       IsDeleted = s.IsDeleted
                   };
        }
    }
}