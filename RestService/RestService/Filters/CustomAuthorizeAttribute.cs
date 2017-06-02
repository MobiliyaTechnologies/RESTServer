namespace RestService.Filters
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Authentication;
    using System.Security.Claims;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using RestService.Enums;
    using RestService.Models;
    using RestService.Services;
    using RestService.Services.Impl;
    using RestService.Utilities;

    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public UserRole UserRole;

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

            if (!actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                  && !actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any() && actionContext.Response == null)
            {
                var userModel = this.InitializaUser();

                if (this.UserRole != UserRole.Any && this.UserRole != userModel.RoleType)
                {
                    actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "User does not have a permission to perform this operation");
                }
                else
                {
                    // set user context
                    var request = (HttpRequestMessage)HttpContext.Current.Items["MS_HttpRequestMessage"];
                    request.Properties.Add("Context", userModel);
                }
            }
        }

        private UserModel InitializaUser()
        {
            IUserService userService = null;
            IRoleService roleService = null;
            UserModel userModel = null;

            try
            {
                userService = new UserService();
                roleService = new RoleService();

                userModel = userService.GetCurrentUser(this.GetClaimValue(ApiConstant.B2cClaimObjectIdentifier));

                if (userModel == null)
                {
                    var newUserRole = roleService.GetNewUserRole();

                    userModel = new UserModel
                    {
                        B2C_ObjectIdentifier = this.GetClaimValue(ApiConstant.B2cClaimObjectIdentifier),
                        FirstName = this.GetClaimValue(ApiConstant.B2cClaimFirstName),
                        LastName = this.GetClaimValue(ApiConstant.B2cClaimLastName),
                        Email = this.GetClaimValue(ApiConstant.B2cClaimEmail),
                        RoleId = newUserRole.Id
                    };
                    userModel.UserId = userService.CreateUser(userModel);
                }
            }
            finally
            {
                if (userService != null)
                {
                    (userService as IDisposable).Dispose();
                }
                if (roleService != null)
                {
                    (roleService as IDisposable).Dispose();
                }
            }

            return userModel;
        }

        private string GetClaimValue(string claimType)
        {
            var claim = ClaimsPrincipal.Current.FindFirst(claimType);
            return claim != null ? claim.Value : string.Empty;
        }
    }
}