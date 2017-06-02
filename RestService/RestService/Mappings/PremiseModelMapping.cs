namespace RestService.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

    public class PremiseModelMapping
    {
        public IQueryable<PremiseModel> Map(IQueryable<Premise> source)
        {
            return from s in source
                   select new PremiseModel
                   {
                       PremiseID = s.PremiseID,
                       PremiseName = s.PremiseName,
                       PremiseDesc = s.PremiseDesc,
                       OrganizationID = s.OrganizationID,
                       Latitude = s.Latitude,
                       Longitude = s.Longitude
                   };
        }

        public PremiseModel Map(Premise source)
        {
            return source == null ? null : this.Map(new List<Premise> { source }.AsQueryable()).First();
        }
    }
}