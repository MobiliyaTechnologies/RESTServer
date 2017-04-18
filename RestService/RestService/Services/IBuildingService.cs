namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Models;

    public interface IBuildingService
    {
        /// <summary>
        /// Get all the Buildings
        /// </summary>
        /// <returns>Returns list of all Buildinges</returns>
        List<BuildingModel> GetAllBuildings();

        /// <summary>
        /// Get a Building by ID
        /// </summary>
        /// <param name="model">Building model</param>
        /// <returns>Returns a specific Building by fetching based on BuildingID</returns>
        BuildingModel GetBuildingByID(BuildingModel model);

        /// <summary>
        /// Inserts a new Building in system
        /// </summary>
        /// <param name="model">Building model</param>
        /// <param name="userId">User</param>
        /// <returns>Insert acknowledgement</returns>
        ResponseModel AddBuilding(BuildingModel model, int userId);

        /// <summary>
        /// Removes an existing Building from system
        /// </summary>
        /// <param name="model">Building model</param>
        /// <param name="userId">User</param>
        /// <returns>Delete acknowledgement</returns>
        ResponseModel DeleteBuilding(BuildingModel model, int userId);

        /// <summary>
        /// Updates information of an existing Building
        /// </summary>
        /// <param name="model">Building model</param>
        /// <param name="userId">User</param>
        /// <returns>Update acknowledgement</returns>
        ResponseModel UpdateBuilding(BuildingModel model, int userId);
    }
}
