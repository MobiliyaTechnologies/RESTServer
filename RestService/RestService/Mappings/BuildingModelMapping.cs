namespace RestService.Mappings
{
    using System;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

    public class BuildingModelMapping
    {
        public IQueryable<BuildingModel> Map(IQueryable<Building> source)
        {
            return from s in source
                   select new BuildingModel
                   {
                       BuildingID = s.BuildingID,
                       BuildingName = s.BuildingName,
                       BuildingDesc = s.BuildingDesc,
                       CampusID = s.CampusID,
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