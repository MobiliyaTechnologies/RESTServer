﻿namespace RestService.Controllers
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
    public class ClassroomController : ApiController
    {
        private IClassroomService classroomService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassroomController"/> class.
        /// </summary>
        public ClassroomController()
        {
            this.classroomService = new ClassroomService();
        }

        /// <summary>
        /// Gets all classrooms.
        /// </summary>
        /// <returns>The classroom details.</returns>
        [Route("GetAllClassrooms")]
        [ResponseType(typeof(List<ClassroomModel>))]
        public HttpResponseMessage GetAllClassrooms()
        {
            var data = this.classroomService.GetAllClassrooms();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the classroom by building.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <returns>The classroom details.</returns>
        [Route("GetClassroomByBuilding/{buildingId}")]
        [ResponseType(typeof(List<ClassroomModel>))]
        public HttpResponseMessage GetClassroomByBuilding(int buildingId)
        {
            var data = this.classroomService.GetClassroomByBuilding(buildingId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.classroomService != null)
            {
                (this.classroomService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}