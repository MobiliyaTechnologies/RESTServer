namespace RestService.Models
{
    public class SensorModel
    {
        public int Sensor_Id { get; set; }

        public string Sensor_Name { get; set; }

        public int? Class_Id { get; set; }

        public string Class_Name { get; set; }

        public double? X { get; set; }

        public double? Y { get; set; }

        public double? Class_X { get; set; }

        public double? Class_Y { get; set; }

        public double Temperature { get; set; }

        public double Humidity { get; set; }

        public double Brightness { get; set; }
    }
}