namespace EnergyManagementScheduler.WebJob.Contracts
{
    public class AnomalyServiceRequest
    {
        public Inputs Inputs { get; set; }

        public GlobalParameter GlobalParameters { get; set; }
    }
}
