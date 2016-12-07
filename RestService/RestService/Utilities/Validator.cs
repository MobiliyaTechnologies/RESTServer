using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.Models;
using System.Text.RegularExpressions;

namespace RestService.Utilities
{
    public class Validator
    {
        public bool SignUpValidator(UserDataModel userDetails)
        {
            try
            {
                if(!string.IsNullOrEmpty(userDetails.First_Name) && !string.IsNullOrEmpty(userDetails.Email) && userDetails.Role_Id > 0)
                {
                    //Validation Successful
                    return true;
                }
                else
                {
                    //Validation Unsuccessful
                    return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool ResetPasswordValidator(UserDataModel userDetails)
        {
            try
            {
                if (!string.IsNullOrEmpty(userDetails.Email))
                {
                    //validation successful
                    return true;
                }
                else
                {
                    //validation unsuccessful
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SignInValidator(UserCredentials userCredentials)
        {
            try
            {
                if (!string.IsNullOrEmpty(userCredentials.Email) && !string.IsNullOrEmpty(userCredentials.Password))
                {
                    //check email
                    bool isValidEmail = EmailIdValidator(userCredentials.Email);

                    //check password
                    bool isValidPassword = false;
                    if (userCredentials.Email.Length > 2)
                    {
                        isValidPassword = true;
                    }
                    //Validation Successful
                    return (isValidEmail && isValidPassword);
                }
                else
                {
                    //Validation Unsuccessful
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool ChangePasswordValidator(UserCredentials userCredentials)
        {
            try
            {
                if (!string.IsNullOrEmpty(userCredentials.Email) && 
                    !string.IsNullOrEmpty(userCredentials.Password) &&
                    !string.IsNullOrEmpty(userCredentials.New_Password))
                {
                    //check email
                    bool isValidEmail = EmailIdValidator(userCredentials.Email);

                    //check password
                    bool isValidPassword = false;
                    if (userCredentials.Password.Length > 2 && userCredentials.New_Password.Length > 2)
                    {
                        isValidPassword = true;
                    }
                    //Validation Successful
                    return (isValidEmail && isValidPassword);
                }
                else
                {
                    //Validation Unsuccessful
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool EmailIdValidator(string email)
        {
            bool isValidEmail = false;
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    //check email
                    isValidEmail = Regex.IsMatch(email,
                        @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                        RegexOptions.IgnoreCase);
                }
            }
            catch (Exception ex) { }
            return isValidEmail;
        }
    }
}