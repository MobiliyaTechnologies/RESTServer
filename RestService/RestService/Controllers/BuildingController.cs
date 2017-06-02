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
    public class BuildingController : ApiController
    {
        private readonly IBuildingService buildingService;

        public BuildingController()
        {
            this.buildingService = new BuildingService();
        }

        /// <summary>
        /// Gets all building details.
        /// </summary>
        /// <returns>The building details.</returns>
        [Route("GetAllBuildings")]
        [ResponseType(typeof(List<BuildingModel>))]
        public HttpResponseMessage GetAllBuildings()
        {
            var data = this.buildingService.GetAllBuildings();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the building by identifier.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <returns>The building detail if found else not found error response, or bad request error response if invalid building id.</returns>
        [Route("GetBuildingByID/{buildingId}")]
        [ResponseType(typeof(BuildingModel))]
        public HttpResponseMessage GetBuildingByID(int buildingId)
        {
            if (buildingId < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Format("Building id must be grater than zero."));
            }

            var data = this.buildingService.GetBuildingByID(buildingId);
            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Building found"));
        }

        /// <summary>
        /// Gets the buildings by premise.
        /// </summary>
        /// <param name="premiseId">The premise identifier.</param>
        /// <returns>The building details.</returns>
        [Route("GetBuildingsByPremise/{premiseId}")]
        [ResponseType(typeof(List<BuildingModel>))]
        public HttpResponseMessage GetBuildingsByPremise(int premiseId)
        {
            var data = this.buildingService.GetBuildingsByPremise(premiseId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the building by location.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>The building detail if found else not found error response, or bad request error response if invalid location details.</returns>
        [Route("GetBuildingByLocation/{latitude}/{longitude}")]
        [ResponseType(typeof(BuildingModel))]
        public HttpResponseMessage GetBuildingByLocation(decimal latitude, decimal longitude)
        {
            if (latitude == default(decimal) || longitude == default(decimal))
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid latitude or longitude");
            }

            var buildings = this.buildingService.GetBuildingByLocation(latitude, longitude);

            if (buildings != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, buildings);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Building does not exists for given location, latitude - {0}  longitude - {1}", latitude, longitude));
        }

        /// <summary>
        /// Adds the building.
        /// </summary>
        /// <param name="buildingModel">The building model.</param>
        /// <returns>The building created confirmation, or bad request error response if invalid parameters.</returns>
        [Route("AddBuilding")]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        [HttpPost]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage AddBuilding([FromBody] BuildingModel buildingModel)
        {
            if (buildingModel == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid building model.");
            }

            if (this.ModelState.IsValid)
            {
                var data = this.buildingService.AddBuilding(buildingModel);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, messages);
        }

        /// <summary>
        /// Updates the building.
        /// Building id is required to update building details other fields are optional, passed only fields required to update.
        /// Building name and description are only modifiable fields.
        /// </summary>
        /// <param name="buildingModel">The building model.</param>
        /// <returns>The building updated confirmation, or bad request error response if invalid parameters. </returns>
        [Route("UpdateBuilding")]
        [HttpPut]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage UpdateBuilding([FromBody] BuildingModel buildingModel)
        {
            if (buildingModel == null && buildingModel.BuildingID < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid building model.");
            }

            var data = this.buildingService.UpdateBuilding(buildingModel);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Deletes the building for given id.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <returns>The building deleted confirmation, or bad request error response if invalid parameters.</returns>
        [Route("DeleteBuilding/{buildingId}")]
        [HttpDelete]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage DeleteBuilding(int buildingId)
        {
            if (buildingId < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Building id must be grater than 0");
            }

            var data = this.buildingService.DeleteBuilding(buildingId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.buildingService != null)
            {
                (this.buildingService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}