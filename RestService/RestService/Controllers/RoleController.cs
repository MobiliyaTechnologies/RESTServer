namespace RestService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using RestService.Models;
    using RestService.Services;
    using RestService.Services.Impl;
    using RestService.Utilities;

    public class RoleController : ApiController
    {
        private readonly IRoleService roleService;

        public RoleController()
        {
            this.roleService = new RoleService();
        }

        [Route("api/getallroles")]
        public HttpResponseMessage GetAllRoles()
        {
            var data = this.roleService.GetAllRoles();
            if (data.Count != 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Role found"));
        }

        [Route("api/getRolebyid/{roleId}")]
        public HttpResponseMessage GetRoleByID(int roleId)
        {
            var data = this.roleService.GetRoleByID(roleId);
            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Role found"));
        }

        [Route("api/addRole")]
        [HttpPost]
        public HttpResponseMessage AddRole([FromBody] RoleModel model)
        {
            if (this.ModelState.IsValid)
            {
                var userId = ServiceUtil.GetUser();
                var data = this.roleService.AddRole(model, userId);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse((HttpStatusCode)615, messages);
        }

        [Route("api/updateRole")]
        [HttpPost]
        public HttpResponseMessage UpdateRole([FromBody] RoleModel model)
        {
            if (this.ModelState.IsValid)
            {
                var userId = ServiceUtil.GetUser();
                var data = this.roleService.UpdateRole(model, userId);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse((HttpStatusCode)615, messages);
        }

        [Route("api/deleteRole")]
        [HttpPost]
        public HttpResponseMessage DeleteRole([FromBody] RoleModel model)
        {
            var userId = ServiceUtil.GetUser();
            var data = this.roleService.DeleteRole(model, userId);
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