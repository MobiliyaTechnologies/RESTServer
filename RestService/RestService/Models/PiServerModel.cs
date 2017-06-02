namespace RestService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Newtonsoft.Json;

    public class PiServerModel
    {
        public int PiServerID { get; set; }

        [Required]
        [MaxLength(200)]
        public string PiServerName { get; set; }

        [MaxLength(500)]
        public string PiServerDesc { get; set; }

        [Range(1, int.MaxValue)]
        public int PremiseID { get; set; }

        [Required]
        public string PiServerURL { get; set; }

        [JsonIgnore]
        public Stream PremiseScheduleFile { get; set; }

        [JsonIgnore]
        public string PremiseScheduleFileType { get; set; }
    }
}