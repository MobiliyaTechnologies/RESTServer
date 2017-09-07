namespace RestService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;
    using RestService.Enums;
    using RestService.Filters;
    using RestService.Models;
    using RestService.Services;
    using RestService.Services.Impl;

    [RoutePrefix("api")]
    public class ApplicationConfigurationController : ApiController
    {
        private IApplicationConfigurationService applicationConfigurationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationConfigurationController"/> class.
        /// </summary>
        public ApplicationConfigurationController()
        {
            this.applicationConfigurationService = new ApplicationConfigurationService();
        }

        /// <summary>
        /// Gets all application configuration.
        /// </summary>
        /// <returns>The application configueations.</returns>
        [Route("GetAllApplicationConfiguration")]
        [ResponseType(typeof(List<ApplicationConfigurationModel>))]
        public HttpResponseMessage GetAllApplicationConfiguration()
        {
            var appConfigs = this.applicationConfigurationService.GetAllApplicationConfiguration();
            return this.Request.CreateResponse(HttpStatusCode.OK, appConfigs);
        }

        /// <summary>
        /// Gets the application configuration for given configuration type.
        /// </summary>
        /// <param name="applicationConfigurationType">Type of the application configuration.</param>
        /// <returns>The application configuration if fount else a not found error response.</returns>
        [Route("GetApplicationConfiguration/{applicationConfigurationType}")]
        [ResponseType(typeof(ApplicationConfigurationModel))]
        public HttpResponseMessage GetApplicationConfiguration(string applicationConfigurationType)
        {
            var appConfig = this.applicationConfigurationService.GetApplicationConfiguration(applicationConfigurationType);

            if (appConfig != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, appConfig);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Configuration does not exists for type - {0}.", applicationConfigurationType.ToString()));
        }

        /// <summary>
        /// Adds the application configuration.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="applicationConfigurationModel">The application configuration model.</param>
        /// <returns>The application configuration creation confirmation, or bad request error response if invalid input parameters.</returns>
        [Route("AddApplicationConfiguration")]
        [HttpPost]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage AddApplicationConfiguration(ApplicationConfigurationModel applicationConfigurationModel)
        {
            if (applicationConfigurationModel == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid application configuration model.");
            }

            if (this.ModelState.IsValid && applicationConfigurationModel.ApplicationConfigurationEntries.Count > 0)
            {
                var data = this.applicationConfigurationService.AddApplicationConfiguration(applicationConfigurationModel);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            if (applicationConfigurationModel.ApplicationConfigurationEntries.Count == 0)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Application configuration entries must required.");
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, messages);
        }

        /// <summary>
        /// Deletes the application configuration for given configuration type.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="applicationConfigurationType">Type of the application configuration.</param>
        /// <returns>The deletion confirmation</returns>
        [Route("DeleteApplicationConfiguration/{applicationConfigurationType}")]
        [HttpDelete]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage DeleteApplicationConfiguration(string applicationConfigurationType)
        {
            var responseModel = this.applicationConfigurationService.DeleteApplicationConfiguration(applicationConfigurationType);
            return this.Request.CreateResponse(HttpStatusCode.OK, responseModel);
        }

        /// <summary>
        /// Updates the application configuration entry.
        /// Id is required to update application configuration entry other fields are optional, passed only fields required to update.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="applicationConfigurationEntryModel">The application configuration entry model.</param>
        /// <returns>The modification confirmation, or bad request error response if invalid input parameters.</returns>
        [Route("UpdateApplicationConfigurationEntry")]
        [HttpPut]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage UpdateApplicationConfigurationEntry(ApplicationConfigurationEntryModel applicationConfigurationEntryModel)
        {
            if (applicationConfigurationEntryModel == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid application configuration entry model.");
            }

            if (applicationConfigurationEntryModel.Id < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Application configuration entry id must be grater than zero.");
            }

            var responseModel = this.applicationConfigurationService.UpdateApplicationConfigurationEntry(applicationConfigurationEntryModel);
            return this.Request.CreateResponse(HttpStatusCode.OK, responseModel);
        }

        /// <summary>
        /// Deletes the application configuration entry for given id.
        /// This API is accessible to only super admin user.
        /// </summary>
        /// <param name="applicationConfigurationEntryId">The application configuration entry identifier.</param>
        /// <returns>The deletion confirmation.</returns>
        [Route("DeleteApplicationConfigurationEntry/{applicationConfigurationEntryId}")]
        [HttpDelete]
        [CustomAuthorize(UserRole = UserRole.SuperAdmin)]
        [OverrideAuthorization]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage DeleteApplicationConfigurationEntry(int applicationConfigurationEntryId)
        {
            if (applicationConfigurationEntryId < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Application configuration entry id must be grater than zero.");
            }

            var responseModel = this.applicationConfigurationService.DeleteApplicationConfigurationEntry(applicationConfigurationEntryId);
            return this.Request.CreateResponse(HttpStatusCode.OK, responseModel);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.applicationConfigurationService != null)
            {
                (this.applicationConfigurationService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
