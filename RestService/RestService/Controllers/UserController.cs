using System;
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

        [Route("api/signup")]
        [HttpPost]
        public HttpResponseMessage SignUp([FromBody] UserDataModel userDetails)
        {
            HttpResponseMessage response;
            try
            {
                log.Debug("Sign Up API called");
                if (ModelState.IsValid)
                {
                    var data = accountService.RegisterUser(userDetails);
                    response = Request.CreateResponse(HttpStatusCode.OK, data);
                    return response;
                }

                // Create an error message for returning
                string messages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                response = Request.CreateResponse(HttpStatusCode.BadRequest, messages);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in SignUp API as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
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
        public HttpResponseMessage SignIn([FromBody] UserCredentials userCredentials)
        {
            log.Debug("Sign In API called");
            HttpResponseMessage response;
            try
            {
                if (ModelState.IsValid)
                {
                    var data = accountService.SignInUser(userCredentials);
                    response = data.Status_Code == (int)Constants.StatusCode.Error ? Request.CreateResponse(HttpStatusCode.Forbidden, data.Message) : Request.CreateResponse(HttpStatusCode.OK, data);
                    return response;
                }

                string messages = string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                response = Request.CreateResponse(HttpStatusCode.BadRequest, messages);
                return response;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in SignIn API as: " + ex);
                response = Request.CreateErrorResponse(HttpStatusCode.ServiceUnavailable, ex);
                return response;
            }

        }

        [Route("api/changepassword")]
        [HttpPost]
        public ResponseUserModel ChangePassword([FromBody] UserCredentials userCredentials)
        {
            try
            {
                log.Debug("Change Password API called");
                return accountService.ChangePassword(userCredentials);
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in ChangePassword API as: " + ex);
                return new ResponseUserModel { Status_Code = (int)Constants.StatusCode.Error, Message = ex.Message };
            }
        }

        [Route("api/signout")]
        [HttpPost]
        public ResponseModel SignOut([FromBody] UserDataModel userDetails)
        {
            try
            {
                log.Debug("Sign Out API called");
                return accountService.SignOutUser(userDetails);
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in SignOut API as: " + ex);
                return new ResponseModel { Status_Code = (int)Constants.StatusCode.Error, Message = ex.Message};
            }
        }

        [Route("api/changeavatar")]
        [HttpPost]
        public ResponseUserModel ChangeAvatar([FromBody] UserDataModel userDetails )
        {
            try
            {
                log.Debug("Change Avatar API called");
                return accountService.ChangeAvatar(userDetails);
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in ChangeAvatar API as: " + ex);
                return new ResponseUserModel { Status_Code = (int)Constants.StatusCode.Error, Message = ex.Message };
            }
        }
    }
}
