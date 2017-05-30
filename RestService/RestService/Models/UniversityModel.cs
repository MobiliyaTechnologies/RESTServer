namespace RestService.Models
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
    }
}