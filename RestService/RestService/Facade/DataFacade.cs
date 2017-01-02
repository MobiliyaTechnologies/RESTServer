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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataFacade()
        {
            dbEntity = new PowerGridEntities();
        }

        public List<MeterDetails> GetMeters()
        {
            log.Debug("GetMeters called");
            var meterList = (from data in dbEntity.MeterDetails select data).ToList<MeterDetails>();
            return meterList;
        }

        public MonthlyConsumptionDetails GetMeterConsumption(MeterDetails meter)
        {
            log.Debug("GetMeterConsumption called");
            string currentMonth = DateTime.UtcNow.ToString("MMM");
            var meterConsumption = (from data in dbEntity.MonthlyConsumptionDetails where meter.Serial.Equals(data.Powerscout) && currentMonth.Equals(data.Month) select data).FirstOrDefault();
            return meterConsumption;
        }

        public DailyConsumptionDetails GetDailyConsumption(MeterDetails meter)
        {
            log.Debug("GetDailyConsumption called");
            DateTime today = DateTime.UtcNow.AddHours(-6);
            var meterConsumption = (from data in dbEntity.DailyConsumptionDetails where meter.Serial.Equals(data.PowerScout) && today.Day == ((DateTime)data.Timestamp).Day && today.Month == ((DateTime)data.Timestamp).Month && today.Year == ((DateTime)data.Timestamp).Year select data).ToList();
            return meterConsumption.LastOrDefault();
        }
    }
}