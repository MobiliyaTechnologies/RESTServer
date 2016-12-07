using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RestService.Models;
using RestService.Service;

namespace RestService.Controllers
{
    public class UserController : ApiController
    {
        private AccountService accountService;
        private ResponseModel response;
        
        public UserController()
        {
            accountService = new AccountService();
            response = new ResponseModel();
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
            response = accountService.RegisterUser(userDetails);
            return response;
        }

        [Route("api/resetpassword")]
        [HttpPost]
        public ResponseModel ResetPassword([FromBody] UserDataModel userDetails)
        {
            response = accountService.ResetPassword(userDetails);
            return response;
        }

        [Route("api/signin")]
        [HttpPost]
        public ResponseUserModel SignIn([FromBody] UserCredentials userCredentials)
        {
            return accountService.SignInUser(userCredentials);
        }

        [Route("api/changepassword")]
        [HttpPost]
        public ResponseUserModel ChangePassword([FromBody] UserCredentials userCredentials)
        {
            return accountService.ChangePassword(userCredentials);
        }

        [Route("api/signout")]
        [HttpPost]
        public ResponseModel SignOut([FromBody] UserDataModel userDetails)
        {
            return accountService.SignOutUser(userDetails);
        }
    }
}
