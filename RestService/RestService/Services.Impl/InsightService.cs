namespace RestService.Services.Impl
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Net.Http;
    using System.Web;
    using RestService.Entities;
    using RestService.Models;
    using RestService.Utilities;

    public sealed class InsightService : IInsightService, IDisposable
    {
        private readonly PowerGridEntities dbContext;
        private readonly IMeterService meterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsightService"/> class.
        /// </summary>
        public InsightService()
        {
            this.dbContext = new PowerGridEntities();
            this.meterService = new MeterService();
        }

        InsightDataModel IInsightService.GetInsightData()
        {
            if (this.HasDemoDateFilter())
            {
                var consumptionPrediction = this.meterService.GetMonthlyConsumptionPredictionPerPremise();
                return new InsightDataModel { ConsumptionValue = consumptionPrediction.Consumption, PredictedValue = consumptionPrediction.Prediction };
            }

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

            if (meterdetails.Count() > 0)
            {
                var now = ServiceUtil.GetCurrentDateTime(meterdetails.First().UTCConversionTime);

                var count = meterdetails.Count() * ((int)now.DayOfWeek == 0 ? 7 : (int)now.DayOfWeek);

                var consumptionValue = this.dbContext.DailyConsumptionDetails.WhereInDateRange().Where(d => meterdetails.Any(m => m.PowerScout.Equals(d.PowerScout, StringComparison.InvariantCultureIgnoreCase))).OrderByDescending(d => d.Timestamp).Take(count).Sum(data => data.Daily_KWH_System);

                insightData.ConsumptionValue = consumptionValue.HasValue ? Math.Round(consumptionValue.Value, 2) : default(double);

                var predictedValue = this.dbContext.WeeklyConsumptionPrediction.WhereInDateRange().Where(w => meterdetails.Any(m => m.PowerScout.Equals(w.PowerScout, StringComparison.InvariantCultureIgnoreCase))).OrderByDescending(w => w.End_Time).Take(meterdetails.Count()).Sum(w => w.Weekly_Predicted_KWH_System);

                insightData.PredictedValue = predictedValue.HasValue ? Math.Round(predictedValue.Value, 2) : default(double);
            }

            return insightData;
        }

        private bool HasDemoDateFilter()
        {
            try
            {
                var request = (HttpRequestMessage)HttpContext.Current.Items["MS_HttpRequestMessage"];
                var queryParam = request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);

                return queryParam.ContainsKey("DateFilter");
            }
            catch (Exception)
            {
            }

            return false;
        }
    }
}