namespace RestService.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

    public class AnomalyInfoModelMapping
    {
        public IQueryable<AnomalyInfoModel> Map(IQueryable<AnomalyOutput> source)
        {
            return from s in source
                   select new AnomalyInfoModel
                   {
                       Id = s.Id,
                       PowerScout = s.PowerScout,
                       Timestamp = s.Timestamp ?? default(DateTime),
                       Temperature = s.Temperature ?? default(double),
                       Visibility = s.Visibility ?? default(double),
                       Days = s.days,
                       KW_System = s.kW_System ?? default(double),
                       ScoredLabels = s.ScoredLabels ?? default(double),
                       ScoredProbabilities = s.ScoredProbabilities ?? default(double),
                       Breaker_details = s.Breaker_details,
                   };
        }

        public AnomalyInfoModel Map(AnomalyOutput source)
        {
            return source == null ? null : this.Map(new List<AnomalyOutput> { source }.AsQueryable()).First();
        }
    }
}