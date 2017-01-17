﻿using RestService.Entities;
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

        public List<MeterDetailsModel> GetMeterList(int UserId)
        {

            try
            {
                log.Debug("GetMeterList called");
                if (accountService.ValidateUser(UserId))
                {
                    List<MeterDetails> meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        List<MeterDetailsModel> meterDetails = new List<MeterDetailsModel>();
                        meterData.All(meterDataItem =>
                        {
                            meterDetails.Add(Converter.MeterDetailsEntityToModel(meterDataItem));
                            return true;
                        });
                        return meterDetails;
                    }
                    else
                    {
                        log.Debug("GetMeterList->No data found");
                        return new List<MeterDetailsModel>();
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
                throw new Exception(ex.Message, ex);
            }

        }

        public List<MeterMonthlyConsumptionModel> GetMeterMonthlyConsumption(int UserId)
        {
            try
            {
                log.Debug("GetMeterMonthlyConsumption called");
                if (accountService.ValidateUser(UserId))
                {
                    List<MeterDetails> meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        List<MeterMonthlyConsumptionModel> meterModelList = new List<MeterMonthlyConsumptionModel>();
                        foreach (var meterDataItem in meterData)
                        {
                            MeterMonthlyConsumptionModel meterMonthlyConsumption = Converter.MeterMonthlyEntityToModel(dataFacade.GetMeterConsumption(meterDataItem));
                            if (meterMonthlyConsumption != null)
                            {
                                meterMonthlyConsumption.Name = meterDataItem.Breaker_details;
                                meterModelList.Add(meterMonthlyConsumption);
                            }
                        }
                        return meterModelList;
                    }
                    else
                    {
                        log.Debug("GetMeterMonthlyConsumption->No data found");
                        return new List<MeterMonthlyConsumptionModel>();
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
                throw new Exception(ex.Message, ex);
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
                throw new Exception(ex.Message, ex);
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
                    return (MeterURLKey)theMethod.Invoke(powerBIUtil, null);
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
                throw new Exception(ex.Message, ex);
            }
        }

        public List<MeterMonthWiseConsumption> GetMonthWiseConsumption(int UserId, int Year)
        {
            try
            {
                log.Debug("GetMonthWiseConsumption called");
                if (accountService.ValidateUser(UserId))
                {
                    List<MeterDetails> meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        List<MeterMonthWiseConsumption> meterDataList = new List<MeterMonthWiseConsumption>();
                        foreach (var meterDataItem in meterData)
                        {
                            MeterMonthWiseConsumption meterMonthWiseConsumption = dataFacade.GetMeterMonthWiseConsumption(meterDataItem, Year);
                            if (meterMonthWiseConsumption != null)
                            {
                                meterMonthWiseConsumption.PowerScout = meterDataItem.Serial;
                                meterMonthWiseConsumption.Name = meterDataItem.Breaker_details;
                                meterDataList.Add(meterMonthWiseConsumption);
                            }
                        }
                        return meterDataList;
                    }
                    else
                    {
                        log.Debug("GetMonthWiseConsumption->No data found");
                        return new List<MeterMonthWiseConsumption>();
                    }
                }
                else
                {
                    log.Debug("GetMonthWiseConsumption -> User validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetMonthWiseConsumption as: " + ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<MeterWeekWiseMonthlyConsumption> GetWeekWiseMonthlyConsumption(int UserId, string Month, int Year)
        {
            try
            {
                log.Debug("GetWeekWiseConsumption called");
                if (accountService.ValidateUser(UserId))
                {
                    List<MeterDetails> meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        List<MeterWeekWiseMonthlyConsumption> weekWiseConsumption = new List<MeterWeekWiseMonthlyConsumption>();
                        foreach (var meter in meterData)
                        {
                            List<DailyConsumptionDetails> dailyConsumptionListForMonth = dataFacade.GetDailyConsumptionForMonth(meter, Month, Year);
                            if(dailyConsumptionListForMonth == null || dailyConsumptionListForMonth.Count < 1)
                            {
                                log.Debug("GetWeekWiseMonthlyConsumption -> No data found");
                                return new List<MeterWeekWiseMonthlyConsumption>();
                            }
                            var meterWeekWiseConsumption = GetWeekWiseConsumptionFromMonthly(dailyConsumptionListForMonth);
                            weekWiseConsumption.Add(meterWeekWiseConsumption);
                        }
                        return weekWiseConsumption;
                    }
                    else
                    {
                        log.Debug("GetWeekWiseMonthlyConsumption -> No data found");
                        return new List<MeterWeekWiseMonthlyConsumption>();
                    }

                }
                else
                {
                    log.Debug("GetWeekWiseMonthlyConsumption -> User Validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetWeekWiseMonthlyConsumption as: " + ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public MeterWeekWiseMonthlyConsumption GetWeekWiseConsumptionFromMonthly(List<DailyConsumptionDetails> dailyConsumptionList)
        {
            //try
            //{
            //    MeterWeekWiseMonthlyConsumption weekWiseConsumption = new MeterWeekWiseMonthlyConsumption();
            //    DateTime startDate = ((DateTime)dailyConsumptionList.FirstOrDefault().Timestamp).Date;
            //    int counter = 0;
            //    while (counter < DateTime.DaysInMonth(startDate.Year, startDate.Month))
            //    {
            //        int range = 8 - ServiceUtil.GetDayOfWeek(startDate.ToString("ddd"));
            //        if(counter + range > DateTime.DaysInMonth(startDate.Year, startDate.Month))
            //        {
            //            range = DateTime.DaysInMonth(startDate.Year, startDate.Month) - counter;
            //        }
            //        var weekList = dailyConsumptionList.GetRange(counter, range);
            //        startDate = ((DateTime)weekList.LastOrDefault().Timestamp).AddDays(1);
            //        weekWiseConsumption.WeekWiseConsumption.Add(weekList.Sum(data => (double)data.Daily_KWH_System));
            //        counter = counter + weekList.Count;
            //    }
            //    weekWiseConsumption.PowerScout = dailyConsumptionList.FirstOrDefault().PowerScout;
            //    return weekWiseConsumption;
            //}
            //catch (Exception ex)
            //{
            //    log.Error("Exception occurred in GetWeekWiseConsumptionFromMonthly as: " + ex);
            //    return new MeterWeekWiseMonthlyConsumption { PowerScout = dailyConsumptionList.FirstOrDefault().PowerScout };
            //}

            try
            {
                MeterWeekWiseMonthlyConsumption weekWiseConsumption = new MeterWeekWiseMonthlyConsumption();
                DateTime startDate = ((DateTime)dailyConsumptionList.FirstOrDefault().Timestamp).Date;
                int counter = 0;
                while (counter < dailyConsumptionList.Count)
                {
                    int range = 8 - ServiceUtil.GetDayOfWeek(startDate.ToString("ddd"));
                    if (counter + range > dailyConsumptionList.Count)
                    {
                        range = dailyConsumptionList.Count - counter;
                    }
                    var weekList = dailyConsumptionList.GetRange(counter, range);
                    startDate = ((DateTime)weekList.LastOrDefault().Timestamp).AddDays(1);
                    weekWiseConsumption.WeekWiseConsumption.Add(weekList.Sum(data => (double)data.Daily_KWH_System));
                    counter = counter + weekList.Count;
                }
                weekWiseConsumption.PowerScout = dailyConsumptionList.FirstOrDefault().PowerScout;
                return weekWiseConsumption;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetWeekWiseConsumptionFromMonthly as: " + ex);
                return new MeterWeekWiseMonthlyConsumption { PowerScout = dailyConsumptionList.FirstOrDefault().PowerScout };
            }
        }
    }
}