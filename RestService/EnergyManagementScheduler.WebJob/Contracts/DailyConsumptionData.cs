namespace EnergyManagementScheduler.WebJob.Contracts
{
    using System;

    public class DailyConsumptionData
    {
        public int Id { get; set; }

        public double? AMPS_SYSTEM_AVG { get; set; }

        public string Building { get; set; }

        public string Breaker_details { get; set; }

        public double? Daily_electric_cost { get; set; }

        public double? Daily_KWH_System { get; set; }

        public double? Monthly_electric_cost { get; set; }

        public double? Monthly_KWH_System { get; set; }

        public string PowerScout { get; set; }

        public double? Temperature { get; set; }

        public DateTime? Timestamp { get; set; }

        public double? Visibility { get; set; }

        public double? KW_System { get; set; }

        public string PiServerName { get; set; }
    }
}
