namespace RestService.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using RestService.Services;
    using RestService.Services.Impl;

    [RoutePrefix("api")]
    public class MeterController : ApiController
    {
        private IMeterService meterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MeterController"/> class.
        /// </summary>
        public MeterController()
        {
            this.meterService = new MeterService();
        }

        [Route("GetMeterList/{buildingId}")]
        public HttpResponseMessage GetMeterList(int buildingId)
        {
            var data = this.meterService.GetMeterList(buildingId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("GetMeterMonthlyConsumption/{buildingId}")]
        public HttpResponseMessage GetMeterMonthlyConsumption(int buildingId)
        {
            var data = this.meterService.GetMeterMonthlyConsumption(buildingId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("GetMeterDailyConsumption/{buildingId}")]
        public HttpResponseMessage GetMeterDailyConsumption(int buildingId)
        {
            var data = this.meterService.GetMeterDailyConsumption(buildingId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("GetMonthWiseConsumption/{buildingId}/{year}")]
        public HttpResponseMessage GetMonthWiseConsumption(int buildingId, int year)
        {
            var data = this.meterService.GetMonthWiseConsumption(buildingId, year);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("GetWeekWiseMonthlyConsumption/{buildingId}/{month}/{year}")]
        public HttpResponseMessage GetWeekWiseMonthlyConsumption(int buildingId, string month, int year)
        {
            var data = this.meterService.GetWeekWiseMonthlyConsumption(buildingId, month, year);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("GetDayWiseMonthlyConsumption/{buildingId}/{month}/{year}")]
        public HttpResponseMessage GetDayWiseMonthlyConsumption(int buildingId, string month, int year)
        {
            var data = this.meterService.GetDayWiseMonthlyConsumption(buildingId, month, year);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("GetDayWiseNextMonthConsumptionPrediction/{buildingId}/{month}/{year}")]
        public HttpResponseMessage GetDayWiseNextMonthConsumptionPrediction(int buildingId, string month, int year)
        {
            var data = this.meterService.GetDayWiseNextMonthPrediction(buildingId, month, year);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("GetDayWiseCurrentMonthConsumptionPrediction/{buildingId}/{month}/{year}")]
        public HttpResponseMessage GetDayWiseCurrentMonthConsumptionPrediction(int buildingId, string month, int year)
        {
            var data = this.meterService.GetDayWiseCurrentMonthPrediction(buildingId, month, year);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                (this.meterService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
