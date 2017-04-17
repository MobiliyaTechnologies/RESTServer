namespace RestService.Services
{
    using RestService.Models;

    /// <summary>
    /// Provides consumption and prediction operations.
    /// </summary>
    public interface IInsightService
    {
        /// <summary>
        /// Gets the insight data.
        /// </summary>
        /// <returns>The consumption and prediction of electricity.</returns>
        InsightDataModel GetInsightData();
    }
}
