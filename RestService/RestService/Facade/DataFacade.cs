using RestService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Facade
{
    public class DataFacade
    {
        PowerGridEntities dbEntity;
        public DataFacade()
        {
            dbEntity = new PowerGridEntities();
        }

        public List<MeterDetails> GetMeters()
        {
            var meterList = (from data in dbEntity.MeterDetails select data).ToList<MeterDetails>();
            return meterList;
        }

        public MonthlyConsumptionDetails GetMeterConsumption(MeterDetails meter)
        {
            var meterConsumption = (from data in dbEntity.MonthlyConsumptionDetails where meter.Name.Equals(data.Powerscout) orderby data.Id select data).ToList();
            return meterConsumption.LastOrDefault();
        }

        public DailyConsumptionDetails GetDailyConsumption(MeterDetails meter)
        {
            var meterConsumption = (from data in dbEntity.DailyConsumptionDetails where meter.Name.Equals(data.PowerScout) orderby data.Id select data).ToList();
            return meterConsumption.LastOrDefault();
        }
    }
}