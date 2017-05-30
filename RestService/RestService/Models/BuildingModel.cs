namespace RestService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class BuildingModel
    {
        public int BuildingID { get; set; }

        [Required]
        [MaxLength(200)]
        public string BuildingName { get; set; }

        [MaxLength(500)]
        public string BuildingDesc { get; set; }

        [Range(1, int.MaxValue)]
        public int CampusID { get; set; }

        public double MonthlyConsumption { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}