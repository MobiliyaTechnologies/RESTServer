namespace RestService.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Newtonsoft.Json;

    public class OrganizationModel
    {
        public int OrganizationID { get; set; }

        [MaxLength(200)]
        public string OrganizationName { get; set; }

        [MaxLength(500)]
        public string OrganizationDesc { get; set; }

        [MaxLength(500)]
        public string OrganizationAddress { get; set; }

        [JsonIgnore]
        public Stream OrganizationLogo { get; set; }

        [JsonIgnore]
        public string OrganizationLogoContentType { get; set; }

        [JsonIgnore]
        public string OrganizationLogoName { get; set; }

        public string OrganizationLogoUri { get; set; }
    }
}