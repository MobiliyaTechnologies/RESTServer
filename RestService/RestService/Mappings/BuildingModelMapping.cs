namespace RestService.Mappings
{
    using System;
    using System.Collections.Generic;
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

        public BuildingModel Map(Building source)
        {
            return source == null ? null : this.Map(new List<Building> { source }.AsQueryable()).First();
        }
    }
}