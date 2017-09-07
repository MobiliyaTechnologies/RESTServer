namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Models;

    /// <summary>
    /// Provides user related operations.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="userModel">The user model.</param>
        /// <returns>
        /// Created user identifier..
        /// </returns>
        int CreateUser(UserModel userModel);

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="userModel">The user model.</param>
        /// <returns>
        /// User updated confirmation.
        /// </returns>
        ResponseModel UpdateUser(UserModel userModel);

        /// <summary>
        /// Deletes the user for given B2C object identifier.
        /// </summary>
        /// <param name="b2cObjectIdentifier">The B2C object identifier.</param>
        /// <returns>User deleted confirmation</returns>
        ResponseModel DeleteUser(string b2cObjectIdentifier);

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <returns>Current loged-in user.</returns>
        UserModel GetCurrentUser(string b2cObjectIdentifier);

        /// <summary>
        /// Gets all user.
        /// </summary>
        /// <returns>All users.</returns>
        List<UserModel> GetAllUser();

        /// <summary>
        /// Assigns the role to user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns>User Role assignment confirmation.</returns>
        ResponseModel AssignRoleToUser(int userId, int roleId);
    }
}
