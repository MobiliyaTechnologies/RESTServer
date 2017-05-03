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
            if (data != null && data.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Meter does not associated with given building id - {0}", buildingId));
        }

        [Route("GetMeterMonthlyConsumption/{buildingId}")]
        public HttpResponseMessage GetMeterMonthlyConsumption(int buildingId)
        {
            var data = this.meterService.GetMeterMonthlyConsumption(buildingId);
            if (data != null && data.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Month wise consumption of electricity does not exists for given building id - {0}", buildingId));
        }

        [Route("GetMeterDailyConsumption/{buildingId}")]
        public HttpResponseMessage GetMeterDailyConsumption(int buildingId)
        {
            var data = this.meterService.GetMeterDailyConsumption(buildingId);
            if (data != null && data.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Day wise consumption of electricity does not exists for given building id - {0}", buildingId));
        }

        [Route("GetMonthWiseConsumption/{buildingId}/{year}")]
        public HttpResponseMessage GetMonthWiseConsumption(int buildingId, int year)
        {
            var data = this.meterService.GetMonthWiseConsumption(buildingId, year);

            if (data != null && data.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Month wise consumption of electricity does not exists for given year - {0}, building id - {1}", year, buildingId));
        }

        [Route("GetWeekWiseMonthlyConsumption/{buildingId}/{month}/{year}")]
        public HttpResponseMessage GetWeekWiseMonthlyConsumption(int buildingId, string month, int year)
        {
            var data = this.meterService.GetWeekWiseMonthlyConsumption(buildingId, month, year);

            if (data != null && data.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format(
                "Week wise monthly consumption of electricity does not exists for given month - {0}, year - {1}, building id - {2}", month, year, buildingId));
        }
       
        [Route("GetDayWiseMonthlyConsumption/{buildingId}/{month}/{year}")]
        public HttpResponseMessage GetDayWiseMonthlyConsumption(int buildingId, string month, int year)
        {
            var data = this.meterService.GetDayWiseMonthlyConsumption(buildingId, month, year);

            if (data != null && data.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format(
                "Day wise monthly consumption of electricity does not exists for given month - {0}, year - {1}, building id - {2}", month, year, buildingId));
        }

        [Route("GetDayWiseNextMonthConsumptionPrediction/{buildingId}/{month}/{year}")]
        public HttpResponseMessage GetDayWiseNextMonthConsumptionPrediction(int buildingId, string month, int year)
        {
            var data = this.meterService.GetDayWiseNextMonthPrediction(buildingId, month, year);

            if (data != null && data.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format(
                "Day wise next monthly prediction of electricity consumption does not exists for given month - " + "{0}, year {1}, building id - {2}", month, year, buildingId));
        }

        [Route("GetDayWiseCurrentMonthConsumptionPrediction/{buildingId}/{month}/{year}")]
        public HttpResponseMessage GetDayWiseCurrentMonthConsumptionPrediction(int buildingId, string month, int year)
        {
            var data = this.meterService.GetDayWiseCurrentMonthPrediction(buildingId, month, year);

            if (data != null && data.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format(
                "Day wise monthly prediction of electricity consumption does not exists for given month - " + "{0}, year - {1}, building id - {2}", month, year, buildingId));
        }

        //[Route("GetMonthWiseConsumptionForOffset/{buildingId}/{month}/{year}/{offset}")]
        //public HttpResponseMessage GetMonthWiseConsumptionForOffset(int buildingId, string month, int year, int offset)
        //{
        //    var data = this.meterService.GetMonthWiseConsumptionForOffset(buildingId, month, year, offset);

        //    if (data != null && data.Count() > 0)
        //    {
        //        return this.Request.CreateResponse(HttpStatusCode.OK, data);
        //    }

        //    return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format(
        //        "Month wise consumption of electricity does not exists for given month - {0}, year - {1}, offset - {2}, building id - {3}", month, year, offset, buildingId));
        //}

        //[Route("GetWeekWiseMonthlyConsumptionForOffset/{buildingId}/{month}/{year}/{offset}")]
        //public HttpResponseMessage GetWeekWiseMonthlyConsumptionForOffset(int buildingId, string month, int year, int offset)
        //{
        //    var data = this.meterService.GetWeekWiseMonthlyConsumptionForOffset(buildingId, month, year, offset);

        //    if (data != null && data.Count() > 0)
        //    {
        //        return this.Request.CreateResponse(HttpStatusCode.OK, data);
        //    }

        //    return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format(
        //        "Week wise monthly consumption of electricity does not exists for given month -{0}, year - {1}, offset -{2}, building id - {3}", month, year, offset, buildingId));
        //}

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
