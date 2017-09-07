namespace EnergyManagementScheduler.WebJob.Contracts
{
    using System;

    public class ProcessStatus
    {
        public string Meter { get; set; }

        public DateTime LastProcessTimeStamp { get; set; }
    }
}
