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

        public List<MeterDetailsModel> GetMeterList(int userId)
        {
            try
            {
                log.Debug("GetMeterList called");
                if (accountService.ValidateUser(userId))
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

        public List<MeterMonthlyConsumptionModel> GetMeterMonthlyConsumption(int userId)
        {
            try
            {
                log.Debug("GetMeterMonthlyConsumption called");
                if (accountService.ValidateUser(userId))
                {
                    List<MeterDetails> meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        List<MeterMonthlyConsumptionModel> meterModelList = new List<MeterMonthlyConsumptionModel>();
                        foreach (var meterDataItem in meterData)
                        {
                            var data = dataFacade.GetMeterConsumption(meterDataItem);
                            MeterMonthlyConsumptionModel meterMonthlyConsumption = data == null ? new MeterMonthlyConsumptionModel { Powerscout = meterDataItem.PowerScout, Name = meterDataItem.Breaker_details } : Converter.MeterMonthlyEntityToModel(dataFacade.GetMeterConsumption(meterDataItem));
                            if (meterMonthlyConsumption != null)
                            {
                                meterMonthlyConsumption.Name = meterDataItem.Breaker_details;
                                meterModelList.Add(meterMonthlyConsumption);
                            }
                            else
                            {
                                meterModelList.Add(new MeterMonthlyConsumptionModel { Powerscout = meterDataItem.PowerScout, Name = meterDataItem.Breaker_details });
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

        public List<DailyConsumptionDetails> GetMeterDailyConsumption(int userId)
        {
            try
            {
                log.Debug("GetMeterDailyConsumption called");
                if (accountService.ValidateUser(userId))
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
                            else
                            {
                                meterModelList.Add(new DailyConsumptionDetails { PowerScout = meterDataItem.PowerScout, Breaker_details = meterDataItem.Breaker_details });
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

        public MeterURLKey GetPowerBIUrl(int userId, string meterSerial)
        {
            try
            {
                if (accountService.ValidateUser(userId))
                {
                    log.Debug("GetPowerBIUrl called");
                    string methodName = "GetURL_" + meterSerial;
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

        public PowerBIGeneralURL GetPowerBIGeneralURL(int userId)
        {
            try
            {
                log.Debug("GetPowerBIGeneralURL called");
                if (accountService.ValidateUser(userId))
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

        public List<MeterMonthWiseConsumption> GetMonthWiseConsumption(int userId, int year)
        {
            try
            {
                log.Debug("GetMonthWiseConsumption called");
                if (accountService.ValidateUser(userId))
                {
                    List<MeterDetails> meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        List<MeterMonthWiseConsumption> meterDataList = new List<MeterMonthWiseConsumption>();
                        foreach (var meterDataItem in meterData)
                        {
                            MeterMonthWiseConsumption meterMonthWiseConsumption = dataFacade.GetMeterMonthWiseConsumption(meterDataItem, year);
                            if (meterMonthWiseConsumption != null)
                            {
                                meterMonthWiseConsumption.PowerScout = meterDataItem.Serial;
                                meterMonthWiseConsumption.Name = meterDataItem.Breaker_details;
                                meterDataList.Add(meterMonthWiseConsumption);
                            }
                            else
                            {
                                meterDataList.Add(new MeterMonthWiseConsumption { PowerScout = meterDataItem.PowerScout, Name = meterDataItem.Breaker_details });
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

        public List<MeterMonthWiseConsumption> GetMonthWiseConsumptionForOffset(int userId, string month, int year, int offset)
        {
            try
            {
                log.Debug("GetMonthWiseConsumptionForOffset called");
                if (accountService.ValidateUser(userId))
                {
                    List<MeterDetails> meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        List<MeterMonthWiseConsumption> monthWiseDataList = new List<MeterMonthWiseConsumption>();
                        foreach (var meterDataItem in meterData)
                        {
                            List<MonthlyConsumptionDetails> monthlyDataList = dataFacade.GetMeterMonthWiseConsumptionForOffset(meterDataItem, month, year, offset);
                            if (monthlyDataList != null && monthlyDataList.Count > 0)
                            {
                                if (monthlyDataList.Count > offset)
                                {
                                    monthlyDataList.RemoveRange(offset, monthlyDataList.Count - offset);
                                }

                                monthWiseDataList.Add(Converter.MeterMonthWiseEntityToModel(monthlyDataList));
                            }
                            else
                            {
                                log.Debug("GetMonthwiseConsumptionForOffset -> No Data found for meter: " + meterDataItem.PowerScout);
                                monthWiseDataList.Add(new MeterMonthWiseConsumption { PowerScout = meterDataItem.PowerScout, Name = meterDataItem.Breaker_details });
                            }
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

        public List<MeterWeekWiseMonthlyConsumption> GetWeekWiseMonthlyConsumption(int userId, string month, int year)
        {
            try
            {
                log.Debug("GetWeekWiseConsumption called");
                if (accountService.ValidateUser(userId))
                {
                    List<MeterDetails> meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        List<MeterWeekWiseMonthlyConsumption> weekWiseConsumption = new List<MeterWeekWiseMonthlyConsumption>();
                        foreach (var meter in meterData)
                        {
                            List<DailyConsumptionDetails> dailyConsumptionListForMonth = dataFacade.GetDailyConsumptionForMonth(meter, month, year);
                            if (dailyConsumptionListForMonth != null && dailyConsumptionListForMonth.Count > 0)
                            {
                                var meterWeekWiseConsumption = GetWeekWiseConsumptionFromMonthly(dailyConsumptionListForMonth);
                                weekWiseConsumption.Add(meterWeekWiseConsumption);
                            }
                            else
                            {
                                log.Debug("GetWeekWiseMonthlyConsumption -> No data found for meter: " + meter.PowerScout);
                                weekWiseConsumption.Add(new MeterWeekWiseMonthlyConsumption { PowerScout = meter.PowerScout, Name = meter.Breaker_details });
                            }
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

        public List<MeterWeekWiseMonthlyConsumption> GetWeekWiseMonthlyConsumptionForOffset(int userId, string month, int year, int offset)
        {
            try
            {
                log.Debug("GetWeekWiseMonthlyConsumptionForOffset called");
                if (accountService.ValidateUser(userId))
                {
                    List<MeterDetails> meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        List<MeterWeekWiseMonthlyConsumption> weekWiseConsumption = new List<MeterWeekWiseMonthlyConsumption>();
                        foreach (var meter in meterData)
                        {
                            MeterWeekWiseMonthlyConsumption meterWeekWiseConsumption = new MeterWeekWiseMonthlyConsumption();
                            DateTime monthDate = DateTime.Parse("01/" + month + "/" + year);
                            string activeMonth = month;
                            int activeYear = year;
                            while (meterWeekWiseConsumption.WeekWiseConsumption.Count < offset)
                            {
                                List<DailyConsumptionDetails> dailyConsumptionListForMonth = dataFacade.GetDailyConsumptionForMonth(meter, activeMonth, activeYear);
                                if (dailyConsumptionListForMonth == null || dailyConsumptionListForMonth.Count < 1)
                                {
                                    log.Debug("GetWeekWiseMonthlyConsumptionForOffset -> No data found for meter: " + meter.PowerScout);
                                    weekWiseConsumption.Add(new MeterWeekWiseMonthlyConsumption { PowerScout = meter.PowerScout, Name = meter.Breaker_details });
                                    break;
                                }

                                var weekWiseList = GetWeekWiseConsumptionFromMonthly(dailyConsumptionListForMonth).WeekWiseConsumption;
                                if (weekWiseList.Count + meterWeekWiseConsumption.WeekWiseConsumption.Count > offset)
                                {
                                    weekWiseList.Reverse();
                                    meterWeekWiseConsumption.WeekWiseConsumption.AddRange(weekWiseList.Take(offset - meterWeekWiseConsumption.WeekWiseConsumption.Count));
                                }
                                else
                                {
                                    meterWeekWiseConsumption.WeekWiseConsumption.AddRange(weekWiseList);
                                }

                                activeMonth = monthDate.AddMonths(-1).ToString("MMM");
                                activeYear = monthDate.AddMonths(-1).Year;
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
                throw new Exception(ex.Message, ex);
            }
        }

        public List<MeterDayWiseMonthlyConsumption> GetDayWiseMonthlyConsumption(int userId, string month, int year)
        {
            try
            {
                log.Debug("GetDayWiseMonthlyConsumption called");
                if (accountService.ValidateUser(userId))
                {
                    var meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        List<MeterDayWiseMonthlyConsumption> dayWiseConsumptionList = new List<MeterDayWiseMonthlyConsumption>();
                        foreach (var meter in meterData)
                        {
                            List<DailyConsumptionDetails> meterDailyConsumption = dataFacade.GetDailyConsumptionForMonth(meter, month, year);
                            if (meterDailyConsumption != null && meterDailyConsumption.Count > 0)
                            {
                                // Conversion from Entity to Model
                                var dayWiseConsumption = Converter.MeterDayWiseEntityToModel(meterDailyConsumption);
                                if (dayWiseConsumption != null)
                                {
                                    dayWiseConsumptionList.Add(dayWiseConsumption);
                                }
                            }
                            else
                            {
                                log.Debug("GetDayWiseMonthlyConsumption -> No Data found");
                                dayWiseConsumptionList.Add(new MeterDayWiseMonthlyConsumption { PowerScout = meter.PowerScout, Name = meter.Breaker_details });
                            }
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

        public List<MeterDayWiseMonthlyConsumptionPrediction> GetDayWiseCurrentMonthPrediction(int userId, string month, int year)
        {
            try
            {
                log.Debug("GetDayWiseCurrentMonthPrediction called");
                if (accountService.ValidateUser(userId))
                {
                    var meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        List<MeterDayWiseMonthlyConsumptionPrediction> dayWisePredictionList = new List<MeterDayWiseMonthlyConsumptionPrediction>();
                        foreach (var meter in meterData)
                        {
                            var dailyPredictionList = dataFacade.GetDayWiseNextMonthPrediction(meter, month, year);
                            if (dailyPredictionList != null && dailyPredictionList.Count > 0)
                            {
                                dayWisePredictionList.Add(Converter.MeterDayWiseMonthlyPredictionEntityToModel(dailyPredictionList));
                            }
                            else
                            {
                                log.Debug("GetDayWiseCurrentMonthPrediction -> No Data found");
                                dayWisePredictionList.Add(new MeterDayWiseMonthlyConsumptionPrediction { PowerScout = meter.PowerScout, Name = meter.Breaker_details });
                            }
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
                throw new Exception(ex.Message, ex);
            }
        }

        public List<MeterDayWiseMonthlyConsumptionPrediction> GetDayWiseNextMonthPrediction(int userId, string month, int year)
        {
            try
            {
                log.Debug("GetDayWiseNextMonthPrediction called");
                if (accountService.ValidateUser(userId))
                {
                    var meterData = dataFacade.GetMeters();
                    if (meterData != null && meterData.Count > 0)
                    {
                        List<MeterDayWiseMonthlyConsumptionPrediction> dayWisePredictionList = new List<MeterDayWiseMonthlyConsumptionPrediction>();
                        DateTime monthDate;
                        DateTime.TryParse("01-" + month + "-" + year, out monthDate);
                        monthDate = monthDate.AddMonths(1);
                        month = monthDate.ToString("MMM");
                        year = monthDate.Year;
                        foreach (var meter in meterData)
                        {
                            var dailyPredictionList = dataFacade.GetDayWiseNextMonthPrediction(meter, month, year);
                            if (dailyPredictionList != null && dailyPredictionList.Count > 0)
                            {
                                dayWisePredictionList.Add(Converter.MeterDayWiseMonthlyPredictionEntityToModel(dailyPredictionList));
                            }
                            else
                            {
                                dayWisePredictionList.Add(new MeterDayWiseMonthlyConsumptionPrediction { PowerScout = meter.PowerScout, Name = meter.Breaker_details });
                            }
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
                throw new Exception(ex.Message, ex);
            }
        }

        public List<AlertModel> GetAllAlerts(int userId)
        {
            try
            {
                log.Debug("GetAllAlerts called");
                if (accountService.ValidateUser(userId))
                {
                    log.Debug("GetAllAlerts -> User validation successful");
                    var data = dataFacade.GetAllAlerts();
                    if (data != null)
                    {
                        return data;
                    }
                    else
                    {
                        return new List<AlertModel>();
                    }
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

        public AlertDetailsModel GetAlertDetails(int userId, int logId)
        {
            try
            {
                log.Debug("GetAlertDetails called");
                if (accountService.ValidateUser(userId))
                {
                    var alertDetails = dataFacade.GetAlertDetails(logId);
                    if (alertDetails != null)
                    {
                        return alertDetails;
                    }
                    else
                    {
                        return new AlertDetailsModel();
                    }
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

        public List<ClassroomModel> GetAllClassrooms(int userId)
        {
            try
            {
                log.Debug("GetAllClassrooms called");
                if (accountService.ValidateUser(userId))
                {
                    log.Debug("GetAllClassrooms -> User validation successful");
                    List<ClassroomModel> classroomModel = new List<ClassroomModel>();
                    var data = dataFacade.GetAllClassrooms();
                    if (data != null && data.Count > 0)
                    {
                        data.All(classroom =>
                        {
                            classroomModel.Add(Converter.ClassroomEntityToModel(classroom));
                            return true;
                        });
                    }

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

        public ResponseModel AcknowledgeAlert(int userId, AlertModel alertDetail)
        {
            try
            {
                log.Debug("Acknowledge alert called");
                if (accountService.ValidateUser(userId))
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

        public ResponseModel StoreFeedback(int userId, Feedback feedbackdetail)
        {
            try
            {
                log.Debug("StoreFeedback called");
                if (accountService.ValidateUser(userId))
                {
                    log.Debug("StoreFeedback -> User validation successful");
                    var data = dataFacade.StoreFeedback(userId, feedbackdetail);
                    var feedbackCount = dataFacade.GetFeedbackCount(new FeedbackCountModel { ClassId = (int)feedbackdetail.ClassID });
                    var exceptionData = feedbackCount.Where(feedback => feedback.AnswerCount > feedback.Threshold && feedback.AnswerId == feedbackdetail.AnswerID).ToList();
                    if (exceptionData != null && exceptionData.Count > 0)
                    {
                        foreach (var exception in exceptionData)
                        {
                            var title = "Temperature Alert";
                            var message = "Students are feeling " + exception.AnswerDesc + " in the class " + exception.ClassName + ". Take appropriate measures.";
                            ServiceUtil.SendNotification(title, message);
                            dataFacade.AddAlert(new Alerts { Sensor_Id = 0, Sensor_Log_Id = 0, Alert_Type = title, Description = message, Is_Acknowledged = 0, Timestamp = DateTime.UtcNow });
                        }
                    }

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

        public List<FeedbackModel> GetAllFeedback(int userId)
        {
            try
            {
                log.Debug("GetAllFeedback called");
                if (accountService.ValidateUser(userId))
                {
                    log.Debug("GetAllFeedback -> User validation successful");
                    List<FeedbackModel> feedbackModel = new List<FeedbackModel>();
                    var data = dataFacade.GetAllFeedback();
                    if (data != null && data.Count > 0)
                    {
                        data.All(feedback =>
                        {
                            feedbackModel.Add(Converter.FeedbackEntityToModel(feedback));
                            return true;
                        });
                    }

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

        public ResponseModel DeleteFeedback(int userId, FeedbackModel feedbackdetail)
        {
            try
            {
                log.Debug("DeleteFeedback called");
                if (accountService.ValidateUser(userId))
                {
                    log.Debug("DeleteFeedback -> User validation successful");
                    var data = dataFacade.DeleteFeedback(feedbackdetail);
                    return data;
                }
                else
                {
                    log.Debug("DeleteFeedback -> User validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in DeleteFeedback as :" + ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public ResponseModel UpdateFeedback(int userId, FeedbackModel feedbackdetail)
        {
            try
            {
                log.Debug("UpdateFeedback called");
                if (accountService.ValidateUser(userId))
                {
                    log.Debug("UpdateFeedback -> User validation successful");
                    var data = dataFacade.UpdateFeedback(userId, feedbackdetail);
                    return data;
                }
                else
                {
                    log.Debug("UpdateFeedback -> User validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in UpdateFeedback as :" + ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public List<SensorModel> GetAllSensors(int userId)
        {
            try
            {
                log.Debug("GetAllSensors called");
                if (accountService.ValidateUser(userId))
                {
                    log.Debug("GetAllSensors -> User validation successful");
                    var data = dataFacade.GetAllSensors();
                    if (data == null || data.Count < 1)
                    {
                        return new List<SensorModel>();
                    }

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

        public ResponseModel MapSensor(int userId, SensorModel sensorDetail)
        {
            try
            {
                log.Debug("MapSensor called");
                if (accountService.ValidateUser(userId))
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

        public List<QuestionModel> GetQuestionAnswers(int userId)
        {
            try
            {
                log.Debug("GetQuestionAnswers called");
                if (accountService.ValidateUser(userId))
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

        public List<AnomalyInfoModel> GetAnomalyDetails(int userId, string timeStamp)
        {
            try
            {
                log.Debug("GetAnomalyDetails called");
                if (accountService.ValidateUser(userId))
                {
                    var anomalyDetails = dataFacade.GetAnomalyDetails(timeStamp);
                    List<AnomalyInfoModel> anomalyInfoModel = new List<AnomalyInfoModel>();
                    if (anomalyDetails != null && anomalyDetails.Count > 0)
                    {
                        anomalyDetails.All(anomaly =>
                        {
                            anomalyInfoModel.Add(Converter.AnomalyDetailsEntityToModel(anomaly));
                            return true;
                        });
                    }

                    return anomalyInfoModel;
                }
                else
                {
                    log.Debug("GetAnomalyDetails -> User Validation failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Exception occured in GetAlertDetails as: " + ex);
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

        public List<FeedbackCountModel> GetFeedbackCount(int userId, FeedbackCountModel answerDetails)
        {
            try
            {
                log.Debug("GetFeedbackCount called");
                if (accountService.ValidateUser(userId))
                {
                    log.Debug("GetFeedbackCount -> User validation successful");
                    var data = dataFacade.GetFeedbackCount(answerDetails);
                    if (data == null || data.Count < 1)
                    {
                        return new List<FeedbackCountModel>();
                    }

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

        public List<SensorModel> GetAllSensorsForClass(int userId, SensorModel sensorData)
        {
            try
            {
                log.Debug("GetAllSensorsForClass called");
                if (accountService.ValidateUser(userId))
                {
                    log.Debug("GetAllSensorsForClass -> User validation successful");
                    var data = dataFacade.GetAllSensorsForClass(sensorData);
                    if (data == null || data.Count < 1)
                    {
                        return new List<SensorModel>();
                    }

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

        public List<AlertModel> GetRecommendations(int userId)
        {
            try
            {
                log.Debug("GetRecommendations called");
                if (accountService.ValidateUser(userId))
                {
                    log.Debug("GetRecommendations -> User validation successful");
                    var data = dataFacade.GetRecommendations();
                    if (data == null || data.Count < 1)
                    {
                        return new List<AlertModel>();
                    }

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

        public InsightData GetInsightData(int userId)
        {
            try
            {
                log.Debug("GetInsightData called");
                if (accountService.ValidateUser(userId))
                {
                    log.Debug("GetInsightData -> User validation successful");
                    InsightData insightData = new InsightData();
                    var data = dataFacade.GetInsightData();
                    if (data == null)
                    {
                        return new InsightData();
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

        public ResponseModel ResetFeedback()
        {
            try
            {
                log.Debug("ResetFeedback called");
                var data = dataFacade.ResetFeedback();
                return data;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in ResetFeedback as: " + ex);
                throw new Exception(ex.Message, ex);
            }
        }

        public ResponseModel ResetSensors()
        {
            try
            {
                log.Debug("ResetSensors called");
                var data = dataFacade.ResetSensors();
                return data;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in ResetSensors as: " + ex);
                throw new Exception(ex.Message, ex);
            }
        }
    }
}