namespace RestService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class CampusModel
    {
        public int CampusID { get; set; }

        [Required]
        [MaxLength(200)]
        public string CampusName { get; set; }

        [MaxLength(500)]
        public string CampusDesc { get; set; }

        [Required]
        public int UniversityID { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}