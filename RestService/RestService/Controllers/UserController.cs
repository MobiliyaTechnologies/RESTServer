namespace RestService.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using RestService.Enums;
    using RestService.Filters;
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

            user.UserCampus = this.campusService.GetCampus();

            return this.Request.CreateResponse(HttpStatusCode.OK, user);
        }

        [Route("GetAllUsers")]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        public HttpResponseMessage GetAllUsers()
        {
            var users = this.userService.GetAllUser();

            if (users == null || users.Count == 0)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Users not exists.");
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, users);
        }

        [Route("UpdateUser")]
        [HttpPut]
        public HttpResponseMessage UpdateUser()
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
    }
}
