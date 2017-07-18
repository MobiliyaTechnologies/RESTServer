namespace RestService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using RestService.Models;
    using RestService.Services;
    using RestService.Services.Impl;

    [RoutePrefix("api")]
    public class BuildingController : ApiController
    {
        private readonly IBuildingService buildingService;
        private readonly IRoomService roomService;

        public BuildingController()
        {
            this.buildingService = new BuildingService();
            this.roomService = new RoomService();
        }

        /// <summary>
        /// Gets all building details.
        /// </summary>
        /// <returns>The building details.</returns>
        [Route("GetAllBuildings")]
        [ResponseType(typeof(List<BuildingModel>))]
        public HttpResponseMessage GetAllBuildings()
        {
            var data = this.buildingService.GetAllBuildings();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the building by identifier.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <returns>The building detail if found else not found error response, or bad request error response if invalid building id.</returns>
        [Route("GetBuildingByID/{buildingId}")]
        [ResponseType(typeof(BuildingModel))]
        public HttpResponseMessage GetBuildingByID(int buildingId)
        {
            if (buildingId < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Format("Building id must be grater than zero."));
            }

            var data = this.buildingService.GetBuildingByID(buildingId);
            if (data != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No Building found"));
        }

        /// <summary>
        /// Gets the buildings by premise.
        /// </summary>
        /// <param name="premiseId">The premise identifier.</param>
        /// <returns>The building details.</returns>
        [Route("GetBuildingsByPremise/{premiseId}")]
        [ResponseType(typeof(List<BuildingModel>))]
        public HttpResponseMessage GetBuildingsByPremise(int premiseId)
        {
            var data = this.buildingService.GetBuildingsByPremise(premiseId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the building by location.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>The building detail if found else not found error response, or bad request error response if invalid location details.</returns>
        [Route("GetBuildingByLocation/{latitude}/{longitude}")]
        [ResponseType(typeof(BuildingModel))]
        public HttpResponseMessage GetBuildingByLocation(decimal latitude, decimal longitude)
        {
            if (latitude == default(decimal) || longitude == default(decimal))
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid latitude or longitude");
            }

            var buildings = this.buildingService.GetBuildingByLocation(latitude, longitude);

            if (buildings != null)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, buildings);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Building does not exists for given location, latitude - {0}  longitude - {1}", latitude, longitude));
        }

        /// <summary>
        /// Updates the building.
        /// Building id is required to update building details other fields are optional, passed only fields required to update.
        /// Building name and description are only modifiable fields.
        /// </summary>
        /// <param name="buildingModel">The building model.</param>
        /// <returns>The building updated confirmation, or bad request error response if invalid parameters. </returns>
        [Route("UpdateBuilding")]
        [HttpPut]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage UpdateBuilding([FromBody] BuildingModel buildingModel)
        {
            if (buildingModel == null && buildingModel.BuildingID < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid building model.");
            }

            var data = this.buildingService.UpdateBuilding(buildingModel);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Adds the rooms to building.
        /// It receive only multipart/form-data content-type.
        /// It only accept rooms details in CSV file format.
        /// Sample Format -> RoomName, X, Y.
        /// RoomName - it must be valid string, if already exist than it's ignored.
        /// X - must be valid double value else ignored.
        /// Y - must be valid double value else ignored.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <returns>The rooms added to building confirmation.</returns>
        [HttpPost]
        [Route("AddRoomsToBuilding/{buildingId}")]
        public HttpResponseMessage AddRoomsToBuilding(int buildingId)
        {
            if (buildingId < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Building id must be grater than 0.");
            }

            var roomModels = this.GetRooms().Result;

            if (roomModels.Count() == 0)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid room details.");
            }

            var responseModel = this.roomService.AddRoomsToBuilding(buildingId, roomModels);

            return this.Request.CreateResponse(HttpStatusCode.OK, responseModel);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.buildingService != null)
            {
                (this.buildingService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }

        private async Task<List<RoomModel>> GetRooms()
        {
            // Check if the request contains multipart/form-data.
            if (!this.Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var contents = await this.Request.Content.ReadAsMultipartAsync();

            var fileContent = contents.Contents.FirstOrDefault(c => c.Headers.ContentType != null && c.Headers.ContentType.MediaType.Equals("application/vnd.ms-excel") && c.Headers.ContentLength > 0);

            if (fileContent != null)
            {
                List<RoomModel> rooms;
                using (var roomCSV = await fileContent.ReadAsStreamAsync())
                {
                    rooms = this.GetRoomFromCSV(roomCSV);
                }

                return rooms;
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        private List<RoomModel> GetRoomFromCSV(Stream roomCSV)
        {
            var rooms = new List<RoomModel>();
            using (var reader = new StreamReader(roomCSV))
            {
                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine().Split(',');

                    if (values.Count() == 3)
                    {
                        var roomName = values[0];
                        var x = string.IsNullOrWhiteSpace(values[1]) ? default(double) : Convert.ToDouble(values[1]);
                        var y = string.IsNullOrWhiteSpace(values[2]) ? default(double) : Convert.ToDouble(values[2]);

                        if (x > 0 && y > 0 && !string.IsNullOrWhiteSpace(roomName))
                        {
                            rooms.Add(new RoomModel
                            {
                                RoomName = roomName.Trim(),
                                X = x,
                                Y = y
                            });
                        }
                    }
                }
            }

            return rooms;
        }
    }
}