namespace RestService.Models
{
    using System;
    using System.Collections.Generic;
    using RestService.Enums;

    public class UserModel
    {
        public UserModel()
        {
            this.UserCampus = new List<CampusModel>();
        }

        public int UserId { get; set; }

        public string B2C_ObjectIdentifier { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int RoleId { get; set; }

        public string Avatar { get; set; }

        public UserRole RoleType
        {
            get
            {
                return Enum.IsDefined(typeof(UserRole), this.RoleId) ? (UserRole)this.RoleId : UserRole.CampusAdmin;
            }
        }

        public List<CampusModel> UserCampus { get; set; }
    }
}