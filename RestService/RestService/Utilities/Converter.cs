using RestService.Entities;
using RestService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Utilities
{
    public class Converter
    {
        public static User UserModelToUserEntity(UserDataModel userDataModel)
        {
            User user = new User();
            UserRole userRole = new UserRole();
            user.First_Name = userDataModel.First_Name;
            user.Last_Name = userDataModel.Last_Name;
            user.Email_Id = userDataModel.Email;
            user.Password = userDataModel.Password;
            return user;
        }

        public static User UserCredentialsToUserEntity(UserCredentials userCredentials)
        {
            User user = new User();
            UserRole userRole = new UserRole();
            user.Email_Id = userCredentials.Email;
            user.Password = userCredentials.Password;
            return user;
        }

        public static UserRole UserModelToUserRoleEntity(UserDataModel userDataModel)
        {
            UserRole userRole = new UserRole();
            userRole.Role_Id = userDataModel.Role_Id;
            return userRole;
        }
        public static ResponseUserModel UserToResponseUserModel(User user)
        {
            ResponseUserModel response = new ResponseUserModel();
            response.First_Name = user.First_Name;
            response.Last_Name = user.Last_Name;
            response.Email = user.Email_Id;
            response.User_Id = user.Id;
            response.Avatar = user.Avatar;
            //response.Role_Id = user.UserRole;
            return response;
        }

        public static MeterDetailsModel MeterDetailsEntityToModel(MeterDetails meterData)
        {
            MeterDetailsModel meterModel = new MeterDetailsModel();
            meterModel.Id = meterData.Id;
            meterModel.PowerScout = meterData.PowerScout;
            meterModel.Altitude = (double)meterData.Altitude;
            meterModel.Description = meterData.Description;
            meterModel.Latitude = (double)meterData.Latitude;
            meterModel.Longitude = (double)meterData.Longitude;
            meterModel.Name = meterData.Breaker_details;
            meterModel.Serial = meterData.Serial;
            return meterModel;
        }

        public static MeterGeneralDataModel MeterEntityToModel(MeterDetails meterData)
        {
            MeterGeneralDataModel meterModel = new MeterGeneralDataModel();
            meterModel.Name = meterData.Breaker_details;
            meterModel.Serial = meterData.Serial;
            meterModel.Latitude = (double)meterData.Latitude;
            meterModel.Longitude = (double)meterData.Longitude;
            meterModel.Altitude = (double)meterData.Altitude;
            return meterModel;
        }

        public static MeterMonthlyConsumptionModel MeterMonthlyEntityToModel(MonthlyConsumptionDetails meterData)
        {
            MeterMonthlyConsumptionModel meterModel = new MeterMonthlyConsumptionModel();
            meterModel.Id = meterData.Id;
            meterModel.Month = meterData.Month;
            meterModel.Year = meterData.Year;
            meterModel.Powerscout = meterData.PowerScout;
            meterModel.X = (double)meterData.X;
            meterModel.Y = (double)meterData.Y;
            meterModel.Ligne = meterData.Ligne;
            meterModel.Monthly_KWH_Consumption = (double)meterData.Monthly_KWH_System;
            meterModel.Monthly_Electric_Cost = (double)meterData.Monthly_electric_cost;
            meterModel.Current_Month = (DateTime)meterData.current_month;
            meterModel.Last_Month = (DateTime)meterData.last_month;
            return meterModel;
        }

        public static MeterMonthWiseConsumption MeterMonthWiseEntityToModel(List<MonthlyConsumptionDetails> monthlyConsumptionList)
        {
            MeterMonthWiseConsumption meterMonthWiseConsumption = new MeterMonthWiseConsumption();
            monthlyConsumptionList.All(meterDataItem =>
            {
                switch (meterDataItem.Month.ToLower())
                {
                    case "jan":
                        meterMonthWiseConsumption.MonthWiseConsumption.Jan = meterMonthWiseConsumption.MonthWiseConsumption.Jan + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "feb":
                        meterMonthWiseConsumption.MonthWiseConsumption.Feb = meterMonthWiseConsumption.MonthWiseConsumption.Feb + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "mar":
                        meterMonthWiseConsumption.MonthWiseConsumption.Mar = meterMonthWiseConsumption.MonthWiseConsumption.Mar + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "apr":
                        meterMonthWiseConsumption.MonthWiseConsumption.Apr = meterMonthWiseConsumption.MonthWiseConsumption.Apr + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "may":
                        meterMonthWiseConsumption.MonthWiseConsumption.May = meterMonthWiseConsumption.MonthWiseConsumption.May + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "jun":
                        meterMonthWiseConsumption.MonthWiseConsumption.Jun = meterMonthWiseConsumption.MonthWiseConsumption.Jun + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "jul":
                        meterMonthWiseConsumption.MonthWiseConsumption.Jul = meterMonthWiseConsumption.MonthWiseConsumption.Jul + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "aug":
                        meterMonthWiseConsumption.MonthWiseConsumption.Aug = meterMonthWiseConsumption.MonthWiseConsumption.Aug + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "sep":
                        meterMonthWiseConsumption.MonthWiseConsumption.Sep = meterMonthWiseConsumption.MonthWiseConsumption.Sep + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "oct":
                        meterMonthWiseConsumption.MonthWiseConsumption.Oct = meterMonthWiseConsumption.MonthWiseConsumption.Oct + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "nov":
                        meterMonthWiseConsumption.MonthWiseConsumption.Nov = meterMonthWiseConsumption.MonthWiseConsumption.Nov + (double)meterDataItem.Monthly_KWH_System;
                        break;

                    case "dec":
                        meterMonthWiseConsumption.MonthWiseConsumption.Dec = meterMonthWiseConsumption.MonthWiseConsumption.Dec + (double)meterDataItem.Monthly_KWH_System;
                        break;

                }
                return true;
            });
            meterMonthWiseConsumption.PowerScout = monthlyConsumptionList.FirstOrDefault().PowerScout;
            meterMonthWiseConsumption.Name = monthlyConsumptionList.FirstOrDefault().Breaker_details;
            return meterMonthWiseConsumption;
        }

        public static MeterDayWiseMonthlyConsumption MeterDayWiseEntityToModel(List<DailyConsumptionDetails> dailyConsumptionList)
        {
            MeterDayWiseMonthlyConsumption dayWiseConsumption = new MeterDayWiseMonthlyConsumption();
            dayWiseConsumption.PowerScout = dailyConsumptionList.FirstOrDefault().PowerScout;
            dailyConsumptionList.All(data =>
            {
                dayWiseConsumption.DayWiseConsumption.Add((double)data.Daily_KWH_System);
                return true;
            });
            return dayWiseConsumption;
        }

        public static MeterDayWiseMonthlyConsumptionPrediction MeterDayWiseMonthlyPredictionEntityToModel(List<DailyConsumptionPrediction> dailyConsumptionPredictionList)
        {
            MeterDayWiseMonthlyConsumptionPrediction dayWiseConsumptionPrediction = new MeterDayWiseMonthlyConsumptionPrediction();
            dayWiseConsumptionPrediction.PowerScout = dailyConsumptionPredictionList.FirstOrDefault().PowerScout;
            dailyConsumptionPredictionList.All(data =>
            {
                dayWiseConsumptionPrediction.DayWiseConsumptionPrediction.Add((double)data.Daily_Predicted_KWH_System);
                return true;
            });
            return dayWiseConsumptionPrediction;
        }

        public static AlertModel AlertsEntityToModel(Alerts alertEntity)
        {
            AlertModel alertModel = new AlertModel();
            alertModel.Sensor_Id = alertEntity.Sensor_Id;
            alertModel.Sensor_Log_Id = alertEntity.Sensor_Log_Id;
            alertModel.Is_Acknowledged = alertEntity.Is_Acknowledged == 0 ? false : true;
            if (alertModel.Is_Acknowledged)
            {
                alertModel.Acknowledged_By = alertEntity.Acknowledged_By;
                alertModel.Acknowledged_Timestamp = (DateTime)alertEntity.Acknowledged_Timestamp; 
            }
            alertModel.Alert_Type = alertEntity.Alert_Type;
            alertModel.Alert_Desc = alertEntity.Description;
            alertModel.Timestamp = (DateTime)alertEntity.Timestamp;
            return alertModel;
        }

        public static AlertDetailsModel AlertDetailsEntityToModel(SensorData alertDetailsEntity)
        {
            AlertDetailsModel alertDetailsModel = new AlertDetailsModel();
            alertDetailsModel.Battery_Remaining = (double)alertDetailsEntity.Battery_Remaining;
            alertDetailsModel.Class_Id = alertDetailsEntity.Class_Id.ToString();
            alertDetailsModel.Humidity = (double)alertDetailsEntity.Humidity;
            alertDetailsModel.Is_Light_ON = alertDetailsEntity.Is_Light_ON == null || alertDetailsEntity.Is_Light_ON == 0 ? false : true;
            alertDetailsModel.Last_Updated = (DateTime)alertDetailsEntity.Last_Updated;
            alertDetailsModel.Light_Intensity = (double)alertDetailsEntity.Light_Intensity;
            alertDetailsModel.Sensor_Id = alertDetailsEntity.Sensor_Id;
            alertDetailsModel.Temperature = (double)alertDetailsEntity.Temperature;
            alertDetailsModel.Timestamp = (DateTime)alertDetailsEntity.Timestamp;
            return alertDetailsModel;
        }

        public static ClassroomModel ClassroomEntityToModel(ClassroomDetails classroomDetails)
        {
            ClassroomModel classroomModel = new ClassroomModel();
            classroomModel.ClassDescription = classroomDetails.Class_Description;
            classroomModel.ClassId = classroomDetails.Class_Id;
            classroomModel.SensorId = (int)classroomDetails.Sensor_Id;
            classroomModel.Building = classroomDetails.Building;
            classroomModel.Breaker_details = classroomDetails.Breaker_details;
            return classroomModel;
        }
    }
}