using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{
    public class ClassroomModel
    {
        public string ClassId { get; set; }

        public string ClassDescription { get; set; }

        public int SensorId { get; set; }

        public string Building { get; set; }

        public string Breaker_details { get; set; }
    }
}