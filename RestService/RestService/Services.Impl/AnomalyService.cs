namespace RestService.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Mappings;
    using RestService.Models;
    using RestService.Utilities;

    public sealed class AnomalyService : IAnomalyService, IDisposable
    {
        private readonly PowerGridEntities dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnomalyService"/> class.
        /// </summary>
        public AnomalyService()
        {
            this.dbContext = new PowerGridEntities();
        }

        List<AnomalyInfoModel> IAnomalyService.GetAnomalyDetails(string timeStamp)
        {
            DateTime date = ServiceUtil.UnixTimeStampToDateTime(Convert.ToDouble(timeStamp)).Date;
            DateTime startTime = date.Date;
            DateTime endTime = startTime.AddHours(24).AddSeconds(-1);
            var anomalyOutputs = this.dbContext.AnomalyOutput.Where(data => data.Timestamp > startTime && data.Timestamp <= endTime);

            return new AnomalyInfoModelMapping().Map(anomalyOutputs).ToList();
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