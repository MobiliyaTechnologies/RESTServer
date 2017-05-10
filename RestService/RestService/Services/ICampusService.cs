namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Models;

    /// <summary>
    /// Provides campus related operations.
    /// </summary>
    public interface ICampusService
    {
        /// <summary>
        /// Get all the campus
        /// </summary>
        /// <returns>Returns list of all campuses</returns>
        List<CampusModel> GetAllCampus();

        /// <summary>
        /// Get a Campus by ID
        /// </summary>
        /// <param name="campusId">The campus identifier.</param>
        /// <returns>
        /// Returns a specific Campus by fetching based on CampusID
        /// </returns>
        CampusModel GetCampusByID(int campusId);

        /// <summary>
        /// Gets the campus accessible to current user.
        /// </summary>
        /// <returns>Campus list associate with current user role.</returns>
        List<CampusModel> GetCampus();

        /// <summary>
        /// Gets the campus by location.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>The campus situated at given location.</returns>
        CampusModel GetCampusByLocation(decimal latitude, decimal longitude);

        /// <summary>
        /// Inserts a new Campus in system
        /// </summary>
        /// <param name="model">Campus model</param>
        /// <returns>
        /// Insert acknowledgment.
        /// </returns>
        ResponseModel AddCampus(CampusModel model);

        /// <summary>
        /// Removes an existing Campus from system
        /// </summary>
        /// <param name="campusId">The campus identifier.</param>
        /// <returns>
        /// Delete acknowledgment.
        /// </returns>
        ResponseModel DeleteCampus(int campusId);

        /// <summary>
        /// Updates information of an existing Campus
        /// </summary>
        /// <param name="model">Campus model</param>
        /// <returns>Update acknowledgment</returns>
        ResponseModel UpdateCampus(CampusModel model);

        /// <summary>
        /// Assigns the roles to campus.
        /// </summary>
        /// <param name="roleIds">The role ids.</param>
        /// <param name="campusId">The campus identifier.</param>
        /// <returns>
        /// Roles Assigned acknowledgment.
        /// </returns>
        ResponseModel AssignRolesToCampus(List<int> roleIds, int campusId);

        /// <summary>
        /// Adds the buildings to campus.
        /// </summary>
        /// <param name="campusId">The campus identifier.</param>
        /// <param name="buildingIds">The building ids.</param>
        /// <returns>Association confirmation.</returns>
        ResponseModel AddBuildingsToCampus(int campusId, List<int> buildingIds);
    }
}
