namespace RestService.Services.Impl
{
    using System;
    using System.Configuration;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

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
            InsightDataModel insightData = new InsightDataModel();
            var meterCount = this.dbContext.MeterDetails.Count();
            insightData.ConsumptionValue = Math.Round(
                (double)(from data in this.dbContext.DailyConsumptionDetails orderby data.Timestamp descending select data)
                .Take(meterCount * ((
                (int)DateTime.UtcNow.AddHours(this.utcOffset).DayOfWeek) == 0 ? 7 : (int)DateTime.UtcNow.AddHours(this.utcOffset).DayOfWeek)).Sum(data => data.Daily_KWH_System), 2);

            insightData.PredictedValue = Math.Round((from data in this.dbContext.WeeklyConsumptionPrediction orderby data.End_Time descending select data).Take(meterCount).Sum(data => data.Weekly_Predicted_KWH_System) ?? default(double), 2);
            return insightData;
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
    }
}