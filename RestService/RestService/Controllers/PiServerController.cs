namespace RestService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;
    using RestService.Models;
    using RestService.Services;
    using RestService.Services.Impl;
    using RestService.Utilities;

    public class PiServerController : ApiController
    {
        private readonly IPiServerService piServerService;

        public PiServerController()
        {
            this.piServerService = new PiServerService();
        }

        [Route("api/getallpiServers")]
        public HttpResponseMessage GetAllPiServers()
        {
            var data = this.piServerService.GetAllPiServers();
            if (data.Count != 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Pi Server found"));
        }

        [Route("api/getpiserverbyid")]
        [HttpPost]
        public HttpResponseMessage GetPiServerByID([FromBody] PiServerModel model)
        {
            var data = this.piServerService.GetPiServerByID(model);
            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Pi Server found"));
        }

        [Route("api/getpiserverbyname")]
        [HttpPost]
        public HttpResponseMessage GetPiServerByName([FromBody] PiServerModel model)
        {
            if (model.PiServerName == "" || model.PiServerName == null)
            {
                return this.Request.CreateResponse((HttpStatusCode)614, string.Format("Pi Server name cannot be blank"));
            }
            else
            {
                var data = this.piServerService.GetPiServerByName(model);
                if (data != null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, data);
                }

                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Pi Server found"));
            }
        }

        [Route("api/addpiserver")]
        [HttpPost]
        public HttpResponseMessage AddPiServer([FromBody] PiServerModel model)
        {
            if (this.ModelState.IsValid)
            {
                var userId = ServiceUtil.GetUser();
                var data = this.piServerService.AddPiServer(model, userId);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateResponse((HttpStatusCode)614, messages);
        }

        [Route("api/updatepiserver")]
        [HttpPost]
        public HttpResponseMessage UpdatePiServer([FromBody] PiServerModel model)
        {
            if (this.ModelState.IsValid)
            {
                var userId = ServiceUtil.GetUser();
                var data = this.piServerService.UpdatePiServer(model, userId);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateResponse((HttpStatusCode)614, messages);
        }

        [Route("api/deletepiserver")]
        [HttpPost]
        public HttpResponseMessage DeletePiServer([FromBody] PiServerModel model)
        {
            var userId = ServiceUtil.GetUser();
            var data = this.piServerService.DeletePiServer(model, userId);
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