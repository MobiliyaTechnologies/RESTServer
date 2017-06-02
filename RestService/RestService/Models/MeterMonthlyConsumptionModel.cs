namespace RestService.Models
{
    using System;

    public class MeterMonthlyConsumptionModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        public string Powerscout { get; set; }

        public double Monthly_KWH_Consumption { get; set; }

        public double Monthly_Electric_Cost { get; set; }

        public DateTime Current_Month { get; set; }

        public DateTime Last_Month { get; set; }
    }
}