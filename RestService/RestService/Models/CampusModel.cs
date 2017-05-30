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

        [Range(1, int.MaxValue)]
        public int UniversityID { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public double MonthlyConsumption { get; set; }
    }
}