namespace RestService.Models
{
    public class MeterDetailsModel
    {
        public int Id { get; set; }

        public string PowerScout { get; set; }

        public string Name { get; set; }

        public double MonthlyConsumption { get; set; }

        public double MonthlyPrediction { get; set; }
    }
}