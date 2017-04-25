namespace RestService.Models
{
    using System;

    public class AlertModel
    {
        public int Alert_Id { get; set; }

        public int Sensor_Log_Id { get; set; }

        public int Sensor_Id { get; set; }

        public string Alert_Type { get; set; }

        public string Alert_Desc { get; set; }

        public int? Class_Id { get; set; }

        public string Class_Name { get; set; }

        public string Class_Desc { get; set; }

        public DateTime Timestamp { get; set; }

        public bool Is_Acknowledged { get; set; }

        public string Acknowledged_By { get; set; }

        public DateTime Acknowledged_Timestamp { get; set; }
    }
}