using RestService.Entities;
using RestService.Facade;
using RestService.Models;
using RestService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace RestService.Service
{
    public class DataService
    {
        DataFacade dataFacade;
        AccountService accountService;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DataService()
        {
            dataFacade = new DataFacade();
            accountService = new AccountService();
        }

        public List<MeterDetails> GetMeterList(int UserId)
        {

            try
            {
                log.Debug("GetMeterList called");
                if (accountService.ValidateUser(UserId))
                {
                    List<MeterDetails> meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        return meterData;
                    }
                    else
                    {
                        log.Debug("GetMeterList->No data found");
                        return new List<MeterDetails>();
                    }
                }
                else
                {
                    log.Debug("GetMeterList user validation unsuccessful");
                    return null;
                }

            }
            catch (Exception ex)
            {
                log.Debug("Exception occurred in GetMeterList as: " + ex);
                throw new Exception(ex.Message);
            }

        }

        public List<MonthlyConsumptionDetails> GetMeterMonthlyConsumption(int UserId)
        {
            try
            {
                log.Debug("GetMeterMonthlyConsumption called");
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
                        log.Debug("GetMeterMonthlyConsumption->No data found");
                        return new List<MonthlyConsumptionDetails>();
                    }
                }
                else
                {
                    log.Debug("GetMeterMonthlyConsumption->User validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception occurred in GetMonthlyConsumption as: " + ex);
                throw new Exception(ex.Message);
            }
        }

        public List<DailyConsumptionDetails> GetMeterDailyConsumption(int UserId)
        {
            try
            {
                log.Debug("GetMeterDailyConsumption called");
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
                        log.Debug("GetMeterDailyConsumption->No Data found");
                        return new List<DailyConsumptionDetails>();
                    }
                }
                else
                {
                    log.Debug("GetMeterDailyConsumption->User validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Exception occurred in GetMeterDailyConsumption as: " + ex);
                throw new Exception(ex.Message);
            }
        }

        public MeterURLKey GetPowerBIUrl(int UserId, string MeterSerial)
        {
            try
            {
                if (accountService.ValidateUser(UserId))
                {
                    log.Debug("GetPowerBIUrl called");
                    string methodName = "GetURL_" + MeterSerial;
                    Type thisType = typeof(PowerBIUtil);
                    MethodInfo theMethod = thisType.GetMethod(methodName);
                    PowerBIUtil powerBIUtil = new PowerBIUtil();
                    return (MeterURLKey) theMethod.Invoke(powerBIUtil, null);
                }
                else
                {
                    log.Debug("GetPowerBIUrl->User validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetPowerBIUrl as: " + ex);
                throw new Exception(ex.Message);
            }
        }
    }
}