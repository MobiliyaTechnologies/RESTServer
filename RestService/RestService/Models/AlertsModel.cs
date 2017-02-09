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
        public string Alert_Desc { get; set; }
        public string Class_Id { get; set; }
        public string Class_Desc { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Is_Acknowledged { get; set; }
        public string Acknowledged_By { get; set; }
        public DateTime Acknowledged_Timestamp { get; set; }
    }

    public class AlertDetailsModel
    {
        public int Sensor_Id { get; set; }
        public string Class_Id { get; set; }
        
        public string Class_Desc { get; set; }

        public bool Is_Light_ON { get; set; }
        public double Temperature { get; set; }
        public double Light_Intensity { get; set; }
        public double Humidity { get; set; }
        public double Battery_Remaining { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime Last_Updated { get; set; }
    }
}