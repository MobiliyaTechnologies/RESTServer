namespace RestService.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using RestService.Services;
    using RestService.Services.Impl;

    public class InsightController : ApiController
    {
        private IInsightService insightService;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsightController"/> class.
        /// </summary>
        public InsightController()
        {
            this.insightService = new InsightService();
        }

        [Route("api/getinsightdata")]
        public HttpResponseMessage GetInsightData()
        {
            var data = this.insightService.GetInsightData();
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
                (this.insightService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
