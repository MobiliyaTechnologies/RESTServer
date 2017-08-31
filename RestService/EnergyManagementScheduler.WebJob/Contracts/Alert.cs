namespace EnergyManagementScheduler.WebJob.Contracts
{
    using System;

    public class Alert
    {
        public int Id { get; set; }

        public int SensorLogId { get; set; }

        public int SensorId { get; set; }

        public string AlertType { get; set; }

        public string Description { get; set; }

        public DateTime TimeStamp { get; set; }

        public int IsAcknowledged { get; set; }

        public string PiServerName { get; set; }
    }
}
