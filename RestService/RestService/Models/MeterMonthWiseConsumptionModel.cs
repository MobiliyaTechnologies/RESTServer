namespace RestService.Models
{
    public class MeterMonthWiseConsumptionModel
    {
        public MeterMonthWiseConsumptionModel()
        {
            this.MonthWiseConsumption = new MonthListModel();
        }

        public string PowerScout { get; set; }

        public string Name { get; set; }

        public MonthListModel MonthWiseConsumption { get; set; }
    }
}