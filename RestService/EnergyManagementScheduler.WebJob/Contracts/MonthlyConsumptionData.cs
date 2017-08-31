namespace EnergyManagementScheduler.WebJob.Contracts
{
    using System;

    public class MonthlyConsumptionData
    {
        public int Id { get; set; }

        public string Month { get; set; }

        public string Year { get; set; }

        public string PowerScout { get; set; }

        public string Breaker_details { get; set; }

        public double? Monthly_KWH_System { get; set; }

        public double? Monthly_electric_cost { get; set; }

        public DateTime? Timestamp { get; set; }

        public DateTime? Current_month { get; set; }

        public DateTime? Last_month { get; set; }

        public string PiServerName { get; set; }

        public string Building { get; set; }
    }
}
