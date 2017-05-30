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
    public class CampusController : ApiController
    {
        private readonly ICampusService campusService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CampusController"/> class.
        /// </summary>
        public CampusController()
        {
            this.campusService = new CampusService();
        }

        /// <summary>
        /// Gets all campus.
        /// </summary>
        /// <returns>The CampusDetails</returns>
        [Route("GetAllCampus")]
        [ResponseType(typeof(List<CampusModel>))]
        public HttpResponseMessage GetAllCampus()
        {
            var data = this.campusService.GetAllCampus();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the campus by identifier.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="campusId">The campus identifier.</param>
        /// <returns>The campus detail if found else not found error response, or bad request error response if invalid parameters.</returns>
        [Route("GetCampusByID/{campusId}")]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        [ResponseType(typeof(CampusModel))]
        public HttpResponseMessage GetCampusByID(int campusId)
        {
            if (campusId < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Campus id musty be grater than 0.");
            }

            var data = this.campusService.GetCampusByID(campusId);
            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Campus found"));
        }

        /// <summary>
        /// Gets the campus by location.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>The campus detail if found else not found error response, or bad request error response if invalid parameters.</returns>
        [Route("GetCampusByLocation/{latitude}/{longitude}")]
        [ResponseType(typeof(CampusModel))]
        public HttpResponseMessage GetCampusByLocation(decimal latitude, decimal longitude)
        {
            if (latitude == default(decimal) || longitude == default(decimal))
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid latitude or longitude");
            }

            var campus = this.campusService.GetCampusByLocation(latitude, longitude);

            if (campus != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, campus);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Campuses does not exists for given location, latitude - {0}  longitude - {1}", latitude, longitude));
        }

        /// <summary>
        /// Adds the campus.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="campusModel">The campus model.</param>
        /// <returns>The campus created confirmation, or bad request error response if invalid parameters.</returns>
        [Route("AddCampus")]
        [HttpPost]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage AddCampus([FromBody] CampusModel campusModel)
        {
            if (campusModel == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid campus model.");
            }

            if (this.ModelState.IsValid)
            {
                var data = this.campusService.AddCampus(campusModel);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, messages);
        }

        /// <summary>
        /// Updates the campus.
        /// Campus id is required to update campus other fields are optional, passed only fields required to update.
        /// </summary>
        /// <param name="campusModel">The campus model.</param>
        /// <returns>The campus update confirmation, or bad request error response if invalid parameters.</returns>
        [Route("UpdateCampus")]
        [HttpPut]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage UpdateCampus([FromBody] CampusModel campusModel)
        {
            if (campusModel.CampusID > 0)
            {
                var data = this.campusService.UpdateCampus(campusModel);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Campus id must be grater than zero.");
        }

        /// <summary>
        /// Deletes the campus for given campus identifier.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="campusId">The campus identifier.</param>
        /// <returns>The campus deleted confirmation, or bad request error response if invalid parameters.</returns>
        [Route("DeleteCampus/{campusId}")]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        [HttpDelete]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage DeleteCampus(int campusId)
        {
            if (campusId < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Campus id must be grater than zero.");
            }

            var data = this.campusService.DeleteCampus(campusId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Assigns the role to campus.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="roleIds">The role ids.</param>
        /// <param name="campusId">The campus identifier.</param>
        /// <returns>The role assigned confirmation, or bad request error response if invalid parameters.</returns>
        [Route("AssignRoleToCampus/{campusId}")]
        [HttpPut]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage AssignRoleToCampus(List<int> roleIds, int campusId)
        {
            if (roleIds == null || roleIds.Any(r => r < 1) || campusId < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid role id or campus id");
            }

            var responseModel = this.campusService.AssignRolesToCampus(roleIds, campusId);
            return this.Request.CreateResponse(HttpStatusCode.OK, responseModel);
        }

        /// <summary>
        /// Adds the buildings to campus.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="campusId">The campus identifier.</param>
        /// <param name="buildingIds">The building ids.</param>
        /// <returns>The building assigned confirmation, or bad request error response if invalid parameters.</returns>
        [Route("AddBuildingsToCampus/{campusId}")]
        [HttpPut]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage AddBuildingsToCampus(int campusId, [FromBody] List<int> buildingIds)
        {
            if (campusId < 1 || buildingIds == null || buildingIds.Count() == 0)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid campus id or building ids");
            }

            var responseModel = this.campusService.AddBuildingsToCampus(campusId, buildingIds);
            return this.Request.CreateResponse(HttpStatusCode.OK, responseModel);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.campusService != null)
            {
                (this.campusService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}