namespace RestService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class BuildingModel
    {
        public int BuildingID { get; set; }

        [Required]
        [MaxLength(200)]
        public string BuildingName { get; set; }

        [MaxLength(500)]
        public string BuildingDesc { get; set; }

        [Required]
        public int CampusID { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}