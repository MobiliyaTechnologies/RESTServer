namespace RestService.Models
{
    public class MeterDetailsModel
    {
        public int Id { get; set; }

        public string PowerScout { get; set; }

        public string Name { get; set; }

        public string Serial { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public double Altitude { get; set; }

        public string Description { get; set; }
    }
}