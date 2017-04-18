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

    public class CampusController : ApiController
    {
        private readonly ICampusService campusService;

        public CampusController()
        {
            this.campusService = new CampusService();
        }

        [Route("api/getallcampus")]
        public HttpResponseMessage GetAllCampus()
        {
            var data = this.campusService.GetAllCampus();
            if (data.Count != 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Campus found"));
        }

        [Route("api/getcampusbyid")]
        [HttpPost]
        public HttpResponseMessage GetCampusByID([FromBody] CampusModel model)
        {
            var data = this.campusService.GetCampusByID(model);
            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Campus found"));
        }

        [Route("api/addcampus")]
        [HttpPost]
        public HttpResponseMessage AddCampus([FromBody] CampusModel model)
        {
            if (this.ModelState.IsValid)
            {
                var userId = ServiceUtil.GetUser();
                var data = this.campusService.AddCampus(model, userId);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateResponse((HttpStatusCode)612, messages);
        }

        [Route("api/updatecampus")]
        [HttpPost]
        public HttpResponseMessage UpdateCampus([FromBody] CampusModel model)
        {
            if (this.ModelState.IsValid)
            {
                var userId = ServiceUtil.GetUser();
                var data = this.campusService.UpdateCampus(model, userId);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateResponse((HttpStatusCode)612, messages);
        }

        [Route("api/deletecampus")]
        [HttpPost]
        public HttpResponseMessage DeleteCampus([FromBody] CampusModel model)
        {
            var userId = ServiceUtil.GetUser();
            var data = this.campusService.DeleteCampus(model, userId);
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
                (this.campusService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}