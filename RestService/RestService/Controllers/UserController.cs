namespace RestService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;
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

        /// <summary>
        /// Gets the logged-in user.
        /// </summary>
        /// <returns>The user detail.</returns>
        [Route("GetCurrentUser")]
        [ResponseType(typeof(UserModel))]
        public HttpResponseMessage GetCurrentUser()
        {
            var user = this.context.Current;
            return this.Request.CreateResponse(HttpStatusCode.OK, user);
        }

        /// <summary>
        /// Gets the logged-in user with it's accessible campus.
        /// </summary>
        /// <returns>The user detail with campus.</returns>
        [Route("GetCurrentUserWithCampus")]
        [ResponseType(typeof(UserModel))]
        public HttpResponseMessage GetCurrentUserWithCampus()
        {
            var user = this.context.Current;
            user.UserCampus = this.campusService.GetAllCampus();

            return this.Request.CreateResponse(HttpStatusCode.OK, user);
        }

        /// <summary>
        /// Gets all users.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <returns>The user details.</returns>
        [Route("GetAllUsers")]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        [ResponseType(typeof(List<UserModel>))]
        public HttpResponseMessage GetAllUsers()
        {
            var users = this.userService.GetAllUser();
            return this.Request.CreateResponse(HttpStatusCode.OK, users);
        }

        /// <summary>
        /// Updates the user.
        /// Can only update logged-in user, Passed only fields required to update
        /// </summary>
        /// <param name="userModel">The user model.</param>
        /// <returns>The user updated confirmation, or bad request error response if invalid parameters.</returns>
        [Route("UpdateUser")]
        [HttpPut]
        [ResponseType(typeof(ResponseModel))]
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

        /// <summary>
        /// Updates the user by updated b2c claim.
        /// It's use to update user after b2c edit profile.
        /// </summary>
        /// <returns>he user updated confirmation.</returns>
        [Route("UpdateUserByClaim")]
        [HttpPut]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage UpdateUserByClaim()
        {
            var responseModel = this.userService.UpdateUser(this.context.Current);
            return this.Request.CreateResponse(HttpStatusCode.OK, responseModel);
        }

        /// <summary>
        /// Deletes the logged-in user.
        /// It's used to delete user on removing user from b2c.
        /// </summary>
        /// <returns>The user deleted confirmation.</returns>
        [Route("DeleteUser")]
        [HttpDelete]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage DeleteUser()
        {
            var responseModel = this.userService.DeleteUser(this.context.Current.B2C_ObjectIdentifier);
            return this.Request.CreateResponse(HttpStatusCode.OK, responseModel);
        }

        /// <summary>
        /// Assigns the role to user.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>The user role assignment confirmation.</returns>
        [Route("AssignRoleToUser/{userId}/{roleId}")]
        [HttpPut]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        [ResponseType(typeof(ResponseModel))]
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
