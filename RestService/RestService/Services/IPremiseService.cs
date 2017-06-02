namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Models;

    /// <summary>
    /// Provides premise related operations.
    /// </summary>
    public interface IPremiseService
    {
        /// <summary>
        /// Get all the premise
        /// </summary>
        /// <returns>Returns list of all premises</returns>
        List<PremiseModel> GetAllPremise();

        /// <summary>
        /// Get a Premise by ID
        /// </summary>
        /// <param name="premiseID">The premise identifier.</param>
        /// <returns>
        /// Returns a specific Premise by fetching based on PremiseID
        /// </returns>
        PremiseModel GetPremiseByID(int premiseID);

        /// <summary>
        /// Gets the premise by location.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <returns>The premise situated at given location.</returns>
        PremiseModel GetPremiseByLocation(decimal latitude, decimal longitude);

        /// <summary>
        /// Inserts a new Premise in system
        /// </summary>
        /// <param name="model">Premise model</param>
        /// <returns>
        /// Insert acknowledgment.
        /// </returns>
        ResponseModel AddPremise(PremiseModel model);

        /// <summary>
        /// Removes an existing Premise from system
        /// </summary>
        /// <param name="premiseID">The premise identifier.</param>
        /// <returns>
        /// Delete acknowledgment.
        /// </returns>
        ResponseModel DeletePremise(int premiseID);

        /// <summary>
        /// Updates information of an existing Premise
        /// </summary>
        /// <param name="model">Premise model</param>
        /// <returns>Update acknowledgment</returns>
        ResponseModel UpdatePremise(PremiseModel model);

        /// <summary>
        /// Assigns the roles to premise.
        /// </summary>
        /// <param name="roleIds">The role ids.</param>
        /// <param name="premiseID">The premise identifier.</param>
        /// <returns>
        /// Roles Assigned acknowledgment.
        /// </returns>
        ResponseModel AssignRolesToPremise(List<int> roleIds, int premiseID);

        /// <summary>
        /// Adds the buildings to premise.
        /// </summary>
        /// <param name="premiseID">The premise identifier.</param>
        /// <param name="buildingIds">The building ids.</param>
        /// <returns>Association confirmation.</returns>
        ResponseModel AddBuildingsToPremise(int premiseID, List<int> buildingIds);
    }
}
