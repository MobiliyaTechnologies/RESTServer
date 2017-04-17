namespace RestService.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using RestService.Services;
    using RestService.Services.Impl;

    public class PowerBIController : ApiController
    {
        private IPowerBIService powerBIService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerBIController" /> class.
        /// </summary>
        public PowerBIController()
        {
            this.powerBIService = new PowerBIService();
        }

        [Route("api/getpowerbiurl/{MeterSerial}")]
        public HttpResponseMessage GetPowerBIURL( string meterSerial)
        {
            var data = this.powerBIService.GetPowerBIUrl(meterSerial);

            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Power BI url does not exists for given meter serial - {0}", meterSerial));
        }

        [Route("api/getpowerbigeneralurl")]
        public HttpResponseMessage GetPowerBIGeneralURL()
        {
            var data = this.powerBIService.GetPowerBIGeneralURL();
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
                (this.powerBIService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
