namespace RestService.Services.Impl
{
    using System;
    using System.Configuration;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;
    using RestService.Utilities;

    public sealed class InsightService : IInsightService, IDisposable
    {
        private readonly PowerGridEntities dbContext;
        private readonly double utcOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsightService"/> class.
        /// </summary>
        public InsightService()
        {
            this.dbContext = new PowerGridEntities();
            this.utcOffset = Convert.ToDouble(ConfigurationManager.AppSettings["UTCOffset"]);
        }

        InsightDataModel IInsightService.GetInsightData()
        {
            var meterdetails = this.dbContext.MeterDetails.WhereActiveAccessibleMeterDetails();
            return this.GetInsightData(meterdetails);
        }

        InsightDataModel IInsightService.GetInsightDataByBuilding(int buildingId)
        {
            var meterdetails = this.dbContext.MeterDetails.WhereActiveAccessibleMeterDetails(m => m.BuildingId == buildingId);
            return this.GetInsightData(meterdetails);
        }

        InsightDataModel IInsightService.GetInsightDataByPremise(int premiseID)
        {
            var meterdetails = this.dbContext.MeterDetails.WhereActiveAccessibleMeterDetails(m => m.Building.PremiseID == premiseID);
            return this.GetInsightData(meterdetails);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            if (this.dbContext != null)
            {
                this.dbContext.Dispose();
            }
        }

        private InsightDataModel GetInsightData(IQueryable<MeterDetails> meterdetails)
        {
            InsightDataModel insightData = new InsightDataModel();

            var count = meterdetails.Count() * ((int)DateTime.UtcNow.AddHours(this.utcOffset).DayOfWeek == 0 ? 7 : (int)DateTime.UtcNow.AddHours(this.utcOffset).DayOfWeek);

            var consumptionValue = this.dbContext.DailyConsumptionDetails.Where(d => meterdetails.Any(m => m.PowerScout.Equals(d.PowerScout, StringComparison.InvariantCultureIgnoreCase))).OrderByDescending(d => d.Timestamp).Take(count).Sum(data => data.Daily_KWH_System);

            insightData.ConsumptionValue = consumptionValue.HasValue ? Math.Round(consumptionValue.Value, 2) : default(double);

            var predictedValue = this.dbContext.WeeklyConsumptionPrediction.Where(w => meterdetails.Any(m => m.PowerScout.Equals(w.PowerScout, StringComparison.InvariantCultureIgnoreCase))).OrderByDescending(w => w.End_Time).Take(meterdetails.Count()).Sum(w => w.Weekly_Predicted_KWH_System);

            insightData.PredictedValue = predictedValue.HasValue ? Math.Round(predictedValue.Value, 2) : default(double);

            return insightData;
        }
    }
}