﻿namespace RestService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class PiServerModel
    {
        public int PiServerID { get; set; }

        [Required]
        [MaxLength(200)]
        public string PiServerName { get; set; }

        [MaxLength(500)]
        public string PiServerDesc { get; set; }

        [Required]
        public int CampusID { get; set; }

        public string PiServerURL { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}