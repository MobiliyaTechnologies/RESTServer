using RestService.Entities;
using RestService.Models;
using RestService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Facade
{
    public class UserFacade
    {
        PowerGridEntities dbEntity;
        ResponseModel response;
        public UserFacade()
        {
            dbEntity = new PowerGridEntities();
            response = new ResponseModel();
        }

        public ResponseModel RegisterUser(User userDetails)
        {
            try
            {
                var data = (from record in dbEntity.User where record.Email_Id.Equals(userDetails.Email_Id) select record).SingleOrDefault();
                if (data != null)
                {
                    //Email exists already
                    response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                    response.Message = "Email already exists";
                    return response;
                }
                else
                {
                    userDetails.Status = 1;
                    userDetails.Creation_Date = DateTime.UtcNow;
                    dbEntity.User.Add(userDetails);
                    dbEntity.SaveChanges();
                    response.Status_Code = Convert.ToInt16(Constants.StatusCode.Ok);
                    response.Message = "User registered successfully";
                    return response;
                    //Email not found
                }
            }
            catch (Exception ex)
            {
                response.Status_Code = 0;
                response.Message = ex.Message;
                return response;
            }
        }

        public ResponseModel AddUserRoleMapping(User userDetails, UserRole userRoleDetail)
        {
            try
            {
                var data = (from record in dbEntity.User where record.Email_Id.Equals(userDetails.Email_Id) select record).SingleOrDefault();
                if (data != null)
                {
                    userRoleDetail.User_Id = data.Id;
                    dbEntity.UserRole.Add(userRoleDetail);
                    dbEntity.SaveChanges();
                    response.Status_Code = Convert.ToInt16(Constants.StatusCode.Ok);
                    response.Message = "User registered successfully";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                response.Message = ex.Message;
            }
            
            return response;
        }

        public ResponseModel CheckEmail(UserCredentials userCredentials)
        {
            try
            {
                var data = (from record in dbEntity.User where record.Email_Id.Equals(userCredentials.Email) select record).SingleOrDefault();
                if (data != null)
                {
                    //Email Registered
                    response.Status_Code = Convert.ToInt16(Constants.StatusCode.Ok);
                    response.Message = "User email registered";
                }
                else
                {
                    //Email Not Registered
                    response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                    response.Message = "User email not registered";
                }
            }
            catch (Exception ex)
            {
                response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                response.Message = ex.Message;
            }
            return response;
        }

        public ResponseUserModel SignIn(UserCredentials userDetails)
        {
            ResponseUserModel response = new ResponseUserModel();
            try
            {
                var user = (from data in dbEntity.User where data.Email_Id.Equals(userDetails.Email) && data.Password.Equals(userDetails.Password) select data).SingleOrDefault();
                if (user == null)
                {
                    //Invalid User
                    response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                    response.Message = "Either username or password is incorrect";
                    return response;
                }
                //Valid User
                //check is session already exist
                var session = (from u in dbEntity.UserSession where u.User_Id == user.Id select u).ToList();
                if (session != null)    //existing session
                {
                    //delete previous session
                    dbEntity.UserSession.RemoveRange(session);
                    dbEntity.SaveChanges();
                }
                 
                //create user session
                UserSession ssn = new UserSession { User_Id = user.Id, Last_Login_Time = DateTime.Now };
                dbEntity.UserSession.Add(ssn);
                dbEntity.SaveChanges();

                //update user session
                //var sess = dbEntity.UserSession.FirstOrDefault(c => c.User_Id == user.Id);
                //sess.Last_Login_Time = DateTime.Now;

                //send response
                response = Converter.UserToResponseUserModel(user);
                response.Status_Code = Convert.ToInt16(Constants.StatusCode.Ok);
                response.Message = "Successfully signed in";

            }
            catch (Exception ex)
            {
                response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                response.Message = ex.Message;
            }
            return response;
        }

        public ResponseModel SignOut(User userDetails)
        {
            try
            {
                var user = (from u in dbEntity.User where u.Email_Id.Equals(userDetails.Email_Id) select u).SingleOrDefault();
                if (user == null)
                {
                    //Invalid User
                    response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                    response.Message = "Invalid user";
                    return response;
                }
                var session = (from u in dbEntity.UserSession where u.User_Id == user.Id select u).SingleOrDefault();
                if (session == null)
                {
                    //Invalid User
                    response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                    response.Message = "User not logged in";
                } else {
                    //delete previous session
                    dbEntity.UserSession.Remove(session);
                    dbEntity.SaveChanges();
                    response.Status_Code = Convert.ToInt16(Constants.StatusCode.Ok);
                    response.Message = "Successfully signed out";
                }
            }
            catch (Exception ex)
            {
                response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                response.Message = ex.Message;
            }
            return response;
        }

        public ResponseUserModel ChangePassword(User userDetails, string newPassword)
        {
            ResponseUserModel response = new ResponseUserModel();
            try
            {
                var user = (from u in dbEntity.User where u.Email_Id.Equals(userDetails.Email_Id) && u.Password.Equals(userDetails.Password) select u).SingleOrDefault();
                if (user == null)
                {
                    //Invalid User
                    response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                    response.Message = "Either username or password is incorrect";
                    return response;
                }
                //Valid User
                //update password
                var usr = dbEntity.User.FirstOrDefault(c => c.Id == user.Id);
                usr.Password = newPassword;
                dbEntity.SaveChanges();

                //send response
                response = Converter.UserToResponseUserModel(user);
                response.Status_Code = Convert.ToInt16(Constants.StatusCode.Ok);
                response.Message = "Successfully changed password";

            }
            catch (Exception ex)
            {
                response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                response.Message = ex.Message;
            }
            return response;
        }
        
        public bool ValidateUser(int UserId)
        {
            try
            {
                var user = (from data in dbEntity.User where data.Id == UserId select data).FirstOrDefault();
                if (user == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string GetPassword(User userDetails)
        {
            string data = null;
            try
            {
                var user = (from u in dbEntity.User where u.Email_Id.Equals(userDetails.Email_Id) select u).SingleOrDefault();
                if (user != null)
                {
                    //Valid User
                    data = user.Password;
                }
            }
            catch (Exception ex) { }
            return data;
        }

        public ResponseModel ResetPassword(User userDetails)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var user = (from u in dbEntity.User where u.Email_Id.Equals(userDetails.Email_Id) select u).SingleOrDefault();
                if (user == null)
                {
                    //Invalid User
                    response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                    response.Message = "Invalid email id";
                    return response;
                }
                //Valid User
                //update password
                var usr = dbEntity.User.FirstOrDefault(c => c.Id == user.Id);
                usr.Password = userDetails.Password;
                dbEntity.SaveChanges();

                //send response
                response.Status_Code = Convert.ToInt16(Constants.StatusCode.Ok);
                response.Message = "Password successfully reset";

            }
            catch (Exception ex)
            {
                response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                response.Message = ex.Message;
            }
            return response;
        }
     }
}