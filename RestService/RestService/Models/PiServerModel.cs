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

        [Required]
        public string PiServerURL { get; set; }

        [JsonIgnore]
        public Stream RoomScheduleFile { get; set; }

        [JsonIgnore]
        public string RoomScheduleFileType { get; set; }
    }
}