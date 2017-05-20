namespace RestService.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Enums;
    using RestService.Mappings;
    using RestService.Models;
    using RestService.Utilities;

    public class UserService : IUserService, IDisposable
    {
        private readonly PowerGridEntities dbContext;
        private readonly IContextInfoAccessorService context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        public UserService()
        {
            this.dbContext = new PowerGridEntities();
            this.context = new ContextInfoAccessorService();
        }

        int IUserService.CreateUser(UserModel userModel)
        {
            var userRole = this.dbContext.Role.WhereActiveRole(r => r.Id == userModel.RoleId).FirstOrDefault();

            if (userRole == null)
            {
                throw new ArgumentException(string.Format("Role does not exists for id - {0}", userModel.RoleId));
            }

            var user = new User
            {
                B2C_ObjectIdentifier = userModel.B2C_ObjectIdentifier,
                Email_Id = userModel.Email,
                First_Name = userModel.FirstName,
                Last_Name = userModel.LastName,
                Creation_Date = DateTime.UtcNow,
                Role = userRole,
                Avatar = userModel.Avatar
            };

            this.dbContext.User.Add(user);
            this.dbContext.SaveChanges();

            return user.Id;
        }

        ResponseModel IUserService.DeleteUser(string b2cObjectIdentifier)
        {
            var user = this.dbContext.User.WhereActiveUser(u => u.B2C_ObjectIdentifier == b2cObjectIdentifier).FirstOrDefault();

            if (user == null)
            {
                return new ResponseModel(StatusCode.Error, string.Format("User does not exist for B2C user id - {0}", b2cObjectIdentifier));
            }
            else
            {
                user.IsDeleted = true;
                user.Role = null;
                this.dbContext.SaveChanges();
                return new ResponseModel(StatusCode.Ok, "User deleted successfully");
            }
        }

        UserModel IUserService.GetCurrentUser(string b2cObjectIdentifier)
        {
            var user = this.dbContext.User.WhereActiveUser(u => u.B2C_ObjectIdentifier.Equals(b2cObjectIdentifier)).FirstOrDefault();

            if (user == null)
            {
                return null;
            }

            return new UserModelMapping().Map(user);
        }

        List<UserModel> IUserService.GetAllUser()
        {
            var user = this.dbContext.User.WhereActiveUser();

            return new UserModelMapping().Map(user).ToList();
        }

        ResponseModel IUserService.UpdateUser(UserModel userModel)
        {
            var user = this.dbContext.User.WhereActiveUser(u => u.Id == userModel.UserId).FirstOrDefault();

            if (user == null)
            {
                return new ResponseModel(StatusCode.Error, string.Format("User does not exists for id - {0}", userModel.UserId));
            }

            if (userModel.RoleId > 0)
            {
                var userRole = this.dbContext.Role.First(r => r.Id == userModel.RoleId);

                if (userRole == null)
                {
                    return new ResponseModel(StatusCode.Error, string.Format("Role does not exists for id - {0}", userModel.RoleId));
                }

                user.Role = userRole;
            }

            user.B2C_ObjectIdentifier = string.IsNullOrWhiteSpace(userModel.B2C_ObjectIdentifier) ? user.B2C_ObjectIdentifier : userModel.B2C_ObjectIdentifier;
            user.Email_Id = string.IsNullOrWhiteSpace(userModel.Email) ? user.Email_Id : userModel.Email;
            user.First_Name = string.IsNullOrWhiteSpace(userModel.FirstName) ? user.First_Name : userModel.FirstName;
            user.Last_Name = string.IsNullOrWhiteSpace(userModel.LastName) ? user.Last_Name : userModel.LastName;
            user.Avatar = string.IsNullOrWhiteSpace(userModel.Avatar) ? user.Avatar : userModel.Avatar;

            this.dbContext.SaveChanges();
            return new ResponseModel(StatusCode.Ok, "User Updated successfully.");
        }

        ResponseModel IUserService.AssignRoleToUser(int userId, int roleId)
        {
            var role = this.dbContext.Role.WhereActiveRole(r => r.Id == roleId).FirstOrDefault(); 

            if (role == null)
            {
                return new ResponseModel { Message = "Invalid Role", Status_Code = (int)StatusCode.Error };
            }

            var user = this.dbContext.User.WhereActiveUser(u => u.Id == userId).FirstOrDefault();

            if (user == null)
            {
                return new ResponseModel { Message = "Invalid User", Status_Code = (int)StatusCode.Error };
            }

            user.Role = role;
            this.dbContext.SaveChanges();

            return new ResponseModel(StatusCode.Ok, "Role Assigned to user successfully.");
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            if (this.dbContext != null)
            {
                this.dbContext.Dispose();
            }
        }
    }
}