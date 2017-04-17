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
    }
}
