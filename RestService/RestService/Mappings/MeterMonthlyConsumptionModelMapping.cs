﻿namespace RestService.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

    public class MeterMonthlyConsumptionModelMapping
    {
        public IQueryable<MeterMonthlyConsumptionModel> Map(IQueryable<MonthlyConsumptionDetails> source)
        {
            return from s in source
                   select new MeterMonthlyConsumptionModel
                   {
                       Id = s.Id,
                       Month = s.Month,
                       Year = s.Year,
                       Powerscout = s.PowerScout,
                       Monthly_KWH_Consumption = s.Monthly_KWH_System ?? default(double),
                       Monthly_Electric_Cost = s.Monthly_electric_cost ?? default(double)
                   };
        }

        public MeterMonthlyConsumptionModel Map(MonthlyConsumptionDetails source)
        {
            return source == null ? null : this.Map(new List<MonthlyConsumptionDetails> { source }.AsQueryable()).FirstOrDefault();
        }
    }
}