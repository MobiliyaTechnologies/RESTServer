namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Models;

    public interface IPiServerService
    {
        /// <summary>
        /// Get all the PiServers
        /// </summary>
        /// <returns>Returns list of all PiServers</returns>
        List<PiServerModel> GetAllPiServers();

        /// <summary>
        /// Get a PiServer by ID
        /// </summary>
        /// <param name="model">PiServer model</param>
        /// <returns>Returns a specific PiServer by fetching based on PiServerID</returns>
        PiServerModel GetPiServerByID(PiServerModel model);

        /// <summary>
        /// Get a PiServer by Name
        /// </summary>
        /// <param name="model">PiServer model</param>
        /// <returns>Returns a specific PiServer by fetching based on PiServer name</returns>
        PiServerModel GetPiServerByName(PiServerModel model);

        /// <summary>
        /// Inserts a new PiServer in system
        /// </summary>
        /// <param name="model">PiServer model</param>
        /// <param name="userId">User</param>
        /// <returns>Insert acknowledgement</returns>
        ResponseModel AddPiServer(PiServerModel model, int userId);

        /// <summary>
        /// Removes an existing PiServer from system
        /// </summary>
        /// <param name="model">PiServer model</param>
        /// <param name="userId">User</param>
        /// <returns>Delete acknowledgement</returns>
        ResponseModel DeletePiServer(PiServerModel model, int userId);

        /// <summary>
        /// Updates information of an existing PiServer
        /// </summary>
        /// <param name="model">PiServer model</param>
        /// <param name="userId">User</param>
        /// <returns>Update acknowledgement</returns>
        ResponseModel UpdatePiServer(PiServerModel model, int userId);
    }
}
