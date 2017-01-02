﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RestService.Models;
using RestService.Service;
using RestService.Utilities;

namespace RestService.Controllers
{
    public class UserController : ApiController
    {
        private AccountService accountService;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UserController()
        {
            accountService = new AccountService();
        }

        // GET: api/User
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/User
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/User/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }

        [Route("api/signup")]
        [HttpPost]
        public ResponseModel SignUp([FromBody] UserDataModel userDetails)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                log.Debug("Sign Up API called");
                response = accountService.RegisterUser(userDetails);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in SignUp API as: " + ex);
                response.Message = ex.Message;
                response.Status_Code = (int)Constants.StatusCode.Error;
                return response;
            }
        }

        [Route("api/forgotpassword")]
        [HttpPost]
        public ResponseModel ForgotPassword([FromBody] UserCredentials userCredentials)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                log.Debug("Forgot Password API called");
                response = accountService.ForgotPassword(userCredentials);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in ForgotPassword as: " + ex);
                response.Message = ex.Message;
                response.Status_Code = (int)Constants.StatusCode.Error;
                return response;
            }
        }

        [Route("api/signin")]
        [HttpPost]
        public ResponseUserModel SignIn([FromBody] UserCredentials userCredentials)
        {
            log.Debug("Sign In API called");
            return accountService.SignInUser(userCredentials);
        }

        [Route("api/changepassword")]
        [HttpPost]
        public ResponseUserModel ChangePassword([FromBody] UserCredentials userCredentials)
        {
            log.Debug("Change Password API called");
            return accountService.ChangePassword(userCredentials);
        }

        [Route("api/signout")]
        [HttpPost]
        public ResponseModel SignOut([FromBody] UserDataModel userDetails)
        {
            log.Debug("Sign Out API called");
            return accountService.SignOutUser(userDetails);
        }

        [Route("api/changeavatar")]
        [HttpPost]
        public ResponseUserModel ChangeAvatar([FromBody] UserDataModel userDetails )
        {
            log.Debug("Change Avatar API called");
            return accountService.ChangeAvatar(userDetails);
        }
    }
}
