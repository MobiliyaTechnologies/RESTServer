using RestService.Entities;
using RestService.Facade;
using RestService.Models;
using RestService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Service
{
    public class DataService
    {
        DataFacade dataFacade;
        public DataService()
        {
            dataFacade = new DataFacade();
        }

        public List<MeterDataModel> GetMeterList()
        {
            List<MeterDetails> meterData = dataFacade.GetMeters();
            if (meterData != null && meterData.Count > 0)
            {
                List<MeterDataModel> meterModelList = new List<MeterDataModel>();
                foreach (var meterDataItem in meterData)
                {
                    MonthlyConsumptionDetails meterMonthlyConsumption = dataFacade.GetMeterConsumption(meterDataItem);
                    DailyConsumptionDetails meterDailyConsumption = dataFacade.GetDailyConsumption(meterDataItem);
                    meterModelList.Add(Converter.MeterEntityToModel(meterDataItem));
                    if (meterMonthlyConsumption != null)
                    {
                        meterModelList.Where(meter => meter.Serial.Equals(meterDataItem.Serial)).LastOrDefault().MonthlyConsumption = (double)meterMonthlyConsumption.Monthly_KWH_Consumption;
                        meterModelList.Where(meter => meter.Serial.Equals(meterDataItem.Serial)).LastOrDefault().MonthlyElectricCost = (double)meterMonthlyConsumption.Monthly_Electric_Cost;
                    }
                    if (meterDailyConsumption != null)
                    {
                        meterModelList.Where(meter => meter.Serial.Equals(meterDataItem.Serial)).LastOrDefault().DailyConsumption = (double)meterDailyConsumption.Daily_KWH_System;
                        meterModelList.Where(meter => meter.Serial.Equals(meterDataItem.Serial)).LastOrDefault().DailyElectricCost = (double)meterDailyConsumption.Daily_electric_cost;
                    }
                }
                return meterModelList;
            }
            else
            {
                return new List<MeterDataModel>();
            }

        }
    }
}