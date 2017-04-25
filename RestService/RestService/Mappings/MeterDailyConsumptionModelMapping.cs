namespace RestService.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

    public class MeterDailyConsumptionModelMapping
    {
        public IQueryable<MeterDailyConsumptionModel> Map(IQueryable<DailyConsumptionDetails> source)
        {
            return from s in source
                   select new MeterDailyConsumptionModel
                   {
                       Id = s.Id,
                       AMPS_SYSTEM_AVG = s.AMPS_SYSTEM_AVG ?? default(double),
                       Building = s.Building,
                       Breaker_details = s.Breaker_details,
                       Daily_electric_cost = s.Daily_electric_cost ?? default(double),
                       Daily_KWH_System = s.Daily_KWH_System ?? default(double),
                       Monthly_electric_cost = s.Monthly_electric_cost ?? default(double),
                       Monthly_KWH_System = s.Monthly_KWH_System ?? default(double),
                       PowerScout = s.PowerScout,
                       Temperature = s.Temperature ?? default(double),
                       Timestamp = s.Timestamp ?? default(DateTime),
                       Visibility = s.Visibility ?? default(double),
                       KW_System = s.kW_System ?? default(double)
                   };
        }

        public MeterDailyConsumptionModel Map(DailyConsumptionDetails source)
        {
            return source == null ? null : this.Map(new List<DailyConsumptionDetails> { source }.AsQueryable()).FirstOrDefault();
        }
    }
}