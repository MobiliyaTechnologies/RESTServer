namespace RestService.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

    public class RoleModelMapping
    {
        public IQueryable<RoleModel> Map(IQueryable<Role> source)
        {
            return from s in source
                   select new RoleModel
                   {
                       Id = s.Id,
                       RoleName = s.RoleName,
                       Description = s.Description,
                       Campuses = (from campus in s.Campus
                                  select new CampusModel
                                  {
                                      CampusID = campus.CampusID,
                                      CampusName = campus.CampusName,
                                      CampusDesc = campus.CampusDesc,
                                      UniversityID = campus.UniversityID,
                                      Latitude = campus.Latitude,
                                      Longitude = campus.Longitude
                                  }).ToList()
        };
        }

        public RoleModel Map(Role source)
        {
            return source == null ? null : this.Map(new List<Role> { source }.AsQueryable()).First();
        }
    }
}