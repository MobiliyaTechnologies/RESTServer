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
    public class PremiseController : ApiController
    {
        private readonly IPremiseService premiseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PremiseController"/> class.
        /// </summary>
        public PremiseController()
        {
            this.premiseService = new PremiseService();
        }

        /// <summary>
        /// Gets all premises.
        /// </summary>
        /// <returns>The Premise Details</returns>
        [Route("GetAllPremise")]
        [ResponseType(typeof(List<PremiseModel>))]
        public HttpResponseMessage GetAllPremises()
        {
            var data = this.premiseService.GetAllPremise();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the premise by identifier.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="premiseID">The premise identifier.</param>
        /// <returns>The premise detail if found else not found error response, or bad request error response if invalid parameters.</returns>
        [Route("GetPremiseByID/{premiseID}")]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        [ResponseType(typeof(PremiseModel))]
        public HttpResponseMessage GetPremiseByID(int premiseID)
        {
            if (premiseID < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Premise id musty be grater than 0.");
            }

            var data = this.premiseService.GetPremiseByID(premiseID);
            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Premise found"));
        }

        /// <summary>
        /// Gets the premise by location.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>The premise detail if found else not found error response, or bad request error response if invalid parameters.</returns>
        [Route("GetPremiseByLocation/{latitude}/{longitude}")]
        [ResponseType(typeof(PremiseModel))]
        public HttpResponseMessage GetPremiseByLocation(decimal latitude, decimal longitude)
        {
            if (latitude == default(decimal) || longitude == default(decimal))
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid latitude or longitude");
            }

            var premise = this.premiseService.GetPremiseByLocation(latitude, longitude);

            if (premise != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, premise);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Premises does not exists for given location, latitude - {0}  longitude - {1}", latitude, longitude));
        }

        /// <summary>
        /// Adds the premise.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="premiseModel">The premise model.</param>
        /// <returns>The premise created confirmation, or bad request error response if invalid parameters.</returns>
        [Route("AddPremise")]
        [HttpPost]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage AddPremise([FromBody] PremiseModel premiseModel)
        {
            if (premiseModel == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid premise model.");
            }

            if (this.ModelState.IsValid)
            {
                var data = this.premiseService.AddPremise(premiseModel);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, messages);
        }

        /// <summary>
        /// Updates the premise.
        /// Premise id is required to update premise other fields are optional, passed only fields required to update.
        /// </summary>
        /// <param name="premiseModel">The premise model.</param>
        /// <returns>The premise update confirmation, or bad request error response if invalid parameters.</returns>
        [Route("UpdatePremise")]
        [HttpPut]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage UpdatePremise([FromBody] PremiseModel premiseModel)
        {
            if (premiseModel.PremiseID > 0)
            {
                var data = this.premiseService.UpdatePremise(premiseModel);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Premise id must be grater than zero.");
        }

        /// <summary>
        /// Deletes the premise for given premise identifier.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="premiseID">The premise identifier.</param>
        /// <returns>The premise deleted confirmation, or bad request error response if invalid parameters.</returns>
        [Route("DeletePremise/{premiseID}")]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        [HttpDelete]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage DeletePremise(int premiseID)
        {
            if (premiseID < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Premise id must be grater than zero.");
            }

            var data = this.premiseService.DeletePremise(premiseID);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Assigns the role to premise.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="roleIds">The role ids.</param>
        /// <param name="premiseID">The premise identifier.</param>
        /// <returns>The role assigned confirmation, or bad request error response if invalid parameters.</returns>
        [Route("AssignRoleToPremise/{premiseID}")]
        [HttpPut]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage AssignRoleToPremise(List<int> roleIds, int premiseID)
        {
            if (roleIds == null || roleIds.Any(r => r < 1) || premiseID < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid role id or premise id");
            }

            var responseModel = this.premiseService.AssignRolesToPremise(roleIds, premiseID);
            return this.Request.CreateResponse(HttpStatusCode.OK, responseModel);
        }

        /// <summary>
        /// Adds the buildings to premise.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="premiseID">The premise identifier.</param>
        /// <param name="buildingIds">The building ids.</param>
        /// <returns>The building assigned confirmation, or bad request error response if invalid parameters.</returns>
        [Route("AddBuildingsToPremise/{premiseID}")]
        [HttpPut]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage AddBuildingsToPremise(int premiseID, [FromBody] List<int> buildingIds)
        {
            if (premiseID < 1 || buildingIds == null || buildingIds.Count() == 0)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid premise id or building ids");
            }

            var responseModel = this.premiseService.AddBuildingsToPremise(premiseID, buildingIds);
            return this.Request.CreateResponse(HttpStatusCode.OK, responseModel);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.premiseService != null)
            {
                (this.premiseService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}