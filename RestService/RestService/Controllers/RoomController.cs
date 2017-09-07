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
    public class RoomController : ApiController
    {
        private IRoomService roomService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomController"/> class.
        /// </summary>
        public RoomController()
        {
            this.roomService = new RoomService();
        }

        /// <summary>
        /// Gets all rooms.
        /// </summary>
        /// <returns>The rooms detail.</returns>
        [Route("GetAllRooms")]
        [ResponseType(typeof(List<RoomModel>))]
        public HttpResponseMessage GetAllRooms()
        {
            var data = this.roomService.GetAllRooms();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the rooms by building.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <returns>The room details.</returns>
        [Route("GetRoomByBuilding/{buildingId}")]
        [ResponseType(typeof(List<RoomModel>))]
        public HttpResponseMessage GetRoomByBuilding(int buildingId)
        {
            if (buildingId < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Building id must be grater than 0.");
            }

            var data = this.roomService.GetRoomByBuilding(buildingId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Deletes the room.
        /// </summary>
        /// <param name="roomId">The room identifier.</param>
        /// <returns>The room deleted confirmation.</returns>
        [HttpDelete]
        [Route("DeleteRoom/{roomId}")]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage DeleteRoom(int roomId)
        {
            if (roomId < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Room id must be grater than 0.");
            }

            var responseModel = this.roomService.DeleteRoom(roomId);

            return this.Request.CreateResponse(HttpStatusCode.OK, responseModel);
        }

        /// <summary>
        /// Updates the room.
        /// </summary>
        /// <param name="roomModel">The room model.</param>
        /// <returns>The room updated confirmation.</returns>
        [HttpPut]
        [Route("UpdateRoom")]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage UpdateRoom(RoomModel roomModel)
        {
            if (roomModel.RoomId < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Room id must be grater than 0.");
            }

            var responseModel = this.roomService.UpdateRoom(roomModel);

            return this.Request.CreateResponse(HttpStatusCode.OK, responseModel);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.roomService != null)
            {
                (this.roomService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
