namespace RestService.Mappings
{
    using System;
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