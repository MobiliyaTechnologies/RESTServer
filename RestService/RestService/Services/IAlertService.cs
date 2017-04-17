namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Models;

    /// <summary>
    /// Provides alert operations.
    /// </summary>
    /// <seealso cref="RestService.Services.IAlertService" />
    /// <seealso cref="System.IDisposable" />
    public interface IAlertService
    {
        /// <summary>
        /// Gets all alerts.
        /// </summary>
        /// <returns>The alerts.</returns>
        List<AlertModel> GetAllAlerts();

        /// <summary>
        /// Gets the alert details.
        /// </summary>
        /// <param name="sensorLogId">The sensor log identifier.</param>
        /// <returns>The alert details.</returns>
        AlertDetailsModel GetAlertDetails(int sensorLogId);

        /// <summary>
        /// Acknowledges the alert.
        /// </summary>
        /// <param name="alertDetail">The alert detail.</param>
        /// <returns>The alert acknowledgment.</returns>
        ResponseModel AcknowledgeAlert(AlertModel alertDetail);

        /// <summary>
        /// Gets all recommendations.
        /// </summary>
        /// <returns>The recommendation.</returns>
        List<AlertModel> GetRecommendations();
    }
}
