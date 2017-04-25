namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Models;

    public interface IUniversityService
    {
        /// <summary>
        /// Get all the universities
        /// </summary>
        /// <returns>Returns list of all universities</returns>
        List<UniversityModel> GetAllUniversities();

        /// <summary>
        /// Get a university by ID
        /// </summary>
        /// <param name="universityID">University ID</param>
        /// <returns>Returns a specific University by fetching based on UniversityID</returns>
        UniversityModel GetUniversityByID(int universityID);

        /// <summary>
        /// Inserts a new University in system
        /// </summary>
        /// <param name="model">University model</param>
        /// <returns>Insert acknowledgement</returns>
        ResponseModel AddUniversity(UniversityModel model);

        /// <summary>
        /// Removes an existing university from system
        /// </summary>
        /// <param name="universityId">The university identifier.</param>
        /// <returns>
        /// Delete acknowledgement
        /// </returns>
        ResponseModel DeleteUniversity(int universityId);

        /// <summary>
        /// Updates information of an existing university
        /// </summary>
        /// <param name="model">University model</param>
        /// <returns>Update acknowledgement</returns>
        ResponseModel UpdateUniversity(UniversityModel model);

        /// <summary>
        /// Adds the campuses to university.
        /// </summary>
        /// <param name="universityId">The university identifier.</param>
        /// <param name="campusIds">The campus ids.</param>
        /// <returns>Campus added to university confirmation</returns>
        ResponseModel AddCampusesToUniversity(int universityId, List<int> campusIds);
    }
}