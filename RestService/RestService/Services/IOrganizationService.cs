namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Models;

    public interface IOrganizationService
    {
        /// <summary>
        /// Get the organization.
        /// </summary>
        /// <returns>
        /// Return organization.
        /// </returns>
        OrganizationModel GetOrganization();

        /// <summary>
        /// Inserts a new Organization in system if not exist or update existing organization.
        /// Only one organization allowed.
        /// </summary>
        /// <param name="model">Organization model.</param>
        /// <returns>Insert acknowledgement.</returns>
        ResponseModel AddOrganization(OrganizationModel model);

        /// <summary>
        /// Adds the premises to organization.
        /// </summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="premiseIds">The premise ids.</param>
        /// <returns>
        /// Premise added to organization confirmation
        /// </returns>
        ResponseModel AddPremisesToOrganization(int organizationId, List<int> premiseIds);
    }
}