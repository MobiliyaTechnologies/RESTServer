using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{
    public class UserDataModel
    {
        public int Id { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Avatar { get; set; }

        public int Role_Id { get; set; }

    }
}