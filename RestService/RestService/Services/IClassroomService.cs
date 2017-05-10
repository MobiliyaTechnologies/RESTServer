namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Models;

    /// <summary>
    /// Provides classroom operation.
    /// </summary>
    public interface IClassroomService
    {
        /// <summary>
        /// Gets all classrooms.
        /// </summary>
        /// <returns>All classrooms data</returns>
        List<ClassroomModel> GetAllClassrooms();

        /// <summary>
        /// Gets all building's classrooms.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <returns>
        /// The building's classrooms.
        /// </returns>
        List<ClassroomModel> GetClassroomByBuilding(int buildingId);
    }
}
