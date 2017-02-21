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

                    //data.All(alert =>
                    //{
                    //    alertModelList.Add(Converter.AlertsEntityToModel(alert));
                    //    return true;
                    //});
                    //return alertModelList;

                    return data;
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
                    if (alertDetails == null)
                    {
                        log.Debug("GetAlertDetails -> No Data Found");
                        return new AlertDetailsModel();
                    }
                    //return Converter.AlertDetailsEntityToModel(alertDetails);
                    return alertDetails;
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

        public ResponseModel AcknowledgeAlert(int UserId, AlertModel alertDetail)
        {
            try
            {
                log.Debug("Acknowledge alert called");
                if (accountService.ValidateUser(UserId))
                {
                    log.Debug("AcknowledgeAlert -> User validation successful");
                    var data = dataFacade.AcknowledgeAlert(alertDetail);
                    return data;
                }
                else
                {
                    log.Debug("AcknowledgeAlert -> User validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in AcknowledgeAlert as :" + ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public ResponseModel StoreFeedback(int UserId, Feedback feedbackdetail)
        {
            try
            {
                log.Debug("StoreFeedback called");
                if (accountService.ValidateUser(UserId))
                {
                    log.Debug("StoreFeedback -> User validation successful");
                    var data = dataFacade.StoreFeedback(UserId, feedbackdetail);
                    return data;
                }
                else
                {
                    log.Debug("StoreFeedback -> User validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in StoreFeedback as :" + ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<FeedbackModel> GetAllFeedback(int UserId)
        {
            try
            {
                log.Debug("GetAllFeedback called");
                if (accountService.ValidateUser(UserId))
                {
                    log.Debug("GetAllFeedback -> User validation successful");
                    List<FeedbackModel> feedbackModel = new List<FeedbackModel>();
                    var data = dataFacade.GetAllFeedback();
                    if (data == null || data.Count < 1)
                    {
                        log.Debug("GetAllFeedback -> No feedback found");
                        return new List<FeedbackModel>();
                    }

                    data.All(feedback =>
                    {
                        feedbackModel.Add(Converter.FeedbackEntityToModel(feedback));
                        return true;
                    });
                    return feedbackModel;
                }
                else
                {
                    log.Debug("GetAllFeedback -> User Validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetAllFeedback as: " + ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public ResponseModel FeedbackDelete(int UserId, FeedbackModel feedbackdetail)
        {
            try
            {
                log.Debug("FeedbackDelete called");
                if (accountService.ValidateUser(UserId))
                {
                    log.Debug("FeedbackDelete -> User validation successful");
                    var data = dataFacade.FeedbackDelete(feedbackdetail);
                    return data;
                }
                else
                {
                    log.Debug("FeedbackDelete -> User validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in FeedbackDelete as :" + ex);
                throw new Exception(ex.Message, ex);
            }
        }
        public ResponseModel FeedbackUpdate(int UserId, FeedbackModel feedbackdetail)
        {
            try
            {
                log.Debug("FeedbackUpdate called");
                if (accountService.ValidateUser(UserId))
                {
                    log.Debug("FeedbackUpdate -> User validation successful");
                    var data = dataFacade.FeedbackUpdate(UserId, feedbackdetail);
                    return data;
                }
                else
                {
                    log.Debug("FeedbackUpdate -> User validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in FeedbackUpdate as :" + ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<SensorModel> GetAllSensors(int UserId)
        {
            try
            {
                log.Debug("GetAllSensors called");
                if (accountService.ValidateUser(UserId))
                {
                    log.Debug("GetAllSensors -> User validation successful");
                    List<SensorModel> sensorModel = new List<SensorModel>();
                    var data = dataFacade.GetAllSensors();
                    if (data == null || data.Count < 1)
                    {
                        log.Debug("GetAllSensors -> No classrooms found");
                        return sensorModel;
                    }

                    //data.All(sensor =>
                    //{
                    //    sensorModel.Add(Converter.SensorMasterEntityToModel(sensor));
                    //    return true;
                    //});

                    return data;
                }
                else
                {
                    log.Debug("GetAllSensors -> User Validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetAllSensors as: " + ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public ResponseModel MapSensor(int UserId, SensorModel sensorDetail)
        {
            try
            {
                log.Debug("MapSensor called");
                if (accountService.ValidateUser(UserId))
                {
                    log.Debug("MapSensor -> User validation successful");
                    var data = dataFacade.MapSensor(sensorDetail);
                    return data;
                }
                else
                {
                    log.Debug("MapSensor -> User validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in MapSensor as :" + ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<QuestionModel> GetQuestionAnswers(int UserId)
        {
            try
            {
                log.Debug("GetQuestionAnswers called");
                if (accountService.ValidateUser(UserId))
                {
                    log.Debug("GetQuestionAnswers -> User validation successful");
                    List<QuestionModel> questionAnswers = new List<QuestionModel>();
                    var questionList = dataFacade.GetQuestions();

                    if (questionList != null && questionList.Count > 0)
                    {
                        questionList.All(question =>
                                    {
                                        questionAnswers.Add(Converter.QuestionEntityToModel(question));
                                        return true;
                                    });
                    }

                    return questionAnswers;
                }
                else
                {
                    log.Debug("GetQuestionAnswers -> User Validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetQuestionAnswers as: " + ex);
                throw new Exception(ex.Message, ex);
            }
        }

        //public List<SensorDataModel> GetSensorDetails(int UserId, SensorDataModel sensorData)
        //{
        //    try
        //    {
        //        log.Debug("GetSensorDetails called");
        //        if (accountService.ValidateUser(UserId))
        //        {
        //            log.Debug("GetSensorDetails -> User validation successful");
        //            List<SensorDataModel> sensorModel = new List<SensorDataModel>();
        //            var data = dataFacade.GetSensorDetails(sensorData);
        //            if (data == null)
        //            {
        //                log.Debug("GetSensorDetails -> No classrooms found");
        //                return sensorModel;
        //            }

        //            return data;
        //        }
        //        else
        //        {
        //            log.Debug("GetSensorDetails -> User Validation failed");
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Exception occurred in GetSensorDetails as: " + ex);
        //        throw new Exception(ex.Message, ex);
        //    }
        //}

        public List<FeedbackCountModel> GetFeedbackCount(int UserId, FeedbackCountModel answerDetails)
        {
            try
            {
                log.Debug("GetFeedbackCount called");
                if (accountService.ValidateUser(UserId))
                {
                    log.Debug("GetFeedbackCount -> User validation successful");
                    List<FeedbackCountModel> sensorModel = new List<FeedbackCountModel>();
                    var data = dataFacade.GetFeedbackCount(answerDetails);
                    if (data == null || data.Count < 1)
                    {
                        log.Debug("GetFeedbackCount -> No Feedback found");
                        return sensorModel;
                    }

                    //data.All(sensor =>
                    //{
                    //    sensorModel.Add(Converter.SensorMasterEntityToModel(sensor));
                    //    return true;
                    //});

                    return data;
                }
                else
                {
                    log.Debug("GetFeedbackCount -> User Validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetFeedbackCount as: " + ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<SensorModel> GetAllSensorsForClass(int UserId, SensorModel sensorData)
        {
            try
            {
                log.Debug("GetAllSensorsForClass called");
                if (accountService.ValidateUser(UserId))
                {
                    log.Debug("GetAllSensorsForClass -> User validation successful");
                    List<SensorModel> sensorModel = new List<SensorModel>();
                    var data = dataFacade.GetAllSensorsForClass(sensorData);
                    if (data == null || data.Count < 1)
                    {
                        log.Debug("GetAllSensorsForClass -> No sensors found");
                        return sensorModel;
                    }

                    //data.All(sensor =>
                    //{
                    //    sensorModel.Add(Converter.SensorMasterEntityToModel(sensor));
                    //    return true;
                    //});

                    return data;
                }
                else
                {
                    log.Debug("GetAllSensorsForClass -> User Validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetAllSensorsForClass as: " + ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<AlertModel> GetRecommendations(int UserId)
        {
            try
            {
                log.Debug("GetRecommendations called");
                if (accountService.ValidateUser(UserId))
                {
                    log.Debug("GetRecommendations -> User validation successful");
                    List<AlertModel> alertModelList = new List<AlertModel>();
                    var data = dataFacade.GetRecommendations();
                    if (data == null || data.Count < 1)
                    {
                        log.Debug("GetRecommendations -> No recommendations found");
                        return alertModelList;
                    }

                    //data.All(alert =>
                    //{
                    //    alertModelList.Add(Converter.AlertsEntityToModel(alert));
                    //    return true;
                    //});
                    //return alertModelList;

                    return data;
                }
                else
                {
                    log.Debug("GetRecommendations -> User Validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetRecommendations as: " + ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public InsightData GetInsightData(int UserId)
        {
            try
            {
                log.Debug("GetInsightData called");
                if (accountService.ValidateUser(UserId))
                {
                    log.Debug("GetInsightData -> User validation successful");
                    InsightData insightData = new InsightData();
                    var data = dataFacade.GetInsightData();
                    if (data == null)
                    {
                        log.Debug("GetInsightData -> No data found");
                        return insightData;
                    }
                    
                    return data;
                }
                else
                {
                    log.Debug("GetInsightData -> User Validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetInsightData as: " + ex);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}