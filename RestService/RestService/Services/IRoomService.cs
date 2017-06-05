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
    }
}
