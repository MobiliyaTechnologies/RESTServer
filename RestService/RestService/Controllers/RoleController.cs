namespace RestService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleController"/> class.
        /// </summary>
        public RoleController()
        {
            this.roleService = new RoleService();
        }

        /// <summary>
        /// Gets all roles.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <returns>The role details.</returns>
        [Route("GetAllRoles")]
        [ResponseType(typeof(List<RoleModel>))]
        public HttpResponseMessage GetAllRoles()
        {
            var data = this.roleService.GetAllRoles();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the role by identifier.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>The role detail if found else not found error response, or bad request error response if invalid parameters.</returns>
        [Route("GetRoleByID/{roleId}")]
        [ResponseType(typeof(RoleModel))]
        public HttpResponseMessage GetRoleByID(int roleId)
        {
            if (roleId < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Role id musty be grater than 0.");
            }

            var data = this.roleService.GetRoleByID(roleId);
            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Role found"));
        }

        /// <summary>
        /// Adds the role.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="roleModel">The role model.</param>
        /// <returns>The role added confirmation, or bad request error response if invalid parameters.</returns>
        [Route("AddRole")]
        [HttpPost]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage AddRole([FromBody] RoleModel roleModel)
        {
            string messages = string.Empty;
            HttpStatusCode statusCode;
            if (roleModel == null)
            {
                statusCode = HttpStatusCode.BadRequest;
                messages = "Invalid role model.";
            }
            else if (roleModel.CampusIds.Count == 0)
            {
                statusCode = HttpStatusCode.BadRequest;
                messages = "Role must be created with campus ids.";
            }
            else if (this.ModelState.IsValid)
            {
                var data = this.roleService.AddRole(roleModel);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }
            else
            {
                messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                statusCode = HttpStatusCode.BadRequest;
            }

            return this.Request.CreateErrorResponse(statusCode, messages);
        }

        /// <summary>
        /// Updates the role.
        /// Role id is required to update role details other fields are optional, passed only fields required to update.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="roleModel">The role model.</param>
        /// <returns>The role updated confirmation, or bad request error response if invalid parameters.</returns>
        [Route("UpdateRole")]
        [HttpPut]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage UpdateRole([FromBody] RoleModel roleModel)
        {
            if (roleModel == null && roleModel.Id < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid role model.");
            }

            var data = this.roleService.UpdateRole(roleModel);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Deletes the role for given role identifier.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>The role deleted confirmation.</returns>
        [Route("DeleteRole/{roleId}")]
        [HttpDelete]
        [ResponseType(typeof(ResponseModel))]
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