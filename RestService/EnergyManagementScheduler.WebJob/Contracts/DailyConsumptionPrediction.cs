namespace EnergyManagementScheduler.WebJob.Contracts
{
    using System;

    public class DailyConsumptionPrediction
    {
        public int Id { get; set; }

        public string PowerScout { get; set; }

        public string Breaker_details { get; set; }

        public DateTime Timestamp { get; set; }

        public double Daily_KWH_System { get; set; }

        public double Daily_Predicted_KWH_System { get; set; }

        public double TomorrowPredictedKWH { get; set; }

        public string Building { get; set; }
    }
}
