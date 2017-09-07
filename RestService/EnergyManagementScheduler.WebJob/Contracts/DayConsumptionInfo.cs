namespace EnergyManagementScheduler.WebJob.Contracts
{
    public class DayConsumptionInfo
    {
        public long ActiveHourConsumption { get; set; }

        public long InActiveHourSameDayConsumption { get; set; }

        public long InActiveHourDifferentDayConsumption { get; set; }

        public long NonWorkingHourConsumption { get; set; }

        public long EarlyMorningConsumption { get; set; }

        public long MorningConsumption { get; set; }

        public long NoonConsumption { get; set; }

        public long EveningConsumption { get; set; }

        public long LateEveningConsumption { get; set; }

        public long TotalConsumption { get; set; }
    }
}
