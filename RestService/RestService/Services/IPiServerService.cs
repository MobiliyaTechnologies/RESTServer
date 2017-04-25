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
        /// <param name="piServerID">PiServer ID</param>
        /// <returns>Returns a specific PiServer by fetching based on PiServerID</returns>
        PiServerModel GetPiServerByID(int piServerID);

        /// <summary>
        /// Get a PiServer by Name
        /// </summary>
        /// <param name="piServerName">Name of the pi server.</param>
        /// <returns>
        /// Returns a specific PiServer by fetching based on PiServer name
        /// </returns>
        PiServerModel GetPiServerByName(string piServerName);

        /// <summary>
        /// Inserts a new PiServer in system
        /// </summary>
        /// <param name="model">PiServer model</param>
        /// <returns>Insert acknowledgment.</returns>
        ResponseModel AddPiServer(PiServerModel model);

        /// <summary>
        /// Removes an existing PiServer from system
        /// </summary>
        /// <param name="piServerId">The pi server identifier.</param>
        /// <returns>
        /// Delete acknowledgment.
        /// </returns>
        ResponseModel DeletePiServer(int piServerId);

        /// <summary>
        /// Updates information of an existing PiServer
        /// </summary>
        /// <param name="model">PiServer model</param>
        /// <returns>Update acknowledgment.</returns>
        ResponseModel UpdatePiServer(PiServerModel model);
    }
}
