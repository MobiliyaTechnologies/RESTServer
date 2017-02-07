using RestService.Entities;
using RestService.Facade;
using RestService.Models;
using RestService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using RestService.Models;

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

        public PowerBIGeneralURL GetPowerBIGeneralURL(int UserId)
        {
            try
            {
                log.Debug("GetPowerBIGeneralURL called");
                if (accountService.ValidateUser(UserId))
                {
                    return new PowerBIGeneralURL();
                }
                else
                {
                    log.Debug("GetPowerBIGeneralURL -> User Validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetPowerBIGeneralURL as: " + ex);
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

        public List<MeterMonthWiseConsumption> GetMonthWiseConsumptionForOffset(int UserId, string Month, int Year, int Offset)
        {
            try
            {
                log.Debug("GetMonthWiseConsumptionForOffset called");
                if (accountService.ValidateUser(UserId))
                {
                    List<MeterDetails> meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        List<MeterMonthWiseConsumption> monthWiseDataList = new List<MeterMonthWiseConsumption>();
                        foreach (var meterDataItem in meterData)
                        {
                            List<MonthlyConsumptionDetails> monthlyDataList = dataFacade.GetMeterMonthWiseConsumptionForOffset(meterDataItem, Month, Year, Offset);
                            if (monthlyDataList == null || monthlyDataList.Count < 1)
                            {
                                log.Debug("GetMonthwiseConsumptionForOffset -> No Data found for meter: " + meterDataItem.PowerScout);
                                monthWiseDataList.Add(new MeterMonthWiseConsumption { PowerScout = meterDataItem.PowerScout, Name = meterDataItem.Breaker_details });
                                continue;
                            }
                            if (monthlyDataList.Count > Offset)
                                monthlyDataList.RemoveRange(Offset, monthlyDataList.Count - Offset);

                            monthWiseDataList.Add(Converter.MeterMonthWiseEntityToModel(monthlyDataList));
                        }
                        return monthWiseDataList;
                    }
                    else
                    {
                        log.Debug("GetMonthWiseConsumptionForOffset -> No data found");
                        return new List<MeterMonthWiseConsumption>();
                    }
                }
                else
                {
                    log.Debug("GetMonthWiseConsumptionForOffset -> User validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetMonthWiseConsumptionForOffset as: " + ex);
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
                            if (dailyConsumptionListForMonth == null || dailyConsumptionListForMonth.Count < 1)
                            {
                                log.Debug("GetWeekWiseMonthlyConsumption -> No data found for meter: " + meter.PowerScout);
                                weekWiseConsumption.Add(new MeterWeekWiseMonthlyConsumption { PowerScout = meter.PowerScout, Name = meter.Breaker_details });
                                continue;
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

        public List<MeterWeekWiseMonthlyConsumption> GetWeekWiseMonthlyConsumptionForOffset(int UserId, string Month, int Year, int Offset)
        {
            try
            {
                log.Debug("GetWeekWiseMonthlyConsumptionForOffset called");
                if (accountService.ValidateUser(UserId))
                {
                    List<MeterDetails> meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        List<MeterWeekWiseMonthlyConsumption> weekWiseConsumption = new List<MeterWeekWiseMonthlyConsumption>();
                        foreach (var meter in meterData)
                        {
                            MeterWeekWiseMonthlyConsumption meterWeekWiseConsumption = new MeterWeekWiseMonthlyConsumption();
                            DateTime monthDate = DateTime.Parse("01/" + Month + "/" + Year);
                            string month = Month;
                            int year = Year;
                            while (meterWeekWiseConsumption.WeekWiseConsumption.Count < Offset)
                            {

                                List<DailyConsumptionDetails> dailyConsumptionListForMonth = dataFacade.GetDailyConsumptionForMonth(meter, month, year);
                                if (dailyConsumptionListForMonth == null || dailyConsumptionListForMonth.Count < 1)
                                {
                                    log.Debug("GetWeekWiseMonthlyConsumptionForOffset -> No data found for meter: " + meter.PowerScout);
                                    weekWiseConsumption.Add(new MeterWeekWiseMonthlyConsumption { PowerScout = meter.PowerScout, Name = meter.Breaker_details });
                                    break;
                                }
                                var weekWiseList = GetWeekWiseConsumptionFromMonthly(dailyConsumptionListForMonth).WeekWiseConsumption;
                                if (weekWiseList.Count + meterWeekWiseConsumption.WeekWiseConsumption.Count > Offset)
                                {
                                    weekWiseList.Reverse();
                                    meterWeekWiseConsumption.WeekWiseConsumption.AddRange(weekWiseList.Take(Offset - meterWeekWiseConsumption.WeekWiseConsumption.Count));
                                }
                                else
                                {
                                    meterWeekWiseConsumption.WeekWiseConsumption.AddRange(weekWiseList);
                                }
                                month = monthDate.AddMonths(-1).ToString("MMM");
                                year = monthDate.AddMonths(-1).Year;
                            }
                            meterWeekWiseConsumption.PowerScout = meter.PowerScout;
                            meterWeekWiseConsumption.Name = meter.Breaker_details;

                            weekWiseConsumption.Add(meterWeekWiseConsumption);
                        }
                        return weekWiseConsumption;
                    }
                    else
                    {
                        log.Debug("GetWeekWiseMonthlyConsumptionForOffset -> No data found");
                        return new List<MeterWeekWiseMonthlyConsumption>();
                    }
                }
                else
                {
                    log.Debug("GetWeekWiseMonthlyConsumptionForOffset -> User Validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetWeekWiseMonthlyConsumptionForOffset as: " + ex);
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
        public List<MeterDayWiseMonthlyConsumption> GetDayWiseMonthlyConsumption(int UserId, string Month, int Year)
        {
            try
            {
                log.Debug("GetDayWiseMonthlyConsumption called");
                if (accountService.ValidateUser(UserId))
                {
                    var meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        List<MeterDayWiseMonthlyConsumption> dayWiseConsumptionList = new List<MeterDayWiseMonthlyConsumption>();
                        foreach (var meter in meterData)
                        {
                            List<DailyConsumptionDetails> meterDailyConsumption = dataFacade.GetDailyConsumptionForMonth(meter, Month, Year);
                            if (meterDailyConsumption == null || meterDailyConsumption.Count < 1)
                            {
                                log.Debug("GetDayWiseMonthlyConsumption -> No Data found");
                                return new List<MeterDayWiseMonthlyConsumption>();
                            }
                            //Conversion from Entity to Model
                            var dayWiseConsumption = Converter.MeterDayWiseEntityToModel(meterDailyConsumption);
                            if (dayWiseConsumption != null)
                                dayWiseConsumptionList.Add(dayWiseConsumption);
                        }
                        return dayWiseConsumptionList;
                    }
                    else
                    {
                        log.Debug("GetDayWiseMonthlyConsumption -> No Data found");
                        return new List<MeterDayWiseMonthlyConsumption>();
                    }
                }
                else
                {
                    log.Debug("GetDayWiseMonthlyConsumption -> User valdation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetDayWiseMonthlyConsumption as: " + ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<MeterDayWiseMonthlyConsumptionPrediction> GetDayWiseCurrentMonthPrediction(int UserId, string Month, int Year)
        {
            try
            {
                log.Debug("GetDayWiseCurrentMonthPrediction called");
                if (accountService.ValidateUser(UserId))
                {
                    var meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        List<MeterDayWiseMonthlyConsumptionPrediction> dayWisePredictionList = new List<MeterDayWiseMonthlyConsumptionPrediction>();
                        foreach (var meter in meterData)
                        {
                            var dailyPredictionList = dataFacade.GetDayWiseNextMonthPrediction(meter, Month, Year);
                            if (dailyPredictionList == null || dailyPredictionList.Count < 1)
                            {
                                log.Debug("GetDayWiseCurrentMonthPrediction -> No Data found");
                                return new List<MeterDayWiseMonthlyConsumptionPrediction>();
                            }
                            dayWisePredictionList.Add(Converter.MeterDayWiseMonthlyPredictionEntityToModel(dailyPredictionList));
                        }
                        return dayWisePredictionList;
                    }
                    else
                    {
                        log.Debug("GetDayWiseCurrentMonthPrediction -> No data found");
                        return new List<MeterDayWiseMonthlyConsumptionPrediction>();
                    }
                }
                else
                {
                    log.Debug("GetDayWiseCurrentMonthPrediction -> User validation failed");
                    return null;
                }

            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetDayWiseCurrentMonthPrediction as: " + ex);
                return new List<MeterDayWiseMonthlyConsumptionPrediction>();
            }
        }
        public List<MeterDayWiseMonthlyConsumptionPrediction> GetDayWiseNextMonthPrediction(int UserId, string Month, int Year)
        {
            try
            {
                log.Debug("GetDayWiseNextMonthPrediction called");
                if (accountService.ValidateUser(UserId))
                {
                    var meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        List<MeterDayWiseMonthlyConsumptionPrediction> dayWisePredictionList = new List<MeterDayWiseMonthlyConsumptionPrediction>();
                        DateTime monthDate;
                        DateTime.TryParse("01-" + Month + "-" + Year, out monthDate);
                        monthDate = monthDate.AddMonths(1);
                        Month = monthDate.ToString("MMM");
                        Year = monthDate.Year;
                        foreach (var meter in meterData)
                        {
                            var dailyPredictionList = dataFacade.GetDayWiseNextMonthPrediction(meter, Month, Year);
                            if (dailyPredictionList == null || dailyPredictionList.Count < 1)
                            {
                                log.Debug("GetDayWiseNextMonthPrediction -> No Data found");
                                return new List<MeterDayWiseMonthlyConsumptionPrediction>();
                            }
                            dayWisePredictionList.Add(Converter.MeterDayWiseMonthlyPredictionEntityToModel(dailyPredictionList));
                        }
                        return dayWisePredictionList;
                    }
                    else
                    {
                        log.Debug("GetDayWiseNextMonthPrediction -> No data found");
                        return new List<MeterDayWiseMonthlyConsumptionPrediction>();
                    }
                }
                else
                {
                    log.Debug("GetDayWiseMonthlyPrediction -> User validation failed");
                    return null;
                }

            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetDayWiseNextMonthPrediction as: " + ex);
                return new List<MeterDayWiseMonthlyConsumptionPrediction>();
            }
        }

        public List<AlertModel> GetAllAlerts(int UserId)
        {
            try
            {
                log.Debug("GetAllAlerts called");
                if (accountService.ValidateUser(UserId))
                {
                    log.Debug("GetAllAlerts -> User validation successful");
                    List<AlertModel> alertModelList = new List<AlertModel>();
                    var data = dataFacade.GetAllAlerts();
                    if (data == null || data.Count < 1)
                    {
                        log.Debug("GetAllAlerts -> No alerts found");
                        return alertModelList;
                    }
                    
                    data.All(alert =>
                    {
                        alertModelList.Add(Converter.AlertsEntityToModel(alert));
                        return true;
                    });
                    return alertModelList;
                }
                else
                {
                    log.Debug("GetAllAlerts -> User Validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetAllAlerts as: " + ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public AlertDetailsModel GetAlertDetails(int UserId, int LogId)
        {
            try
            {
                log.Debug("GetAlertDetails called");
                if (accountService.ValidateUser(UserId))
                {
                    var alertDetails = dataFacade.GetAlertDetails(LogId);
                    if(alertDetails == null)
                    {
                        log.Debug("GetAlertDetails -> No Data Found");
                        return new AlertDetailsModel();
                    }
                    return Converter.AlertDetailsEntityToModel(alertDetails);
                }
                else
                {
                    log.Debug("GetAlertDetails -> User Validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occured in GetAlertDetails as: " + ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<ClassroomModel> GetAllClassrooms(int UserId)
        {
            try
            {
                log.Debug("GetAllClassrooms called");
                if (accountService.ValidateUser(UserId))
                {
                    log.Debug("GetAllClassrooms -> User validation successful");
                    List<ClassroomModel> classroomModel = new List<ClassroomModel>();
                    var data = dataFacade.GetAllClassrooms();
                    if (data == null || data.Count < 1)
                    {
                        log.Debug("GetAllClassrooms -> No classrooms found");
                        return classroomModel;
                    }

                    data.All(classroom =>
                    {
                        classroomModel.Add(Converter.ClassroomEntityToModel(classroom));
                        return true;
                    });
                    return classroomModel;
                }
                else
                {
                    log.Debug("GetAllClassrooms -> User Validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetAllClassrooms as: " + ex);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}