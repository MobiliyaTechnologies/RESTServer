﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RestService.Services;
using RestService.Services.Impl;

namespace RestService.Controllers
{
    public class AnomalyController : ApiController
    {
        private IAnomalyService anomalyService;

        public AnomalyController()
        {
            this.anomalyService = new AnomalyService();
        }


        [Route("api/getAnomalyDetailsByDay/{Timestamp}")]
        public HttpResponseMessage GetAnomalyDetailsByDay(string timestamp)
        {
            var data = this.anomalyService.GetAnomalyDetails(timestamp);

            if (data != null && data.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Anomaly details does not exists for given time stamp  - {0}", timestamp));
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                (this.anomalyService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
