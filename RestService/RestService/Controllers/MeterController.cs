namespace RestService.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using RestService.Services;
    using RestService.Services.Impl;

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

        [Route("api/getmeterlist")]
        public HttpResponseMessage GetMeterList()
        {
            var data = this.meterService.GetMeterList();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("api/getmonthlyconsumption")]
        public HttpResponseMessage GetMeterMonthlyConsumption()
        {
            var data = this.meterService.GetMeterMonthlyConsumption();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("api/getdailyconsumption")]
        public HttpResponseMessage GetMeterDailyConsumption()
        {
            var data = this.meterService.GetMeterDailyConsumption();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }


        [Route("api/getmonthwiseconsumption/{Year}")]
        public HttpResponseMessage GetMonthWiseConsumption(int year)
        {
            var data = this.meterService.GetMonthWiseConsumption(year);

            if (data != null && data.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Month wise consumption of electricity does not exists for given year - {0}", year));
        }

        [Route("api/getmonthwiseconsumptionforoffset/{Month}/{Year}/{Offset}")]
        public HttpResponseMessage GetMonthWiseConsumptionForOffset(string month, int year, int offset)
        {
            var data = this.meterService.GetMonthWiseConsumptionForOffset(month, year, offset);

            if (data != null && data.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format(
                "Month wise consumption of electricity does not exists for given month - {0}, year - {1}, offset - {2}", month, year, offset));
        }

        [Route("api/getweekwisemonthlyconsumption/{Month}/{Year}")]
        public HttpResponseMessage GetWeekWiseMonthlyConsumption( string month, int year)
        {
            var data = this.meterService.GetWeekWiseMonthlyConsumption(month, year);

            if (data != null && data.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format(
                "Week wise monthly consumption of electricity does not exists for given month - {0}, year - {1}", month, year));
        }

        [Route("api/getweekwisemonthlyconsumptionforoffset/{Month}/{Year}/{Offset}")]
        public HttpResponseMessage GetWeekWiseMonthlyConsumptionForOffset( string month, int year, int offset)
        {
            var data = this.meterService.GetWeekWiseMonthlyConsumptionForOffset(month, year, offset);

            if (data != null && data.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format(
                "Week wise monthly consumption of electricity does not exists for given month -{0}, year - {1}, offset -{2}", month, year, offset));

        }

        [Route("api/getdaywisemonthlyconsumption/{Month}/{Year}")]
        public HttpResponseMessage GetDayWiseMonthlyConsumption( string month, int year)
        {
            var data = this.meterService.GetDayWiseMonthlyConsumption(month, year);

            if (data != null && data.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format(
                "Day wise monthly consumption of electricity does not exists for given month - {0}, year - {1}", month, year));
        }

        [Route("api/getdaywisenextmonthprediction/{Month}/{Year}")]
        public HttpResponseMessage GetDayWiseNextMonthConsumptionPrediction(string month, int year)
        {
            var data = this.meterService.GetDayWiseNextMonthPrediction(month, year);

            if (data != null && data.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format(
                "Day wise next monthly prediction of electricity consumption does not exists for given month - " + "{0}, year {1}", month, year));
        }

        [Route("api/getdaywisecurrentmonthprediction/{Month}/{Year}")]
        public HttpResponseMessage GetDayWiseCurrentMonthConsumptionPrediction( string month, int year)
        {
            var data = this.meterService.GetDayWiseCurrentMonthPrediction(month, year);

            if (data != null && data.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format(
                "Day wise monthly prediction of electricity consumption does not exists for given month - " + "{0}, year - {1}", month, year));
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
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
