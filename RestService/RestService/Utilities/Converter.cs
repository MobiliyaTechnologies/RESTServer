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
            //response.Role_Id = user.UserRole;
            return response;
        }

        public static MeterDataModel MeterEntityToModel(MeterDetails meterData)
        {
            MeterDataModel meterModel = new MeterDataModel();
            meterModel.Name = meterData.Name;
            meterModel.Serial = meterData.Serial;
            meterModel.Latitude = (double)meterData.Latitude;
            meterModel.Longitude = (double)meterData.Longitude;
            meterModel.Altitude = (double)meterData.Altitude;
            return meterModel;
        }
    }
}