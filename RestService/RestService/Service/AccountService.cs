using RestService.Entities;
using RestService.Facade;
using RestService.Models;
using RestService.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            try
            {
                User userInfo = Converter.UserModelToUserEntity(userDetails);
                response = userFacade.RegisterUser(userInfo);
                if (response.Status_Code == (int)Constants.StatusCode.Ok)
                {
                    UserRole userRoleInfo = Converter.UserModelToUserRoleEntity(userDetails);
                    response = userFacade.AddUserRoleMapping(userInfo, userRoleInfo);
                }

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public ResponseUserModel SignInUser(UserCredentials userCredentials)
        {
            try
            {
                ResponseUserModel userResponse;
                userResponse = userFacade.SignIn(userCredentials);
                return userResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public ResponseUserModel ChangePassword(UserCredentials userCredentials)
        {
            try
            {
                ResponseUserModel response;
                if (validator.ChangePasswordValidator(userCredentials))
                {
                    // validation successful
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
                    // validation unsuccessful
                    response = new ResponseUserModel();
                    response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                    response.Message = "Invalid user";
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public ResponseModel ForgotPassword(UserCredentials userCredentials)
        {
            try
            {
                if (validator.ResetPasswordValidator(userCredentials))
                {
                    // validation successful
                    response = userFacade.CheckEmail(userCredentials);
                    if (response.Status_Code == Convert.ToInt16(Constants.StatusCode.Ok))
                    {
                        string newPw = RandomString(8);

                        // changePassword
                        User userInfo = Converter.UserCredentialsToUserEntity(userCredentials);
                        response = userFacade.ResetPassword(userInfo, newPw);

                        if (response.Status_Code == Convert.ToInt16(Constants.StatusCode.Ok))
                        {
                            // Send Email
                            string messageBody = "Your Account password has been reset." +

                                "</br></br>username: <b>" + userCredentials.Email +
                                "</b></br>password: <b>" + newPw +

                                "</b></br></br><p>Please change your password when you " +
                                "<a href=\"" + ConfigurationManager.AppSettings["LoginPage"] + "\">login</a> to CSU.</p>" +

                                "</br></br></br><i>Microsoft Team</i>";

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
                    // validation unsuccessful
                    response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                    response.Message = "Validation Faliure";
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public ResponseUserModel ChangeAvatar(UserDataModel userDetails)
        {
            try
            {
                ResponseUserModel response = userFacade.ChangeAvatar(userDetails);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public string RandomString(int length)
        {
            try
            {
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
                return new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public ResponseModel SignOutUser(UserDataModel userDetails)
        {
            try
            {
                if (validator.EmailIdValidator(userDetails.Email))
                {
                    // validation successful
                    User userInfo = Converter.UserModelToUserEntity(userDetails);
                    response = userFacade.SignOut(userInfo);
                    return response;
                }
                else
                {
                    // validation unsuccessful
                    response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                    response.Message = "Invalid user";
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public bool ValidateUser(int UserId)
        {
            try
            {
                return userFacade.ValidateUser(UserId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}