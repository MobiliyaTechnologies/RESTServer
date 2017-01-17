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
    }
}