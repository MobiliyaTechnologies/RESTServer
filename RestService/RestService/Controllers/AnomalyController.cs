namespace RestService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;
    using RestService.Services;
    using RestService.Services.Impl;

    [RoutePrefix("api")]
    public class AnomalyController : ApiController
    {
        private IAnomalyService anomalyService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnomalyController"/> class.
        /// </summary>
        public AnomalyController()
        {
            this.anomalyService = new AnomalyService();
        }

        /// <summary>
        /// Gets the anomaly details for given date.
        /// </summary>
        /// <param name="timestamp">The date.</param>
        /// <returns>The anomaly info models.</returns>
        [Route("GetAnomalyDetailsByDay/{timestamp}")]
        [ResponseType(typeof(List<Models.AnomalyInfoModel>))]
        public HttpResponseMessage GetAnomalyDetailsByDay(string timestamp)
        {
            var data = this.anomalyService.GetAnomalyDetails(timestamp);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.anomalyService != null)
            {
                (this.anomalyService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
