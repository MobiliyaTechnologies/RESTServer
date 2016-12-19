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
        AccountService accountService;
        public DataService()
        {
            dataFacade = new DataFacade();
            accountService = new AccountService();
        }

        public List<MeterDetails> GetMeterList(int UserId)
        {

            try
            {
                if (accountService.ValidateUser(UserId))
                {
                    List<MeterDetails> meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        return meterData;
                    }
                    else
                    {
                        return new List<MeterDetails>();
                    }
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public List<MonthlyConsumptionDetails> GetMeterMonthlyConsumption(int UserId)
        {
            try
            {
                if (accountService.ValidateUser(UserId))
                {
                    List<MeterDetails> meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        List<MonthlyConsumptionDetails> meterModelList = new List<MonthlyConsumptionDetails>();
                        foreach (var meterDataItem in meterData)
                        {
                            MonthlyConsumptionDetails meterMonthlyConsumption = dataFacade.GetMeterConsumption(meterDataItem);
                            if (meterMonthlyConsumption != null)
                            {
                                meterModelList.Add(meterMonthlyConsumption);
                            }
                        }
                        return meterModelList;
                    }
                    else
                    {
                        return new List<MonthlyConsumptionDetails>();
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<DailyConsumptionDetails> GetMeterDailyConsumption(int UserId)
        {
            try
            {
                if (accountService.ValidateUser(UserId))
                {
                    List<MeterDetails> meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        List<DailyConsumptionDetails> meterModelList = new List<DailyConsumptionDetails>();
                        foreach (var meterDataItem in meterData)
                        {
                            DailyConsumptionDetails meterMonthlyConsumption = dataFacade.GetDailyConsumption(meterDataItem);
                            if (meterMonthlyConsumption != null)
                            {
                                meterModelList.Add(meterMonthlyConsumption);
                            }
                        }
                        return meterModelList;
                    }
                    else
                    {
                        return new List<DailyConsumptionDetails>();
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //public List<MonthlyConsumptionModel> GetMeterMonthlyConsumption()
        //{
        //    List<MeterDetails> meterData = dataFacade.GetMeters();
        //    if (meterData != null && meterData.Count > 0)
        //    {
        //        List<MonthlyConsumptionModel> meterModelList = new List<MonthlyConsumptionModel>();
        //        foreach (var meterDataItem in meterData)
        //        {
        //            MonthlyConsumptionDetails meterMonthlyConsumption = dataFacade.GetMeterConsumption(meterDataItem);
        //            if (meterMonthlyConsumption != null)
        //            {
        //                meterModelList.Add(Converter.MeterMonthlyEntityToModel(meterMonthlyConsumption));
        //            }
        //        }
        //        return meterModelList;
        //    }
        //    else
        //    {
        //        return new List<MonthlyConsumptionModel>();
        //    }
        //}

        //public List<DailyConsumptionModel> GetMeterDailyConsumption()
        //{
        //    List<MeterDetails> meterData = dataFacade.GetMeters();
        //    if (meterData != null && meterData.Count > 0)
        //    {
        //        List<DailyConsumptionModel> meterModelList = new List<DailyConsumptionModel>();
        //        foreach (var meterDataItem in meterData)
        //        {
        //            DailyConsumptionDetails meterDailyConsumption = dataFacade.GetDailyConsumption(meterDataItem);
        //            if (meterDailyConsumption != null)
        //            {
        //                meterModelList.Add(Converter.MeterDailyEntityToModel(meterDailyConsumption));
        //            }
        //        }
        //        return meterModelList;
        //    }
        //    else
        //    {
        //        return new List<DailyConsumptionModel>();
        //    }
        //}
    }
}