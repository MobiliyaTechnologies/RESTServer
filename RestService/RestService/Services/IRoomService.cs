namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Models;

    /// <summary>
    /// Provides room related operation.
    /// </summary>
    public interface IRoomService
    {
        /// <summary>
        /// Gets all rooms.
        /// </summary>
        /// <returns>All rooms data</returns>
        List<RoomModel> GetAllRooms();

        /// <summary>
        /// Gets all building's rooms.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <returns>
        /// The building's rooms.
        /// </returns>
        List<RoomModel> GetRoomByBuilding(int buildingId);

        /// <summary>
        /// Adds the rooms to building.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <param name="roomModels">The room models.</param>
        /// <returns>The rooms added confirmations.</returns>
        ResponseModel AddRoomsToBuilding(int buildingId, List<RoomModel> roomModels);

        /// <summary>
        /// Deletes the room.
        /// </summary>
        /// <param name="roomId">The room identifier.</param>
        /// <returns>The room deleted confirmation.</returns>
        ResponseModel DeleteRoom(int roomId);

        /// <summary>
        /// Updates the room.
        /// </summary>
        /// <param name="roomModel">The room model.</param>
        /// <returns></returns>
        ResponseModel UpdateRoom(RoomModel roomModel);
    }
}
