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
                       IsActive = s.IsActive,
                       CreatedBy = s.CreatedBy ?? default(int),
                       CreatedOn = s.CreatedOn ?? default(DateTime),
                       ModifiedBy = s.ModifiedBy ?? default(int),
                       ModifiedOn = s.ModifiedOn ?? default(DateTime),
                       IsDeleted = s.IsDeleted,
                       Campuses = (from campus in s.Campus
                                  select new CampusModel
                                  {
                                      CampusID = campus.CampusID,
                                      CampusName = campus.CampusName,
                                      CampusDesc = campus.CampusDesc,
                                      UniversityID = campus.UniversityID,
                                      IsActive = campus.IsActive,
                                      CreatedBy = campus.CreatedBy ?? default(int),
                                      CreatedOn = campus.CreatedOn ?? default(DateTime),
                                      ModifiedBy = campus.ModifiedBy ?? default(int),
                                      ModifiedOn = campus.ModifiedOn ?? default(DateTime),
                                      IsDeleted = campus.IsDeleted,
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