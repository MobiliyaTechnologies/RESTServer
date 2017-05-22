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

    public class RoleService : IRoleService, IDisposable
    {
        private readonly PowerGridEntities dbContext;
        private readonly IContextInfoAccessorService context;

        public RoleService()
        {
            this.dbContext = new PowerGridEntities();
            this.context = new ContextInfoAccessorService();
        }

        List<RoleModel> IRoleService.GetAllRoles()
        {
            var roles = this.dbContext.Role.WhereActiveRole();
            return new RoleModelMapping().Map(roles).ToList();
        }

        RoleModel IRoleService.GetRoleByID(int roleId)
        {
            var role = this.dbContext.Role.WhereActiveRole(data => data.Id == roleId).FirstOrDefault();
            return new RoleModelMapping().Map(role);
        }

        ResponseModel IRoleService.AddRole(RoleModel roleModel)
        {
            var role = new Role();
            role.RoleName = roleModel.RoleName;
            role.Description = roleModel.Description;
            role.CreatedBy = this.context.Current.UserId;
            role.CreatedOn = DateTime.UtcNow;
            role.ModifiedBy = this.context.Current.UserId;
            role.ModifiedOn = DateTime.UtcNow;
            role.IsActive = true;
            role.IsDeleted = false;

            this.dbContext.Role.Add(role);

            var campuses = this.dbContext.Campus.WhereActiveCampus(c => roleModel.CampusIds.Any(id => id == c.CampusID));

            foreach (var campus in campuses)
            {
                campus.Role.Add(role);
            }

            this.dbContext.SaveChanges();
            return new ResponseModel { Message = "Role added successfully", Status_Code = (int)StatusCode.Ok };
        }

        ResponseModel IRoleService.DeleteRole(int roleId)
        {
            var data = this.dbContext.Role.WhereActiveRole(f => f.Id == roleId).FirstOrDefault();
            if (data == null)
            {
                return new ResponseModel { Message = "Invalid Role", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                data.IsDeleted = true;
                data.ModifiedBy = this.context.Current.UserId;
                data.ModifiedOn = DateTime.UtcNow;
                this.dbContext.SaveChanges();
                return new ResponseModel { Message = "Role deleted successfully", Status_Code = (int)StatusCode.Ok };
            }
        }

        ResponseModel IRoleService.UpdateRole(RoleModel model)
        {
            var data = this.dbContext.Role.WhereActiveRole(f => f.Id == model.Id).FirstOrDefault();

            if (data == null)
            {
                return new ResponseModel { Message = "Invalid Role", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(model.RoleName))
                {
                    data.RoleName = model.RoleName;
                }

                if (!string.IsNullOrWhiteSpace(model.Description))
                {
                    data.Description = model.Description;
                }

                data.ModifiedBy = this.context.Current.UserId;
                data.ModifiedOn = DateTime.UtcNow;
            }

            this.dbContext.SaveChanges();
            return new ResponseModel { Message = "Role details Updated", Status_Code = (int)StatusCode.Ok };
        }

        RoleModel IRoleService.GetNewUserRole()
        {
            var anySuperAdminUser = this.dbContext.User.WhereActiveUser().Any(u => u.Role.RoleName.Equals(UserRole.SuperAdmin.ToString()));
            Role role;

            if (anySuperAdminUser)
            {
                role = this.dbContext.Role.Where(r => r.RoleName.Equals(UserRole.Student.ToString())).First();
            }
            else
            {
                role = this.dbContext.Role.Where(r => r.RoleName.Equals(UserRole.SuperAdmin.ToString())).First();
            }

            var roleModel = new RoleModelMapping().Map(role);
            return roleModel;
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