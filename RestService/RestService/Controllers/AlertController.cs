namespace RestService.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
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

        [Route("GetAllAlerts")]
        public HttpResponseMessage GetAllAlerts()
        {
            var data = this.alertService.GetAllAlerts();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("GetAlertDetails/{sensorLogId}")]
        public HttpResponseMessage GetAlertDetails(int sensorLogId)
        {
            var data = this.alertService.GetAlertDetails(sensorLogId);

            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Alert does not exists for given sensor - {0}", sensorLogId));
        }

        [Route("AcknowledgeAlert")]
        [HttpPut]
        public HttpResponseMessage AcknowledgeAlert([FromBody] AlertModel alertDetail)
        {
            if (alertDetail != null && !string.IsNullOrWhiteSpace(alertDetail.Acknowledged_By))
            {
                var data = this.alertService.AcknowledgeAlert(alertDetail);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please enter User name for Acknowledgment");
        }

        [Route("GetRecommendations")]
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
            if (disposing)
            {
                (this.alertService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}