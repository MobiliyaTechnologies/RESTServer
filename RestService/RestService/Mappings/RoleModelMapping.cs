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
                       Premises = (from premise in s.Premise
                                  select new PremiseModel
                                  {
                                      PremiseID = premise.PremiseID,
                                      PremiseName = premise.PremiseName,
                                      PremiseDesc = premise.PremiseDesc,
                                      OrganizationID = premise.OrganizationID,
                                      Latitude = premise.Latitude,
                                      Longitude = premise.Longitude
                                  }).ToList()
        };
        }

        public RoleModel Map(Role source)
        {
            return source == null ? null : this.Map(new List<Role> { source }.AsQueryable()).First();
        }
    }
}