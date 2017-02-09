using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestService.Entities;

namespace RestService.Models
{
    public class ResponseUserModel
    {
        public int Status_Code { get; set; }

        public string Message { get; set; }

        public string First_Name { get; set; }

        public string Last_Name { get; set; }

        public string Email { get; set; }

        public string Avatar { get; set; }

        public int User_Id { get; set; }

        public int Role_Id { get; set; }

        //public ICollection<UserRole> Role_Id { get; set; }
    }
}