namespace RestService.Models
{
    using System.Collections.Generic;

    public class MeterWeekWiseMonthlyConsumptionModel
    {
        public MeterWeekWiseMonthlyConsumptionModel()
        {
            this.WeekWiseConsumption = new List<double>();
        }

        public string PowerScout { get; set; }

        public string Name { get; set; }

        public List<double> WeekWiseConsumption { get; set; }
    }
}