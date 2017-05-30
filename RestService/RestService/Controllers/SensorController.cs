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

        /// <summary>
        /// Gets all sensors.
        /// </summary>
        /// <returns>The sensor details.</returns>
        [Route("GetAllSensors")]
        [ResponseType(typeof(List<SensorModel>))]
        public HttpResponseMessage GetAllSensors()
        {
            var data = this.sensorService.GetAllSensors();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets all sensors mapped to class.
        /// </summary>
        /// <returns>The sensor details.</returns>
        [Route("GetAllMapSensors")]
        [ResponseType(typeof(List<SensorModel>))]
        public HttpResponseMessage GetAllMapSensors()
        {
            var data = this.sensorService.GetAllMapSensors();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets all sensors unmapped to class.
        /// </summary>
        /// <returns>The sensor details.</returns>
        [Route("GetAllUnMapSensors")]
        [ResponseType(typeof(List<SensorModel>))]
        public HttpResponseMessage GetAllUnMapSensors()
        {
            var data = this.sensorService.GetAllUnMapSensors();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Maps the sensor to class.
        /// </summary>
        /// <param name="sensorId">The sensor identifier.</param>
        /// <param name="classId">The class identifier.</param>
        /// <returns>The sensor class mapping confirmation, or bad request error response if invalid parameters.</returns>
        [Route("MapSensor/{sensorId}/{classId}")]
        [HttpPut]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage MapSensor(int sensorId, int classId)
        {
            if (sensorId < 1 || classId < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid sensor id or class id");
            }

            var data = this.sensorService.MapSensor(sensorId, classId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets all sensors for class.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <returns>The sensor details, or bad request error response if invalid parameters.</returns>
        [Route("GetAllSensorsForClass/{classId}")]
        [ResponseType(typeof(List<SensorModel>))]
        public HttpResponseMessage GetAllSensorsForClass(int classId)
        {
            if (classId < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid class id.");
            }

            var data = this.sensorService.GetAllSensorsForClass(classId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Resets the sensors.
        /// </summary>
        /// <returns>The sensor reset confirmation.</returns>
        [Route("ResetSensors")]
        [HttpDelete]
        [ResponseType(typeof(ResponseModel))]
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
