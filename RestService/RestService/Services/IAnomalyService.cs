namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Models;

    /// <summary>
    /// Provides anomaly info.
    /// </summary>
    public interface IAnomalyService
    {
        /// <summary>
        /// Gets the anomaly details.
        /// </summary>
        /// <param name="timeStamp">The time stamp.</param>
        /// <returns>The anomaly info.</returns>
        List<AnomalyInfoModel> GetAnomalyDetails(string timeStamp);
    }
}
