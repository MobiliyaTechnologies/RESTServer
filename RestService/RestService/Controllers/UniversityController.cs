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

        /// <summary>
        /// Gets all universities.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <returns>The university details.</returns>
        [Route("GetAllUniversities")]
        [ResponseType(typeof(List<UniversityModel>))]
        public HttpResponseMessage GetAllUniversities()
        {
            var data = this.universityService.GetAllUniversities();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the university by identifier.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="universityID">The university identifier.</param>
        /// <returns>The university detail if found else not found error response.</returns>
        [Route("GetUniversityByID/{universityID}")]
        [ResponseType(typeof(UniversityModel))]
        public HttpResponseMessage GetUniversityByID(int universityID)
        {
            var data = this.universityService.GetUniversityByID(universityID);
            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No university found"));
        }

        /// <summary>
        /// Adds the university.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="universityModel">The university model.</param>
        /// <returns>The university added confirmation, or bad request error response if invalid parameters.</returns>
        [Route("AddUniversity")]
        [HttpPost]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage AddUniversity([FromBody] UniversityModel universityModel)
        {
            if (universityModel == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid university model.");
            }

            if (this.ModelState.IsValid)
            {
                var data = this.universityService.AddUniversity(universityModel);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, messages);
        }

        /// <summary>
        /// Updates the university.
        /// University id is required to update university other fields are optional, passed only fields required to update.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="universityModel">The university model.</param>
        /// <returns>The university updated confirmation, or bad request error response if invalid parameters.</returns>
        [Route("UpdateUniversity")]
        [HttpPut]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage UpdateUniversity([FromBody] UniversityModel universityModel)
        {
            if (universityModel == null || universityModel.UniversityID < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid university model.");
            }

            var data = this.universityService.UpdateUniversity(universityModel);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Deletes the university for given identifier.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="universityID">The university identifier.</param>
        /// <returns>The university deleted confirmation.</returns>
        [Route("DeleteUniversity/{universityID}")]
        [HttpDelete]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage DeleteUniversity(int universityID)
        {
            var data = this.universityService.DeleteUniversity(universityID);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Adds the campus to university.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="universityId">The university identifier.</param>
        /// <param name="campusIds">The campus ids.</param>
        /// <returns>The campus added to university confirmation, or bad request error response if invalid parameters.</returns>
        [Route("AddCampusToUniversity/{universityId}")]
        [HttpPut]
        [ResponseType(typeof(ResponseModel))]
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