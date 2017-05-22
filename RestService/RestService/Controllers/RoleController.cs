namespace RestService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using RestService.Enums;
    using RestService.Filters;
    using RestService.Models;
    using RestService.Services;
    using RestService.Services.Impl;
    using RestService.Utilities;

    [RoutePrefix("api")]
    [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
    [OverrideAuthorization]
    public class RoleController : ApiController
    {
        private readonly IRoleService roleService;

        public RoleController()
        {
            this.roleService = new RoleService();
        }

        [Route("GetAllRoles")]
        public HttpResponseMessage GetAllRoles()
        {
            var data = this.roleService.GetAllRoles();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("GetRoleByID/{roleId}")]
        public HttpResponseMessage GetRoleByID(int roleId)
        {
            var data = this.roleService.GetRoleByID(roleId);
            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Role found"));
        }

        [Route("AddRole")]
        [HttpPost]
        public HttpResponseMessage AddRole([FromBody] RoleModel model)
        {
            string messages = string.Empty;
            HttpStatusCode statusCode;
            if (model == null)
            {
                statusCode = HttpStatusCode.BadRequest;
                messages = "Invalid role model.";
            }
            else if (model.CampusIds.Count == 0)
            {
                statusCode = HttpStatusCode.BadRequest;
                messages = "Role must be created with campus ids.";
            }
            else if (this.ModelState.IsValid)
            {
                var data = this.roleService.AddRole(model);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }
            else
            {
                messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                statusCode = (HttpStatusCode)615;
            }

            return this.Request.CreateErrorResponse(statusCode, messages);
        }

        [Route("UpdateRole")]
        [HttpPut]
        public HttpResponseMessage UpdateRole([FromBody] RoleModel model)
        {
            if (model == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid role model.");
            }

            if (this.ModelState.IsValid)
            {
                var data = this.roleService.UpdateRole(model);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse((HttpStatusCode)615, messages);
        }

        [Route("DeleteRole/{roleId}")]
        [HttpDelete]
        public HttpResponseMessage DeleteRole(int roleId)
        {
            var data = this.roleService.DeleteRole(roleId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                (this.roleService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}