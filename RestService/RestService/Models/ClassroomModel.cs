namespace RestService.Models
{
    public class ClassroomModel
    {
        public int ClassId { get; set; }

        public string ClassName { get; set; }

        public string ClassDescription { get; set; }

        public int SensorId { get; set; }

        public string Building { get; set; }

        public string Breaker_details { get; set; }

        public double X { get; set; }

        public double Y { get; set; }
    }
}