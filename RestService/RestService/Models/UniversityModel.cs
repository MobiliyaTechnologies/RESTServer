﻿namespace RestService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class UniversityModel
    {
        public int UniversityID { get; set; }

        [Required]
        [MaxLength(200)]
        public string UniversityName { get; set; }

        [MaxLength(500)]
        public string UniversityDesc { get; set; }

        [MaxLength(500)]
        public string UniversityAddress { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}