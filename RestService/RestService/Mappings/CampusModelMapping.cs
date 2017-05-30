namespace RestService.Mappings
{
    using System;
    using System.Collections.Generic;
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
                       Latitude = s.Latitude,
                       Longitude = s.Longitude
                   };
        }

        public CampusModel Map(Campus source)
        {
            return source == null ? null : this.Map(new List<Campus> { source }.AsQueryable()).First();
        }
    }
}