﻿namespace RestService.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserDataModel
    {
        public int Id { get; set; }

        [Required]
        public string First_Name { get; set; }

        [Required]
        public string Last_Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [MinLength(8)]
        public string Password { get; set; }

        public string Avatar { get; set; }

        [Required]
        [Range(1,2)]
        public int Role_Id { get; set; }

    }
}