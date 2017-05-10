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

    [RoutePrefix("api")]
    public class PiServerController : ApiController
    {
        private readonly IPiServerService piServerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PiServerController"/> class.
        /// </summary>
        public PiServerController()
        {
            this.piServerService = new PiServerService();
        }

        [Route("GetAllPiServers")]
        public HttpResponseMessage GetAllPiServers()
        {
            var data = this.piServerService.GetAllPiServers();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("GetPiServerByID/{piServerID}")]
        public HttpResponseMessage GetPiServerByID(int piServerID)
        {
            var data = this.piServerService.GetPiServerByID(piServerID);

            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Pi Server found"));
        }

        [Route("GetPiServerByName/{piServerName}")]
        public HttpResponseMessage GetPiServerByName(string piServerName)
        {
            if (string.IsNullOrWhiteSpace(piServerName))
            {
                return this.Request.CreateResponse((HttpStatusCode)614, string.Format("Pi Server name cannot be blank"));
            }
            else
            {
                var data = this.piServerService.GetPiServerByName(piServerName);
                if (data != null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, data);
                }

                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Pi Server found"));
            }
        }

        [Route("AddPiServer")]
        [HttpPost]
        public HttpResponseMessage AddPiServer([FromBody] PiServerModel model)
        {
            if (this.ModelState.IsValid)
            {
                var data = this.piServerService.AddPiServer(model);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse((HttpStatusCode)614, messages);
        }

        [Route("UpdatePiServer")]
        [HttpPut]
        public HttpResponseMessage UpdatePiServer([FromBody] PiServerModel model)
        {
            if (this.ModelState.IsValid)
            {
                var data = this.piServerService.UpdatePiServer(model);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse((HttpStatusCode)614, messages);
        }

        [Route("DeletePiServer/{piServerId}")]
        [HttpDelete]
        public HttpResponseMessage DeletePiServer(int piServerId)
        {
            var data = this.piServerService.DeletePiServer(piServerId);
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
                (this.piServerService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}