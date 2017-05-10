namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Models;

    public interface IBuildingService
    {
        /// <summary>
        /// Get all the Buildings
        /// </summary>
        /// <returns>Returns list of all Buildings</returns>
        List<BuildingModel> GetAllBuildings();

        /// <summary>
        /// Get all the Buildings accessible to current user.
        /// </summary>
        /// <returns>
        /// Returns list of all Buildings.
        /// </returns>
        List<BuildingModel> GetBuildings();

        /// <summary>
        /// Get a Building by ID
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <returns>
        /// Returns a specific Building by fetching based on BuildingID
        /// </returns>
        BuildingModel GetBuildingByID(int buildingId);

        /// <summary>
        /// Gets the buildings by campus.
        /// </summary>
        /// <param name="campusId">The campus identifier.</param>
        /// <returns>Buildings associated with given campus id.</returns>
        List<BuildingModel> GetBuildingsByCampus(int campusId);

        /// <summary>
        /// Gets the building by location.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>The building situated at given location.</returns>
        BuildingModel GetBuildingByLocation(decimal latitude, decimal longitude);

        /// <summary>
        /// Inserts a new Building in system
        /// </summary>
        /// <param name="model">Building model</param>
        /// <returns>Insert acknowledgment</returns>
        ResponseModel AddBuilding(BuildingModel model);

        /// <summary>
        /// Removes an existing Building from system
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <returns>
        /// Delete acknowledgment.
        /// </returns>
        ResponseModel DeleteBuilding(int buildingId);

        /// <summary>
        /// Updates information of an existing Building
        /// </summary>
        /// <param name="model">Building model</param>
        /// <returns>
        /// Update acknowledgment.
        /// </returns>
        ResponseModel UpdateBuilding(BuildingModel model);
    }
}
