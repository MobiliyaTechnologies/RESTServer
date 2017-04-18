namespace RestService.Mappings
{
    using System;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

    public class CampusModelMapping
    {
        public IQueryable<CampusModel> Map(IQueryable<Campus> source)
        {
            return from s in source
                   select new CampusModel
                   {
                       CampusID = s.CampusID,
                       CampusName = s.CampusName,
                       CampusDesc = s.CampusDesc,
                       UniversityID = s.UniversityID,
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