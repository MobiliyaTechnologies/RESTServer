namespace RestService.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using RestService.Models;
    using RestService.Services;
    using RestService.Services.Impl;

    public class SensorController : ApiController
    {
        private ISensorService sensorService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SensorController" /> class.
        /// </summary>
        public SensorController()
        {
            this.sensorService = new SensorService();
        }

        [Route("api/getallsensors")]
        public HttpResponseMessage GetAllSensors()
        {
            var data = this.sensorService.GetAllSensors();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("api/mapsensortoclass")]
        [HttpPost]
        public HttpResponseMessage MapSensor([FromBody] SensorModel sensorDetail)
        {
            if (sensorDetail != null && sensorDetail.Class_Id.HasValue && sensorDetail.Class_Id.Value > 0)
            {
                var data = this.sensorService.MapSensor(sensorDetail);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            var errorMessage = sensorDetail == null ? "Invalid sensor model" : "Please enter Class Id to map sensor";
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, errorMessage);
        }

        [Route("api/getallsensorsforclass")]
        [HttpPost]
        public HttpResponseMessage GetAllSensorsForClass([FromBody] SensorModel sensorData)
        {
            if (sensorData == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid sensor model");
            }

            var data = this.sensorService.GetAllSensorsForClass(sensorData);

            if (data != null && data.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Sensors does not mapped to given class id - {0}", sensorData.Class_Id));
        }

        [Route("api/resetsensors")]
        [HttpGet]
        public HttpResponseMessage ResetSensors()
        {
            var data = this.sensorService.ResetSensors();
            return this.Request.CreateResponse(HttpStatusCode.OK, data.Message);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                (this.sensorService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
