namespace RestService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class RoleModel
    {
        public RoleModel()
        {
            this.CampusIds = new List<int>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string RoleName { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public List<int> CampusIds { get; set; }

        public List<CampusModel> Campuses { get; set; }
    }
}