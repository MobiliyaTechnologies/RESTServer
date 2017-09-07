namespace EnergyManagementScheduler.WebJob.Contracts
{
    using System;

    public class AzureLiveData
    {
        public int Id { get; set; }

        public double AMPS_L1 { get; set; }

        public double AMPS_L2 { get; set; }

        public double AMPS_L3 { get; set; }

        public double AMPS_SYSTEM_AVG { get; set; }

        public string Breaker_details { get; set; }

        public string Breaker_label { get; set; }

        public string Building { get; set; }

        public int ClassOccupanyRemaining { get; set; }

        public int ClassOccupiedValue { get; set; }

        public int TotalClassCapacity { get; set; }

        public double Daily_electric_cost { get; set; }

        public double Daily_KWH_System { get; set; }

        public int IsClassOccupied { get; set; }

        public double KW_L1 { get; set; }

        public double KW_L2 { get; set; }

        public double KW_L3 { get; set; }

        public double Monthly_electric_cost { get; set; }

        public double Monthly_KWH_System { get; set; }

        public string PowerScout { get; set; }

        public double Rated_Amperage { get; set; }

        public double Pressure { get; set; }

        public double Relative_humidity { get; set; }

        public double Rolling_hourly_kwh_system { get; set; }

        public string Serial_number { get; set; }

        public double Temperature { get; set; }

        public DateTime Timestamp { get; set; }

        public string Type { get; set; }

        public double Visibility { get; set; }

        public double Volts_L1_to_neutral { get; set; }

        public double Volts_L2_to_neutral { get; set; }

        public double Volts_L3_to_neutral { get; set; }

        public double KW_System { get; set; }

        public string PiServerName { get; set; }
    }
}
