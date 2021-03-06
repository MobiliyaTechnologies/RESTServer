﻿namespace RestService.Mappings
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
                       PremiseID = s.PremiseID ?? default(int),
                       Latitude = s.Latitude ?? default(decimal),
                       Longitude = s.Longitude ?? default(decimal)
                   };
        }

        public BuildingModel Map(Building source)
        {
            return source == null ? null : this.Map(new List<Building> { source }.AsQueryable()).First();
        }
    }
}