namespace RestService.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

    public class MeterDetailsModelMapping
    {
        public IQueryable<MeterDetailsModel> Map(IQueryable<MeterDetails> source)
        {
            return from s in source
                   select new MeterDetailsModel
                   {
                       Id = s.Id,
                       PowerScout = s.PowerScout,
                       Name = s.Breaker_details,
                   };
        }

        public MeterDetailsModel Map(MeterDetails source)
        {
            return source == null ? null : this.Map(new List<MeterDetails> { source }.AsQueryable()).First();
        }
    }
}