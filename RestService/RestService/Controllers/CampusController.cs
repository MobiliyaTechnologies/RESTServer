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
    public class CampusController : ApiController
    {
        private readonly ICampusService campusService;

        public CampusController()
        {
            this.campusService = new CampusService();
        }

        [Route("GetAllCampus")]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        public HttpResponseMessage GetAllCampus()
        {
            var data = this.campusService.GetAllCampus();

            if (data.Count != 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Campus found"));
        }

        [Route("GetCampusByID/{campusId}")]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        public HttpResponseMessage GetCampusByID(int campusId)
        {
            var data = this.campusService.GetCampusByID(campusId);
            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Campus found"));
        }

        [Route("GetCampus")]
        public HttpResponseMessage GetCampus()
        {
            var campus = this.campusService.GetCampus();

            if (campus != null && campus.Count > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, campus);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Campuses does not associated with current user"));
        }

        [Route("GetCampusByLocation/{latitude}/{longitude}")]
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

        [Route("AddCampus")]
        [HttpPost]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        public HttpResponseMessage AddCampus([FromBody] CampusModel model)
        {
            if (this.ModelState.IsValid)
            {
                var data = this.campusService.AddCampus(model);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse((HttpStatusCode)612, messages);
        }

        [Route("UpdateCampus")]
        [HttpPut]
        public HttpResponseMessage UpdateCampus([FromBody] CampusModel model)
        {
            if (this.ModelState.IsValid)
            {
                var data = this.campusService.UpdateCampus(model);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse((HttpStatusCode)612, messages);
        }

        [Route("DeleteCampus/{campusId}")]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        [HttpDelete]
        public HttpResponseMessage DeleteCampus(int campusId)
        {
            var data = this.campusService.DeleteCampus(campusId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("AssignRoleToCampus/{roleId}/{campusId}")]
        [HttpPut]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        public HttpResponseMessage AssignRoleToCampus(int roleId, int campusId)
        {
            if (roleId == 0 || campusId == 0)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid role id or campus id");
            }

            var responseModel = this.campusService.AssignRoleToCampus(roleId, campusId);
            return this.Request.CreateResponse(HttpStatusCode.OK, responseModel);
        }

        [Route("AddBuildingsToCampus/{campusId}")]
        [HttpPut]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
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
            if (disposing)
            {
                (this.campusService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}