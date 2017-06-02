namespace RestService.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

    public class OrganizationModelMapping
    {
        public IQueryable<OrganizationModel> Map(IQueryable<Organization> source)
        {
            return from s in source
                   select new OrganizationModel
                   {
                       OrganizationName = s.OrganizationName,
                       OrganizationDesc = s.OrganizationDesc,
                       OrganizationAddress = s.OrganizationAddress,
                       OrganizationID = s.OrganizationID
                   };
        }

        public OrganizationModel Map(Organization source)
        {
            return source == null ? null : this.Map(new List<Organization> { source }.AsQueryable()).First();
        }
    }
}