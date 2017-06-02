namespace RestService.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;
    using RestService.Models;
    using RestService.Services;
    using RestService.Services.Impl;

    [RoutePrefix("api")]
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

        /// <summary>
        /// Gets the insight data.
        /// </summary>
        /// <returns>The detail of consumption and prediction of current week.</returns>
        [Route("GetInsightData")]
        [ResponseType(typeof(InsightDataModel))]
        public HttpResponseMessage GetInsightData()
        {
            var data = this.insightService.GetInsightData();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the insight data by building.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <returns>The detail of consumption and prediction of current week.</returns>
        [Route("GetInsightDataByBuilding/{buildingId}")]
        [ResponseType(typeof(InsightDataModel))]
        public HttpResponseMessage GetInsightDataByBuilding(int buildingId)
        {
            var data = this.insightService.GetInsightDataByBuilding(buildingId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the insight data by premise.
        /// </summary>
        /// <param name="premiseID">The premise identifier.</param>
        /// <returns>The detail of consumption and prediction of current week.</returns>
        [Route("GetInsightDataByPremise/{premiseID}")]
        [ResponseType(typeof(InsightDataModel))]
        public HttpResponseMessage GetInsightDataByPremise(int premiseID)
        {
            var data = this.insightService.GetInsightDataByPremise(premiseID);
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
