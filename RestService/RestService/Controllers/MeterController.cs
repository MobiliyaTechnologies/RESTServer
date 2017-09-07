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

        /// <summary>
        /// Gets the meter list for given building identifier.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <returns>The meter details.</returns>
        [Route("GetMeterList/{buildingId}")]
        [ResponseType(typeof(List<MeterDetailsModel>))]
        public HttpResponseMessage GetMeterList(int buildingId)
        {
            var data = this.meterService.GetMeterList(buildingId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the meter monthly consumption for given building identifier.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <returns>The meter monthly consumption.</returns>
        [Route("GetMeterMonthlyConsumption/{buildingId}")]
        [ResponseType(typeof(List<MeterMonthlyConsumptionModel>))]
        public HttpResponseMessage GetMeterMonthlyConsumption(int buildingId)
        {
            var data = this.meterService.GetMeterMonthlyConsumption(buildingId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the today's meter daily consumptions for given building identifier.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <returns>The meter daily consumption.</returns>
        [Route("GetMeterDailyConsumption/{buildingId}")]
        [ResponseType(typeof(List<MeterDailyConsumptionModel>))]
        public HttpResponseMessage GetMeterDailyConsumption(int buildingId)
        {
            var data = this.meterService.GetMeterDailyConsumption(buildingId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the month wise consumptions for given building identifier and year.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <param name="year">The year.</param>
        /// <returns>The meter's month wise consumption.</returns>
        [Route("GetMonthWiseConsumption/{buildingId}/{year}")]
        [ResponseType(typeof(List<MeterMonthWiseConsumptionModel>))]
        public HttpResponseMessage GetMonthWiseConsumption(int buildingId, int year)
        {
            var data = this.meterService.GetMonthWiseConsumption(buildingId, year);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the week wise monthly consumption for given building identifier, month and year.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <param name="month">The abbreviated month.(e.g - 'jan')</param>
        /// <param name="year">The year.</param>
        /// <returns>The meter's week wise consumption</returns>
        [Route("GetWeekWiseMonthlyConsumption/{buildingId}/{month}/{year}")]
        [ResponseType(typeof(List<MeterWeekWiseMonthlyConsumptionModel>))]
        public HttpResponseMessage GetWeekWiseMonthlyConsumption(int buildingId, string month, int year)
        {
            var data = this.meterService.GetWeekWiseMonthlyConsumption(buildingId, month, year);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the meter day wise consumptions for given building identifier, month and year.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <param name="month">The abbreviated month.(e.g - 'jan')</param>
        /// <param name="year">The year.</param>
        /// <returns>The meter day wise consumption.</returns>
        [Route("GetDayWiseMonthlyConsumption/{buildingId}/{month}/{year}")]
        [ResponseType(typeof(List<MeterDayWiseMonthlyConsumptionModel>))]
        public HttpResponseMessage GetDayWiseMonthlyConsumption(int buildingId, string month, int year)
        {
            var data = this.meterService.GetDayWiseMonthlyConsumption(buildingId, month, year);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the meter day wise next month consumption prediction for given building identifier, month and year.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <param name="month">The abbreviated month.(e.g - 'jan')</param>
        /// <param name="year">The year.</param>
        /// <returns>The meter day wise next month consumption prediction.</returns>
        [Route("GetDayWiseNextMonthConsumptionPrediction/{buildingId}/{month}/{year}")]
        [ResponseType(typeof(List<MeterDayWiseMonthlyConsumptionPredictionModel>))]
        public HttpResponseMessage GetDayWiseNextMonthConsumptionPrediction(int buildingId, string month, int year)
        {
            var data = this.meterService.GetDayWiseNextMonthPrediction(buildingId, month, year);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the meter day wise current month consumption prediction for given building identifier, month and year.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <param name="month">The abbreviated month.(e.g - 'jan')</param>
        /// <param name="year">The year.</param>
        /// <returns>The meter day wise current month consumption prediction.</returns>
        [Route("GetDayWiseCurrentMonthConsumptionPrediction/{buildingId}/{month}/{year}")]
        [ResponseType(typeof(List<MeterDayWiseMonthlyConsumptionPredictionModel>))]
        public HttpResponseMessage GetDayWiseCurrentMonthConsumptionPrediction(int buildingId, string month, int year)
        {
            var data = this.meterService.GetDayWiseCurrentMonthPrediction(buildingId, month, year);
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
                (this.meterService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
