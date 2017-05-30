﻿namespace RestService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Newtonsoft.Json.Linq;
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

        /// <summary>
        /// Gets all pi servers.
        /// </summary>
        /// <returns>The PiServer details.</returns>
        [Route("GetAllPiServers")]
        [ResponseType(typeof(List<PiServerModel>))]
        public HttpResponseMessage GetAllPiServers()
        {
            var data = this.piServerService.GetAllPiServers();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the pi server by identifier.
        /// </summary>
        /// <param name="piServerID">The pi server identifier.</param>
        /// <returns>The PiServer detail if found else not found error message.</returns>
        [Route("GetPiServerByID/{piServerID}")]
        [ResponseType(typeof(PiServerModel))]
        public HttpResponseMessage GetPiServerByID(int piServerID)
        {
            var data = this.piServerService.GetPiServerByID(piServerID);

            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Pi Server found"));
        }

        /// <summary>
        /// Gets the pi server by it's unique name.
        /// </summary>
        /// <param name="piServerName">Name of the pi server.</param>
        /// <returns>The PiServer detail if found else not found error response, or bad request error response if invalid PiServer name.</returns>
        [Route("GetPiServerByName/{piServerName}")]
        [ResponseType(typeof(PiServerModel))]
        public HttpResponseMessage GetPiServerByName(string piServerName)
        {
            if (string.IsNullOrWhiteSpace(piServerName))
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, string.Format("Pi Server name cannot be blank"));
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

        /// <summary>
        /// Adds the pi server.
        /// It receive only multipart/form-data content-type.
        /// Required fields - CampusId, PiServerName(must be unique), PiServerURL and CSV campus schedule file
        /// Sample request -
        /// {  "PiServerID": 0,  "PiServerName": "", "PiServerDesc": "",  "CampusID": 1,  "PiServerURL": ""}
        /// Campus schedule file with any name but must be CSV type.
        /// </summary>
        /// <returns>The PiServer added confirmation, or bad request error response if invalid parameters.</returns>
        [Route("AddPiServer")]
        [HttpPost]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage AddPiServer()
        {
            var model = this.GetPiServerModelFromRequest().Result;

            if (model.CampusID == 0 || string.IsNullOrWhiteSpace(model.PiServerName) || string.IsNullOrWhiteSpace(model.PiServerURL) || model.CampusScheduleFile == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Missing required fields - CampusId, PiServerName, PiServerURL or CSV campus schedule file.");
            }

            var data = this.piServerService.AddPiServer(model);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Updates the pi server.
        /// It receive only multipart/form-data content-type.
        /// Required fields - PiServerID and fields to update.
        /// Sample request -
        /// {  "PiServerID": 0,  "PiServerName": "", "PiServerDesc": "",  "CampusID": 1,  "PiServerURL": ""}
        /// Campus schedule file with any name but must be CSV type, it's optional post only to update existing campus schedule.
        /// </summary>
        /// <returns>The PiServer updated confirmation, or bad request error response if invalid parameters.</returns>
        [Route("UpdatePiServer")]
        [HttpPut]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage UpdatePiServer()
        {
            var model = this.GetPiServerModelFromRequest().Result;

            if (model.PiServerID == 0)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "PiServerId must be grater than zero.");
            }

            var data = this.piServerService.UpdatePiServer(model);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Deletes the pi server for given PiServer identifier.
        /// </summary>
        /// <param name="piServerId">The pi server identifier.</param>
        /// <returns>The PiServer deleted confirmation</returns>
        [Route("DeletePiServer/{piServerId}")]
        [HttpDelete]
        [ResponseType(typeof(ResponseModel))]
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

        private async Task<PiServerModel> GetPiServerModelFromRequest()
        {
            // Check if the request contains multipart/form-data.
            if (!this.Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var piServerModel = new PiServerModel();

            var contents = await this.Request.Content.ReadAsMultipartAsync();

            var fileContent = contents.Contents.FirstOrDefault(c => c.Headers.ContentType != null && c.Headers.ContentType.MediaType.Equals("application/vnd.ms-excel") && c.Headers.ContentLength > 0);
            var formDataContents = contents.Contents.Where(c => c.Headers.ContentType == null || (c.Headers.ContentDisposition != null && c.Headers.ContentDisposition.FileName == null));

            if (fileContent != null)
            {
                piServerModel.CampusScheduleFile = await fileContent.ReadAsStreamAsync();
                piServerModel.CampusScheduleFileType = fileContent.Headers.ContentType.ToString();
            }

            piServerModel.PiServerName = this.GetFormDataValue(formDataContents, "PiServerName");
            piServerModel.PiServerDesc = this.GetFormDataValue(formDataContents, "PiServerDesc");
            piServerModel.PiServerURL = this.GetFormDataValue(formDataContents, "PiServerURL");
            piServerModel.PiServerID = Convert.ToInt32(this.GetFormDataValue(formDataContents, "PiServerID"));
            piServerModel.CampusID = Convert.ToInt32(this.GetFormDataValue(formDataContents, "campusId"));

            return piServerModel;
        }

        private string GetFormDataValue(IEnumerable<HttpContent> formDataContents, string name)
        {
            var formDataContent = formDataContents.FirstOrDefault(f => f.Headers.ContentDisposition.Name.Trim('"').Equals(name, StringComparison.InvariantCultureIgnoreCase));

            return formDataContent != null ? formDataContent.ReadAsStringAsync().Result : null;
        }
    }
}