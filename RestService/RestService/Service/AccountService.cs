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

        public ResponseUserModel SignInUser(UserCredentials userCredentials)
        {
            try
            {
                userCredentials.Email = userCredentials.Email.Replace(" ", string.Empty);
                userCredentials.Password = userCredentials.Password.Replace(" ", string.Empty);

                ResponseUserModel userResponse;
                ResponseModel response = validator.SignInValidator(userCredentials); //Check for correct credential format
                if (response.Status_Code == (int)Constants.StatusCode.Ok)
                {
                    //validation successful

                    userResponse = userFacade.SignIn(userCredentials);
                    return userResponse;
                }
                else
                {
                    //validation unsuccessful
                    userResponse = new ResponseUserModel();
                    userResponse.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                    userResponse.Message = response.Message;
                    return userResponse;
                }
            }
            catch (Exception ex)
            {
                return new ResponseUserModel { Status_Code = (int)Constants.StatusCode.Error, Message = ex.Message };
            }
        }

        public ResponseUserModel ChangePassword(UserCredentials userCredentials)
        {
            ResponseUserModel response;
            if (validator.ChangePasswordValidator(userCredentials))
            {
                //validation successful
                if (userCredentials.New_Password.Equals(userCredentials.Password))
                {
                    response = new ResponseUserModel();
                    response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                    response.Message = "New password and old password can not be same";
                    return response;
                }
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

        public ResponseModel ForgotPassword(UserCredentials userCredentials)
        {
            if (validator.ResetPasswordValidator(userCredentials))
            {
                //validation successful
                response = userFacade.CheckEmail(userCredentials);
                if (response.Status_Code == Convert.ToInt16(Constants.StatusCode.Ok))
                {
                    string newPw = RandomString(8);
                    //changePassword
                    User userInfo = Converter.UserCredentialsToUserEntity(userCredentials);
                    response = userFacade.ResetPassword(userInfo, newPw);

                    if (response.Status_Code == Convert.ToInt16(Constants.StatusCode.Ok))
                    {
                        //Send Email
                        string messageBody = "Your CSU password has been reset." + 
                            
                            "</br></br>username: <b>" + userCredentials.Email + 
                            "</b></br>password: <b>" + newPw +

                            "</b></br></br><p>Please change your password when you " +
                            "<a href=\"http://13.72.102.73/CSU/#/login\">login</a> to CSU.</p>" +
                            
                            "</br></br></br><i>Mobiliya Team</i>";

                        response = ServiceUtil.SendMail(userCredentials.Email, "CSU Password Reset", messageBody);
                        return response;
                    }
                    return response;
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

        
        public string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
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

        public bool ValidateUser(int UserId)
        {
            return userFacade.ValidateUser(UserId);
        }
    }
}