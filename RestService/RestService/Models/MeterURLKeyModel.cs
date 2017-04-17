namespace RestService.Models
{
    public class MeterURLKeyModel
    {
        public string Weather { get; set; }

        public string QuarterlyConsumption { get; set; }

        public string LastQuarterlyConsumption { get; set; }

        public string MonthlyConsumption { get; set; }

        public string LastMonthlyConsumption { get; set; }

        public string WeeklyConsumption { get; set; }

        public string LastWeeklyConsumption { get; set; }

        public string DailyConsumption { get; set; }

        public string YesterdayConsumption { get; set; }

        public string MonthlyKWh { get; set; }

        public string MonthlyCost { get; set; }

        public string DayWiseConsumption { get; set; }

        public string PeriodWiseConsumption { get; set; }

        public string Report { get; set; }
    }
}