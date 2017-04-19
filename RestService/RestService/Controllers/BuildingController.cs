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

    public class BuildingController : ApiController
    {
        private readonly IBuildingService buildingService;

        public BuildingController()
        {
            this.buildingService = new BuildingService();
        }

        [Route("api/getallbuildings")]
        public HttpResponseMessage GetAllBuildings()
        {
            var data = this.buildingService.GetAllBuildings();
            if (data.Count != 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Building found"));
        }

        [Route("api/getbuildingbyid/{buildingID}")]
        public HttpResponseMessage GetBuildingByID(int buildingID)
        {
            var data = this.buildingService.GetBuildingByID(buildingID);
            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Building found"));
        }

        [Route("api/addbuilding")]
        [HttpPost]
        public HttpResponseMessage AddBuilding([FromBody] BuildingModel model)
        {
            if (this.ModelState.IsValid)
            {
                var userId = ServiceUtil.GetUser();
                var data = this.buildingService.AddBuilding(model, userId);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse((HttpStatusCode)613, messages);
        }

        [Route("api/updatebuilding")]
        [HttpPost]
        public HttpResponseMessage UpdateBuilding([FromBody] BuildingModel model)
        {
            if (this.ModelState.IsValid)
            {
                var userId = ServiceUtil.GetUser();
                var data = this.buildingService.UpdateBuilding(model, userId);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse((HttpStatusCode)613, messages);
        }

        [Route("api/deletebuilding")]
        [HttpPost]
        public HttpResponseMessage DeleteBuilding([FromBody] BuildingModel model)
        {
            var userId = ServiceUtil.GetUser();
            var data = this.buildingService.DeleteBuilding(model, userId);
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