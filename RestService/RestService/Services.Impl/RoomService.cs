namespace RestService.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Mappings;
    using RestService.Models;
    using RestService.Utilities;

    public sealed class RoomService : IRoomService, IDisposable
    {
        private readonly PowerGridEntities dbContext;
        private readonly IContextInfoAccessorService context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoomService"/> class.
        /// </summary>
        public RoomService()
        {
            this.dbContext = new PowerGridEntities();
            this.context = new ContextInfoAccessorService();
        }

        List<RoomModel> IRoomService.GetAllRooms()
        {
            var roomDetails = this.dbContext.RoomDetail.WhereActiveAccessibleRoom();

            return new RoomModelMapping().Map(roomDetails).ToList();
        }

        List<RoomModel> IRoomService.GetRoomByBuilding(int buildingId)
        {
            var roomDetails = this.dbContext.RoomDetail.WhereActiveAccessibleRoom(b => b.BuildingID == buildingId);

            return new RoomModelMapping().Map(roomDetails).ToList();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            if (this.dbContext != null)
            {
                this.dbContext.Dispose();
            }
        }

        ResponseModel IRoomService.AddRoomsToBuilding(int buildingId, List<RoomModel> roomModels)
        {
            var isAccessibleBuilding = this.dbContext.Building.WhereActiveAccessibleBuilding(b => b.BuildingID == buildingId).Any();

            if (!isAccessibleBuilding)
            {
                return new ResponseModel(Enums.StatusCode.Error, "User does not have a permission to add rooms in building.");
            }

            var existingRoom = this.dbContext.RoomDetail.WhereActiveAccessibleRoom(b => b.BuildingID == buildingId).Select(r => r.Room_Name);

            roomModels = roomModels.Where(r => !existingRoom.Any(e => e.Equals(r.RoomName, StringComparison.InvariantCultureIgnoreCase))).ToList();

            foreach (var roomModel in roomModels)
            {
                var room = new RoomDetail
                {
                    Room_Name = roomModel.RoomName,
                    X = roomModel.X,
                    Y = roomModel.Y,
                    BuildingID = buildingId
                };

                this.dbContext.RoomDetail.Add(room);
            }

            this.dbContext.SaveChanges();

            return new ResponseModel(Enums.StatusCode.Ok, "Room added to building successfully.");
        }

        ResponseModel IRoomService.DeleteRoom(int roomId)
        {
            var roomDetail = this.dbContext.RoomDetail.WhereActiveAccessibleRoom(b => b.Room_Id == roomId).FirstOrDefault();

            if (roomDetail == null)
            {
                return new ResponseModel(Enums.StatusCode.Error, "Room does not exist or user does not have a permission for this room.");
            }

            this.dbContext.RoomDetail.Remove(roomDetail);
            this.dbContext.SaveChanges();

            return new ResponseModel(Enums.StatusCode.Ok, "Room deleted successfully.");
        }

        ResponseModel IRoomService.UpdateRoom(RoomModel roomModel)
        {
            var roomDetail = this.dbContext.RoomDetail.WhereActiveAccessibleRoom(b => b.Room_Id == roomModel.RoomId).FirstOrDefault();

            if (roomDetail == null)
            {
                return new ResponseModel(Enums.StatusCode.Error, "Room does not exist or user does not have a permission for this room.");
            }

            roomDetail.Room_Name = string.IsNullOrWhiteSpace(roomModel.RoomName) ? roomDetail.Room_Name : roomModel.RoomName;
            roomDetail.X = roomModel.X == default(double) ? roomDetail.X : roomModel.X;
            roomDetail.Y = roomModel.Y == default(double) ? roomDetail.Y : roomModel.Y;

            this.dbContext.SaveChanges();

            return new ResponseModel(Enums.StatusCode.Ok, "Room updated successfully.");
        }
    }
}