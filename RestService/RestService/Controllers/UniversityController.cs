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
    public class UniversityController : ApiController
    {
        private readonly IUniversityService universityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UniversityController"/> class.
        /// </summary>
        public UniversityController()
        {
            this.universityService = new UniversityService();
        }

        [Route("GetAllUniversities")]
        public HttpResponseMessage GetAllUniversities()
        {
            var data = this.universityService.GetAllUniversities();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("GetUniversityByID/{universityID}")]
        public HttpResponseMessage GetUniversityByID(int universityID)
        {
            var data = this.universityService.GetUniversityByID(universityID);
            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No university found"));
        }

        [Route("AddUniversity")]
        [HttpPost]
        public HttpResponseMessage AddUniversity([FromBody] UniversityModel model)
        {
            if (model == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid university model.");
            }

            if (this.ModelState.IsValid)
            {
                var data = this.universityService.AddUniversity(model);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse((HttpStatusCode)611, messages);
        }

        [Route("UpdateUniversity")]
        [HttpPut]
        public HttpResponseMessage UpdateUniversity([FromBody] UniversityModel model)
        {
            if (model == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid university model.");
            }

            if (this.ModelState.IsValid)
            {
                var data = this.universityService.UpdateUniversity(model);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse((HttpStatusCode)611, messages);
        }

        [Route("DeleteUniversity/{universityID}")]
        [HttpDelete]
        public HttpResponseMessage DeleteUniversity(int universityID)
        {
            var data = this.universityService.DeleteUniversity(universityID);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("AddCampusToUniversity/{universityId}")]
        [HttpPut]
        public HttpResponseMessage AddCampusToUniversity(int universityId, [FromBody] List<int> campusIds)
        {
            if (universityId < 1 || campusIds.Count() < 1 || campusIds.Any(c => c < 1))
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid university id or campus ids.");
            }

            var campusAddedConfirmation = this.universityService.AddCampusesToUniversity(universityId, campusIds);
            return this.Request.CreateResponse(HttpStatusCode.OK, campusAddedConfirmation);
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