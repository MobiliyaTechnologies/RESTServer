namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Models;

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
        /// <param name="campusID">Campus ID</param>
        /// <returns>Returns a specific Campus by fetching based on CampusID</returns>
        CampusModel GetCampusByID(int campusID);

        /// <summary>
        /// Inserts a new Campus in system
        /// </summary>
        /// <param name="model">Campus model</param>
        /// <param name="userId">User</param>
        /// <returns>Insert acknowledgement</returns>
        ResponseModel AddCampus(CampusModel model, int userId);

        /// <summary>
        /// Removes an existing Campus from system
        /// </summary>
        /// <param name="model">Campus model</param>
        /// <param name="userId">User</param>
        /// <returns>Delete acknowledgement</returns>
        ResponseModel DeleteCampus(CampusModel model, int userId);

        /// <summary>
        /// Updates information of an existing Campus
        /// </summary>
        /// <param name="model">Campus model</param>
        /// <param name="userId">User</param>
        /// <returns>Update acknowledgement</returns>
        ResponseModel UpdateCampus(CampusModel model, int userId);
    }
}
