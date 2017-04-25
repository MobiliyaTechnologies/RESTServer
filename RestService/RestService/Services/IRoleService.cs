namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Models;

    public interface IRoleService
    {
        /// <summary>
        /// Get all the roles
        /// </summary>
        /// <returns>Returns list of all roles</returns>
        List<RoleModel> GetAllRoles();

        /// <summary>
        /// Get a Role by ID
        /// </summary>
        /// <param name="roleId">Role ID</param>
        /// <returns>Returns a specific Role by fetching based on RoleID</returns>
        RoleModel GetRoleByID(int roleId);

        /// <summary>
        /// Inserts a new Role in system
        /// </summary>
        /// <param name="model">Role model</param>
        /// <returns>Insert acknowledgment</returns>
        ResponseModel AddRole(RoleModel model);

        /// <summary>
        /// Removes an existing Role from system
        /// </summary>
        /// <param name="roleId">The role identifier to be deleted.</param>
        /// <returns>
        /// Delete acknowledgment
        /// </returns>
        ResponseModel DeleteRole(int roleId);

        /// <summary>
        /// Updates information of an existing Role
        /// </summary>
        /// <param name="model">Role model</param>
        /// <returns>Update acknowledgment</returns>
        ResponseModel UpdateRole(RoleModel model);
    }
}
