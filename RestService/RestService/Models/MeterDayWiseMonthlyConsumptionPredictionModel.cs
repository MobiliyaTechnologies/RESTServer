namespace RestService.Models
{
    using System.Collections.Generic;

    public class MeterDayWiseMonthlyConsumptionPredictionModel
    {
        public MeterDayWiseMonthlyConsumptionPredictionModel()
        {
            this.DayWiseConsumptionPrediction = new List<double>();
        }

        public string PowerScout { get; set; }

        public string Name { get; set; }

        public List<double> DayWiseConsumptionPrediction { get; set; }
    }
}