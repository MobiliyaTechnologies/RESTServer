﻿namespace RestService.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
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

        [Route("GetAllSensors")]
        public HttpResponseMessage GetAllSensors()
        {
            var data = this.sensorService.GetAllSensors();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("MapSensor/{sensorId}/{classId}")]
        [HttpPut]
        public HttpResponseMessage MapSensor(int sensorId, int classId)
        {
            if (sensorId < 1 || classId < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid sensor id or class id");
            }

            var data = this.sensorService.MapSensor(sensorId, classId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("GetAllSensorsForClass/{classId}")]
        public HttpResponseMessage GetAllSensorsForClass(int classId)
        {
            if (classId < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid class id.");
            }

            var data = this.sensorService.GetAllSensorsForClass(classId);

            if (data != null && data.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Sensors does not mapped to given class id - {0}", classId));
        }

        [Route("ResetSensors")]
        [HttpDelete]
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
