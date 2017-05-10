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

        /// <summary>
        /// Gets the insight data by building.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <returns>The consumption and prediction of electricity for given building.</returns>
        InsightDataModel GetInsightDataByBuilding(int buildingId);

        /// <summary>
        /// Gets the insight data by campus.
        /// </summary>
        /// <param name="campusId">The campus identifier.</param>
        /// <returns>The consumption and prediction of electricity for given campus.</returns>
        InsightDataModel GetInsightDataByCampus(int campusId);
    }
}
