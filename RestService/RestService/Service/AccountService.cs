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
    public class AccountService
    {
        private Validator validator;
        private UserFacade userFacade;
        private ResponseModel response;  
        public AccountService()
        {
            validator = new Validator();
            userFacade = new UserFacade();
            response = new ResponseModel();
        }

        public ResponseModel RegisterUser(UserDataModel userDetails)
        {
            if(validator.SignUpValidator(userDetails))
            {
                //Validation Successful
                User userInfo = Converter.UserModelToUserEntity(userDetails); 
                response = userFacade.RegisterUser(userInfo);
                if(response.Status_Code == Convert.ToInt16(Constants.StatusCode.Ok))
                {
                    UserRole userRoleInfo = Converter.UserModelToUserRoleEntity(userDetails);
                    response = userFacade.AddUserRoleMapping(userInfo, userRoleInfo);
                }
                return response;
            }
            else
            {
                //validation Unsuccessful
                response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                response.Message = "Validation Faliure";
                return response;
            }
        }

        public ResponseModel ResetPassword(UserDataModel userDetails)
        {
            if(validator.ResetPasswordValidator(userDetails))
            {
                //validation successful
                response = userFacade.CheckEmail(userDetails);
                if(response.Status_Code == Convert.ToInt16(Constants.StatusCode.Ok))
                {
                    //Send Email
                    response = ServiceUtil.SendEmail(userDetails.Email);
                }
                else
                {
                    response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                    response.Message = "Email not registered";
                }
                return response;
            }
            else
            {
                //validation unsuccessful
                response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                response.Message = "Validation Faliure";
                return response;
            }
        }

        public ResponseUserModel SignInUser(UserCredentials userCredentials)
        {
            ResponseUserModel response;
            if (validator.SignInValidator(userCredentials))
            {
                //validation successful
                User userInfo = Converter.UserCredentialsToUserEntity(userCredentials);
                response = userFacade.SignIn(userInfo);
                return response;
            }
            else
            {
                //validation unsuccessful
                response = new ResponseUserModel();
                response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                response.Message = "Invalid user";
                return response;
            }
        }

        public ResponseUserModel ChangePassword(UserCredentials userCredentials)
        {
            ResponseUserModel response;
            if (validator.ChangePasswordValidator(userCredentials))
            {
                //validation successful
                User userInfo = Converter.UserCredentialsToUserEntity(userCredentials);
                response = userFacade.ChangePassword(userInfo, userCredentials.New_Password);
                return response;
            }
            else
            {
                //validation unsuccessful
                response = new ResponseUserModel();
                response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                response.Message = "Invalid user";
                return response;
            }
        }

        public ResponseModel SignOutUser(UserDataModel userDetails)
        {
            if (validator.EmailIdValidator(userDetails.Email))
            {
                //validation successful
                User userInfo = Converter.UserModelToUserEntity(userDetails);
                response = userFacade.SignOut(userInfo);
                return response;
            }
            else
            {
                //validation unsuccessful
                response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                response.Message = "Invalid user";
                return response;
            }
        }
    }
}