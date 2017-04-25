﻿namespace RestService.Services.Impl
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
            var roles = this.dbContext.Role;
            return new RoleModelMapping().Map(roles).ToList();
        }

        RoleModel IRoleService.GetRoleByID(int roleId)
        {
            var role = this.dbContext.Role.FirstOrDefault(data => data.Id == roleId);
            return new RoleModelMapping().Map(role);
        }

        ResponseModel IRoleService.AddRole(RoleModel model)
        {
            var role = new Role();
            role.RoleName = model.RoleName;
            role.Description = model.Description;
            role.CreatedBy = this.context.Current.UserId;
            role.CreatedOn = DateTime.UtcNow;
            role.ModifiedBy = this.context.Current.UserId;
            role.ModifiedOn = DateTime.UtcNow;
            role.IsActive = true;
            role.IsDeleted = false;

            this.dbContext.Role.Add(role);
            this.dbContext.SaveChanges();
            return new ResponseModel { Message = "Role added successfully", Status_Code = (int)StatusCode.Ok };
        }

        ResponseModel IRoleService.DeleteRole(int roleId)
        {
            var data = this.dbContext.Role.FirstOrDefault(f => f.Id == roleId);
            if (data == null)
            {
                return new ResponseModel { Message = "Invalid Role", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                data.IsActive = false;
                data.IsDeleted = true;
                data.ModifiedBy = this.context.Current.UserId;
                data.ModifiedOn = DateTime.UtcNow;
                this.dbContext.SaveChanges();
                return new ResponseModel { Message = "Role deleted successfully", Status_Code = (int)StatusCode.Ok };
            }
        }

        ResponseModel IRoleService.UpdateRole(RoleModel model)
        {
            var data = this.dbContext.Role.FirstOrDefault(f => f.Id == model.Id);

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

                data.IsActive = model.IsActive;
                data.IsDeleted = model.IsDeleted;
                data.ModifiedBy = this.context.Current.UserId;
                data.ModifiedOn = DateTime.UtcNow;
            }

            this.dbContext.SaveChanges();
            return new ResponseModel { Message = "Role details Updated", Status_Code = (int)StatusCode.Ok };
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