namespace RestService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
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

        [Range(1, int.MaxValue)]
        public int CampusID { get; set; }

        [Required]
        public string PiServerURL { get; set; }

        public Stream CampusScheduleFile { get; set; }

        public string CampusScheduleFileType { get; set; }
    }
}