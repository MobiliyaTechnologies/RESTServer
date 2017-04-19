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

    public class UniversityController : ApiController
    {
        private readonly IUniversityService universityService;

        public UniversityController()
        {
            this.universityService = new UniversityService();
        }

        [Route("api/getalluniversities")]
        public HttpResponseMessage GetAllUniversities()
        {
            var data = this.universityService.GetAllUniversities();
            if (data.Count != 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No university found"));
        }

        [Route("api/getuniversitybyid/{universityID}")]
        public HttpResponseMessage GetUniversityByID(int universityID)
        {
            var data = this.universityService.GetUniversityByID(universityID);
            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No university found"));
        }

        [Route("api/adduniversity")]
        [HttpPost]
        public HttpResponseMessage AddUniversity([FromBody] UniversityModel model)
        {
            if (this.ModelState.IsValid)
            {
                var userId = ServiceUtil.GetUser();
                var data = this.universityService.AddUniversity(model, userId);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse((HttpStatusCode)611, messages);
        }

        [Route("api/updateuniversity")]
        [HttpPost]
        public HttpResponseMessage UpdateUniversity([FromBody] UniversityModel model)
        {
            if (this.ModelState.IsValid)
            {
                var userId = ServiceUtil.GetUser();
                var data = this.universityService.UpdateUniversity(model, userId);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse((HttpStatusCode)611, messages);
        }

        [Route("api/deleteuniversity")]
        [HttpPost]
        public HttpResponseMessage DeleteUniversity([FromBody] UniversityModel model)
        {
            var userId = ServiceUtil.GetUser();
            var data = this.universityService.DeleteUniversity(model, userId);
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
                (this.universityService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}