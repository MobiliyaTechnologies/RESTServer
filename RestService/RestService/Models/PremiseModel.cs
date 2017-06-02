namespace RestService.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PremiseModel
    {
        public int PremiseID { get; set; }

        [Required]
        [MaxLength(200)]
        public string PremiseName { get; set; }

        [MaxLength(500)]
        public string PremiseDesc { get; set; }

        [Range(1, int.MaxValue)]
        public int OrganizationID { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public double MonthlyConsumption { get; set; }
    }
}