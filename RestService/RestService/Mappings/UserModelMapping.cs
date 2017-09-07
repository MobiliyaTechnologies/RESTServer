namespace RestService.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

    public class UserModelMapping
    {
        public IQueryable<UserModel> Map(IQueryable<User> source)
        {
            return from s in source
                   select new UserModel
                   {
                       UserId = s.Id,
                       B2C_ObjectIdentifier = s.B2C_ObjectIdentifier,
                       Email = s.Email_Id,
                       FirstName = s.First_Name,
                       LastName = s.Last_Name,
                       RoleId = s.RoleId ?? default(int),
                       Avatar = s.Avatar
                   };
        }

        public UserModel Map(User source)
        {
            return source == null ? null : this.Map(new List<User> { source }.AsQueryable()).FirstOrDefault();
        }
    }
}