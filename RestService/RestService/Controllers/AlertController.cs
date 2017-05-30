namespace RestService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;
    using RestService.Models;
    using RestService.Services;
    using RestService.Services.Impl;

    [RoutePrefix("api")]
    public class AlertController : ApiController
    {
        private IAlertService alertService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertController"/> class.
        /// </summary>
        public AlertController()
        {
            this.alertService = new AlertService();
        }

        /// <summary>
        /// Gets all alerts.
        /// </summary>
        /// <returns>The alerts.</returns>
        [Route("GetAllAlerts")]
        [ResponseType(typeof(List<AlertModel>))]
        public HttpResponseMessage GetAllAlerts()
        {
            var data = this.alertService.GetAllAlerts();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the alert details for given sensor log id.
        /// </summary>
        /// <param name="sensorLogId">The sensor log identifier.</param>
        /// <returns>The alert model if found else a not found error response. </returns>
        [Route("GetAlertDetails/{sensorLogId}")]
        [ResponseType(typeof(AlertModel))]
        public HttpResponseMessage GetAlertDetails(int sensorLogId)
        {
            var data = this.alertService.GetAlertDetails(sensorLogId);

            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Alert does not exists for given sensor - {0}", sensorLogId));
        }

        /// <summary>
        /// Acknowledges the alert.
        /// </summary>
        /// <param name="alertDetail">The alert detail.</param>
        /// <returns>The acknowledgment confirmation.</returns>
        [Route("AcknowledgeAlert")]
        [HttpPut]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage AcknowledgeAlert([FromBody] AlertModel alertDetail)
        {
            if (alertDetail != null && this.ModelState.IsValid)
            {
                var data = this.alertService.AcknowledgeAlert(alertDetail);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please enter User name for Acknowledgment");
        }

        /// <summary>
        /// Gets the recommendation type alerts.
        /// </summary>
        /// <returns>The recommendation's alerts.</returns>
        [Route("GetRecommendations")]
        [ResponseType(typeof(List<AlertModel>))]
        public HttpResponseMessage GetRecommendations()
        {
            var data = this.alertService.GetRecommendations();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.alertService != null)
            {
                (this.alertService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}