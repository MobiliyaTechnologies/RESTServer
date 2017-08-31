namespace EnergyManagementScheduler.WebJob.Contracts
{
    using System;

    public partial class WeeklyConsumptionPrediction
    {
        public int Id { get; set; }

        public DateTime Start_Time { get; set; }

        public DateTime End_Time { get; set; }

        public string PowerScout { get; set; }

        public string Breaker_details { get; set; }

        public double Weekly_Predicted_KWH_System { get; set; }

        public string Building { get; set; }
    }
}
