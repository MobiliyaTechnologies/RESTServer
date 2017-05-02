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
    public class BuildingController : ApiController
    {
        private readonly IBuildingService buildingService;

        public BuildingController()
        {
            this.buildingService = new BuildingService();
        }

        [Route("GetAllBuildings")]
        public HttpResponseMessage GetAllBuildings()
        {
            var data = this.buildingService.GetAllBuildings();
            if (data.Count != 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Building found"));
        }

        [Route("GetBuildingByID/{buildingId}")]
        public HttpResponseMessage GetBuildingByID(int buildingId)
        {
            var data = this.buildingService.GetBuildingByID(buildingId);
            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Building found"));
        }

        [Route("GetBuildingsByCampus/{campusId}")]
        public HttpResponseMessage GetBuildingsByCampus(int campusId)
        {
            var data = this.buildingService.GetBuildingsByCampus(campusId);
            if (data.Count != 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Building found"));
        }

        [Route("GetBuildingByLocation/{latitude}/{longitude}")]
        public HttpResponseMessage GetBuildingByLocation(decimal latitude, decimal longitude)
        {
            if (latitude == default(decimal) || longitude == default(decimal))
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid latitude or longitude");
            }

            var campus = this.buildingService.GetBuildingByLocation(latitude, longitude);

            if (campus != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, campus);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Building does not exists for given location, latitude - {0}  longitude - {1}", latitude, longitude));
        }

        [Route("GetBuildings")]
        public HttpResponseMessage GetBuildings()
        {
            var data = this.buildingService.GetBuildings();
            if (data.Count != 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Building found"));
        }

        [Route("AddBuilding")]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        [HttpPost]
        public HttpResponseMessage AddBuilding([FromBody] BuildingModel model)
        {
            if (this.ModelState.IsValid)
            {
                var data = this.buildingService.AddBuilding(model);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse((HttpStatusCode)613, messages);
        }

        [Route("UpdateBuilding")]
        [HttpPut]
        public HttpResponseMessage UpdateBuilding([FromBody] BuildingModel model)
        {
            if (this.ModelState.IsValid)
            {
                var data = this.buildingService.UpdateBuilding(model);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse((HttpStatusCode)613, messages);
        }

        [Route("DeleteBuilding/{buildingId}")]
        [HttpDelete]
        public HttpResponseMessage DeleteBuilding(int buildingId)
        {
            var data = this.buildingService.DeleteBuilding(buildingId);
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
                (this.buildingService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}