namespace RestService.Models
{
    using System.Collections.Generic;

    public class MeterDayWiseMonthlyConsumptionModel
    {
        public MeterDayWiseMonthlyConsumptionModel()
        {
            this.DayWiseConsumption = new List<double>();
        }

        public string PowerScout { get; set; }

        public string Name { get; set; }

        public List<double> DayWiseConsumption { get; set; }
    }
}