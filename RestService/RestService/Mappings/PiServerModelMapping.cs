namespace RestService.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

    public class PiServerModelMapping
    {
        public IQueryable<PiServerModel> Map(IQueryable<PiServer> source)
        {
            return from s in source
                   select new PiServerModel
                   {
                       PiServerID = s.PiServerID,
                       PiServerName = s.PiServerName,
                       PiServerDesc = s.PiServerDesc,
                       PiServerURL = s.PiServerURL,
                       PremiseID = s.Premise.PremiseID
                   };
        }

        public PiServerModel Map(PiServer source)
        {
            return source == null ? null : this.Map(new List<PiServer> { source }.AsQueryable()).First();
        }
    }
}