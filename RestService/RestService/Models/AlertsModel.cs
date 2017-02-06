using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Models
{
    public class AlertModel
    {
        public int Sensor_Log_Id { get; set; }
        public int Sensor_Id { get; set; }
        public string Alert_Type { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Is_Acknowledged { get; set; }
        public string Acknowledged_By { get; set; }
        public DateTime Acknowledged_Timestamp { get; set; }
    }
}