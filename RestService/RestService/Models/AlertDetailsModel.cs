namespace RestService.Models
{
    using System;

    public class AlertDetailsModel
    {
        public int Sensor_Id { get; set; }

        public int Class_Id { get; set; }

        public string Class_Name { get; set; }

        public string Class_Desc { get; set; }

        public double Temperature { get; set; }

        public double Light_Intensity { get; set; }

        public double Humidity { get; set; }

        public DateTime Timestamp { get; set; }
    }
}