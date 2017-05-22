namespace RestService.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using RestService.Enums;
    using RestService.Filters;
    using RestService.Models;
    using RestService.Services;
    using RestService.Services.Impl;

    [RoutePrefix("api")]
    public class UserController : ApiController
    {
        private readonly IUserService userService;
        private readonly ICampusService campusService;
        private readonly IContextInfoAccessorService context;

        public UserController()
        {
            this.userService = new UserService();
            this.campusService = new CampusService();
            this.context = new ContextInfoAccessorService();
        }

        [Route("GetCurrentUser")]
        public HttpResponseMessage GetCurrentUser()
        {
            var user = this.context.Current;

            if (user == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "User not exists.");
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, user);
        }

        [Route("GetCurrentUserWithCampus")]
        public HttpResponseMessage GetCurrentUserWithCampus()
        {
            var user = this.context.Current;

            if (user == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "User not exists.");
            }

            user.UserCampus = this.campusService.GetAllCampus();

            return this.Request.CreateResponse(HttpStatusCode.OK, user);
        }

        [Route("GetAllUsers")]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        public HttpResponseMessage GetAllUsers()
        {
            var users = this.userService.GetAllUser();
            return this.Request.CreateResponse(HttpStatusCode.OK, users);
        }

        [Route("UpdateUser")]
        [HttpPut]
        public HttpResponseMessage UpdateUser(UserModel userModel)
        {
            if (userModel.UserId > 0 && userModel.UserId != this.context.Current.UserId)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Users can only modify their own profile.");
            }

            userModel.UserId = this.context.Current.UserId;
            var responseModel = this.userService.UpdateUser(userModel);
            return this.Request.CreateResponse(HttpStatusCode.OK, responseModel);
        }

        [Route("UpdateUserByClaim")]
        [HttpPut]
        public HttpResponseMessage UpdateUserByClaim()
        {
            var responseModel = this.userService.UpdateUser(this.context.Current);
            return this.Request.CreateResponse(HttpStatusCode.OK, responseModel);
        }

        [Route("DeleteUser")]
        [HttpDelete]
        public HttpResponseMessage DeleteUser()
        {
            var responseModel = this.userService.DeleteUser(this.context.Current.B2C_ObjectIdentifier);
            return this.Request.CreateResponse(HttpStatusCode.OK, responseModel);
        }

        [Route("AssignRoleToUser/{userId}/{roleId}")]
        [HttpPut]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        public HttpResponseMessage AssignRoleToUser(int userId, int roleId)
        {
            var responseModel = this.userService.AssignRoleToUser(userId, roleId);
            return this.Request.CreateResponse(HttpStatusCode.OK, responseModel);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if ( this.userService != null )
                {
                    (this.userService as IDisposable).Dispose();
                }

                if (this.campusService != null)
                {
                    (this.userService as IDisposable).Dispose();
                }
            }

            base.Dispose(disposing);
        }
    }
}
