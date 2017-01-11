using RestService.Entities;
using RestService.Models;
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

        public MeterMonthWiseConsumption GetMeterMonthWiseConsumption(MeterDetails meterData, int Year)
        {
            var meterDataList = (from data in dbEntity.MonthlyConsumptionDetails where data.Powerscout.Equals(meterData.Serial) && (data.Year).Equals(Year.ToString()) select data).ToList();
            MeterMonthWiseConsumption meterConsumption = new MeterMonthWiseConsumption();
            meterDataList.All(meterDataItem =>
            {
            switch (meterDataItem.Month.ToLower()){
                    case "jan":
                        meterConsumption.MonthWiseConsumption.Jan = meterConsumption.MonthWiseConsumption.Jan + (double)meterDataItem.Monthly_KWH_Consumption;
                        break;

                    case "feb":
                        meterConsumption.MonthWiseConsumption.Feb = meterConsumption.MonthWiseConsumption.Feb + (double)meterDataItem.Monthly_KWH_Consumption;
                        break;

                    case "mar":
                        meterConsumption.MonthWiseConsumption.Mar = meterConsumption.MonthWiseConsumption.Mar + (double)meterDataItem.Monthly_KWH_Consumption;
                        break;

                    case "apr":
                        meterConsumption.MonthWiseConsumption.Apr = meterConsumption.MonthWiseConsumption.Apr + (double)meterDataItem.Monthly_KWH_Consumption;
                        break;

                    case "may":
                        meterConsumption.MonthWiseConsumption.May = meterConsumption.MonthWiseConsumption.May + (double)meterDataItem.Monthly_KWH_Consumption;
                        break;

                    case "jun":
                        meterConsumption.MonthWiseConsumption.Jun = meterConsumption.MonthWiseConsumption.Jun + (double)meterDataItem.Monthly_KWH_Consumption;
                        break;

                    case "jul":
                        meterConsumption.MonthWiseConsumption.Jul = meterConsumption.MonthWiseConsumption.Jul + (double)meterDataItem.Monthly_KWH_Consumption;
                        break;

                    case "aug":
                        meterConsumption.MonthWiseConsumption.Aug = meterConsumption.MonthWiseConsumption.Aug + (double)meterDataItem.Monthly_KWH_Consumption;
                        break;

                    case "sep":
                        meterConsumption.MonthWiseConsumption.Sep = meterConsumption.MonthWiseConsumption.Sep + (double)meterDataItem.Monthly_KWH_Consumption;
                        break;

                    case "oct":
                        meterConsumption.MonthWiseConsumption.Oct = meterConsumption.MonthWiseConsumption.Oct + (double)meterDataItem.Monthly_KWH_Consumption;
                        break;

                    case "nov":
                        meterConsumption.MonthWiseConsumption.Nov = meterConsumption.MonthWiseConsumption.Nov + (double)meterDataItem.Monthly_KWH_Consumption;
                        break;

                    case "dec":
                        meterConsumption.MonthWiseConsumption.Dec = meterConsumption.MonthWiseConsumption.Dec + (double)meterDataItem.Monthly_KWH_Consumption;
                        break;

                }
                return true;
            });

            return meterConsumption;
        }
    }
}