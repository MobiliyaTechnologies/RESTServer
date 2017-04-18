namespace RestService.Models
{
    using RestService.Utilities;

    public class PowerBIGeneralURLModel
    {
        public string MonthlyConsumptionKWh
        {
            get { return ApiConfiguration.MonthlyConsumptionKWh; }
        }

        public string MonthlyConsumptionCost
        {
            get { return ApiConfiguration.MonthlyConsumptionCost; }
        }
    }
}