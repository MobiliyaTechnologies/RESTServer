namespace RestService.Models
{
    using System;

    public class AlertDetailsModel
    {
        public int Sensor_Id { get; set; }

        public int Room_Id { get; set; }

        public string Room_Name { get; set; }

        public string Room_Desc { get; set; }

        public double Temperature { get; set; }

        public double Light_Intensity { get; set; }

        public double Humidity { get; set; }

        public DateTime Timestamp { get; set; }
    }
}