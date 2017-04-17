namespace RestService.Services
{
    using RestService.Models;

    /// <summary>
    /// Provide power BI url operations.
    /// </summary>
    public interface IPowerBIService
    {
        /// <summary>
        /// Gets the power bi URL.
        /// 
        /// </summary>
        /// <param name="meterSerial">The meter serial.</param>
        /// <returns>The power BI url for given serial if exists else null.</returns>
        MeterURLKeyModel GetPowerBIUrl(string meterSerial);

        /// <summary>
        /// Gets the power bi general URL.
        /// </summary>
        /// <returns>The general power BI url.</returns>
        PowerBIGeneralURLModel GetPowerBIGeneralURL();
    }
}
