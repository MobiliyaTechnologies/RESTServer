namespace EnergyManagementScheduler.WebJob.Contracts
{
    using System;

    public class PiServer
    {
        public string PiServerName { get; set; }

        public double UTCConversionTime { get; set; }

        public DateTime PiServerCurrentDateTime
        {
            get
            {
                return DateTime.UtcNow.AddHours(this.UTCConversionTime);
            }
        }
    }
}
