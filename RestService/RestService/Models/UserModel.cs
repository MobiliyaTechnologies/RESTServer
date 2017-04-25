namespace RestService.Models
{
    using System;
    using RestService.Enums;

    public class UserModel
    {
        public int UserId { get; set; }

        public string B2C_ObjectIdentifier { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int RoleId { get; set; }

        public UserRole RoleType
        {
            get
            {
                return Enum.IsDefined(typeof(UserRole), this.RoleId) ? (UserRole)this.RoleId : UserRole.CampusAdmin;
            }
        }
    }
}