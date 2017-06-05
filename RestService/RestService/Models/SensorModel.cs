namespace RestService.Models
{
    public class SensorModel
    {
        public int Sensor_Id { get; set; }

        public string Sensor_Name { get; set; }

        public int? Room_Id { get; set; }

        public string Room_Name { get; set; }

        public double? X { get; set; }

        public double? Y { get; set; }

        public double? Room_X { get; set; }

        public double? Room_Y { get; set; }

        public double Temperature { get; set; }

        public double Humidity { get; set; }

        public double Brightness { get; set; }
    }
}