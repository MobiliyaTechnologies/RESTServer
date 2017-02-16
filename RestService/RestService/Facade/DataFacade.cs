using RestService.Entities;
using RestService.Models;
using RestService.Utilities;
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
            var meterConsumption = (from data in dbEntity.MonthlyConsumptionDetails where meter.Serial.Equals(data.PowerScout) && currentMonth.Equals(data.Month) select data).FirstOrDefault();
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
            var meterDataList = (from data in dbEntity.MonthlyConsumptionDetails where data.PowerScout.Equals(meterData.Serial) && (data.Year).Equals(Year.ToString()) select data).ToList();
            MeterMonthWiseConsumption meterConsumption = new MeterMonthWiseConsumption();
            meterDataList.All(meterDataItem =>
            {
                switch (meterDataItem.Month.ToLower())
                {
                    case "jan":
                        meterConsumption.MonthWiseConsumption.Jan = meterConsumption.MonthWiseConsumption.Jan + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "feb":
                        meterConsumption.MonthWiseConsumption.Feb = meterConsumption.MonthWiseConsumption.Feb + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "mar":
                        meterConsumption.MonthWiseConsumption.Mar = meterConsumption.MonthWiseConsumption.Mar + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "apr":
                        meterConsumption.MonthWiseConsumption.Apr = meterConsumption.MonthWiseConsumption.Apr + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "may":
                        meterConsumption.MonthWiseConsumption.May = meterConsumption.MonthWiseConsumption.May + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "jun":
                        meterConsumption.MonthWiseConsumption.Jun = meterConsumption.MonthWiseConsumption.Jun + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "jul":
                        meterConsumption.MonthWiseConsumption.Jul = meterConsumption.MonthWiseConsumption.Jul + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "aug":
                        meterConsumption.MonthWiseConsumption.Aug = meterConsumption.MonthWiseConsumption.Aug + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "sep":
                        meterConsumption.MonthWiseConsumption.Sep = meterConsumption.MonthWiseConsumption.Sep + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "oct":
                        meterConsumption.MonthWiseConsumption.Oct = meterConsumption.MonthWiseConsumption.Oct + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "nov":
                        meterConsumption.MonthWiseConsumption.Nov = meterConsumption.MonthWiseConsumption.Nov + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "dec":
                        meterConsumption.MonthWiseConsumption.Dec = meterConsumption.MonthWiseConsumption.Dec + (double)meterDataItem.Monthly_KWH_System;
                        break;

                }
                return true;
            });

            return meterConsumption;
        }

        public List<MonthlyConsumptionDetails> GetMeterMonthWiseConsumptionForOffset(MeterDetails meterData, string Month, int Year, int Offset)
        {
            DateTime endDate;
            DateTime.TryParse("01-" + Month + "-" + Year, out endDate);
            endDate = endDate.AddMonths(1).AddDays(-1);
            DateTime startDate = endDate.AddMonths(-Offset);
            var meterDataList = (from data in dbEntity.MonthlyConsumptionDetails where data.PowerScout.Equals(meterData.PowerScout) && (data.Year.Equals(startDate.Year.ToString()) || data.Year.Equals(endDate.Year.ToString())) orderby data.Id descending select data).ToList().Where(data => ((DateTime)data.Timestamp).Date > startDate.Date && ((DateTime)data.Timestamp).Date <= endDate.Date).ToList();
            return meterDataList;
        }

        public List<DailyConsumptionDetails> GetDailyConsumptionForMonth(MeterDetails meter, string Month, int Year)
        {
            DateTime monthDate;
            DateTime.TryParse("01-" + Month + "-" + Year, out monthDate);
            var DailyConsumptionList = (from data in dbEntity.DailyConsumptionDetails where meter.Serial.Equals(data.PowerScout) && monthDate.Month == ((DateTime)data.Timestamp).Month && monthDate.Year == ((DateTime)data.Timestamp).Year select data).ToList();
            return DailyConsumptionList;
        }

        public List<DailyConsumptionPrediction> GetDayWiseNextMonthPrediction(MeterDetails meter, string Month, int Year)
        {
            DateTime monthDate;
            DateTime.TryParse("01-" + Month + "-" + Year, out monthDate);
            var dailyConsumptionPrediction = (from data in dbEntity.DailyConsumptionPrediction where meter.Serial.Equals(data.PowerScout) && monthDate.Month == ((DateTime)data.Timestamp).Month && monthDate.Year == ((DateTime)data.Timestamp).Year select data).ToList();
            return dailyConsumptionPrediction;
        }

        public List<AlertModel> GetAllAlerts()
        {
            //var data = (from alerts in dbEntity.Alerts orderby alerts.Timestamp descending select alerts).ToList();
            //return data;
            var data = (from alerts in dbEntity.Alerts
                        join sensorData in dbEntity.SensorMaster on alerts.Sensor_Id equals sensorData.Sensor_Id
                        join classData in dbEntity.ClassroomDetails on sensorData.Class_Id equals classData.Class_Id
                        orderby alerts.Timestamp descending
                        select new AlertModel
                        { Alert_Id = alerts.Id, Acknowledged_By = alerts.Acknowledged_By == null ? "" : alerts.Acknowledged_By, Acknowledged_Timestamp = alerts.Acknowledged_Timestamp == null ? new DateTime() : (DateTime)alerts.Acknowledged_Timestamp, Alert_Desc = alerts.Description, Alert_Type = alerts.Alert_Type, Class_Desc = classData.Class_Desc, Class_Id = classData.Class_Id, Class_Name = classData.Class_Name, Is_Acknowledged = alerts.Is_Acknowledged == 0 ? false : true, Sensor_Id = alerts.Sensor_Id, Sensor_Log_Id = alerts.Sensor_Log_Id, Timestamp = (DateTime)alerts.Timestamp }).ToList();
            return data;
        }

        public AlertDetailsModel GetAlertDetails(int sensorLogId)
        {
            //var alertDetails = (from data in dbEntity.SensorData where data.Sensor_Log_Id == sensorLogId select data).FirstOrDefault();
            //return alertDetails;
            
            var alertDetails = (from data in dbEntity.SensorLiveData
                                join sensorData in dbEntity.SensorMaster on data.Sensor_Id equals sensorData.Sensor_Id
                                join classData in dbEntity.ClassroomDetails on sensorData.Class_Id equals classData.Class_Id
                                where data.Sensor_Log_Id == sensorLogId
                                select new AlertDetailsModel
                                { Sensor_Id = (int)data.Sensor_Id, Class_Id = classData.Class_Id, Class_Name = classData.Class_Name, Class_Desc = classData.Class_Desc, Humidity = Math.Round((double)data.Humidity,2), Light_Intensity = Math.Round((double)data.Brightness,2), Temperature = Math.Round((double)data.Temperature,2), Timestamp = (DateTime)data.Timestamp }).FirstOrDefault();
            return alertDetails;
        }

        public List<ClassroomDetails> GetAllClassrooms()
        {
            var classroomList = (from data in dbEntity.ClassroomDetails select data).ToList();
            return classroomList;
        }

        public ResponseModel AcknowledgeAlert(AlertModel alertDetail)
        {
            var data = (from alert in dbEntity.Alerts where alert.Id == alertDetail.Alert_Id select alert).FirstOrDefault();
            if (data == null)
            {
                return new ResponseModel { Message = "Invalid Alert", Status_Code = (int)Constants.StatusCode.Error };
            }
            else
            {
                data.Acknowledged_By = alertDetail.Acknowledged_By;
                data.Is_Acknowledged = 1;
                data.Acknowledged_Timestamp = DateTime.UtcNow;
                dbEntity.SaveChanges();
                return new ResponseModel { Message = "Acknowledgement successful", Status_Code = (int)Constants.StatusCode.Ok };
            }
        }

        public ResponseModel StoreFeedback(int UserId, Feedback feedbackDetail)
        {
            var feedbackdesc = feedbackDetail.FeedbackDesc == null ? "" : feedbackDetail.FeedbackDesc;
            feedbackDetail.CreatedBy = UserId;
            feedbackDetail.ModifiedBy = UserId;
            feedbackDetail.CreatedOn = feedbackDetail.ModiifiedOn = DateTime.UtcNow;
            feedbackDetail.FeedbackDesc = feedbackdesc;           
            dbEntity.Feedback.Add(feedbackDetail);
            dbEntity.SaveChanges();

            //Add reward points
            var data = (from user in dbEntity.User where user.Id == UserId select user).FirstOrDefault();
            data.RewardPoints += 10;
            dbEntity.SaveChanges();
            return new ResponseModel { Message = "Feedback successfully recorded", Status_Code = (int)Constants.StatusCode.Ok };
        }

        public List<Feedback> GetAllFeedback()
        {
            var feedbackList = (from data in dbEntity.Feedback select data).ToList();
            return feedbackList;
        }

        public ResponseModel FeedbackDelete(FeedbackModel feedbackDetail)
        {
            var data = (from feedback in dbEntity.Feedback where feedback.FeedbackID == feedbackDetail.FeedbackId select feedback).FirstOrDefault();
            if (data == null)
            {
                return new ResponseModel { Message = "Invalid Feedback", Status_Code = (int)Constants.StatusCode.Error };
            }
            else
            {
                dbEntity.Feedback.Remove(data);
                dbEntity.SaveChanges();
                return new ResponseModel { Message = "Feedback deleted", Status_Code = (int)Constants.StatusCode.Ok };
            }
        }

        public ResponseModel FeedbackUpdate(int UserId, FeedbackModel feedbackDetail)
        {
            var data = (from feedback in dbEntity.Feedback where feedback.FeedbackID == feedbackDetail.FeedbackId select feedback).FirstOrDefault();
            if (data == null)
            {
                return new ResponseModel { Message = "Invalid Feedback", Status_Code = (int)Constants.StatusCode.Error };
            }
            else
            {
                if (data.AnswerID != feedbackDetail.AnswerID && feedbackDetail.AnswerID > 0)
                {
                    data.AnswerID = feedbackDetail.AnswerID;
                }
                if (data.FeedbackDesc != feedbackDetail.FeedbackDesc && !string.IsNullOrEmpty(feedbackDetail.FeedbackDesc))
                {
                    data.FeedbackDesc = feedbackDetail.FeedbackDesc;
                }
                data.ModifiedBy = UserId;
                data.ModiifiedOn = DateTime.UtcNow;
                dbEntity.SaveChanges();
                return new ResponseModel { Message = "Feedback Updated", Status_Code = (int)Constants.StatusCode.Ok };
            }
        }
    }
}