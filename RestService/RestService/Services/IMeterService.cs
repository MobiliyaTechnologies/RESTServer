namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Entities;
    using RestService.Models;

    /// <summary>
    /// Provides meter related operations.
    /// </summary>
    public interface IMeterService
    {
        /// <summary>
        /// Gets the meter list.
        /// </summary>
        /// <returns>All meter details.</returns>
        List<MeterDetailsModel> GetMeterList();

        /// <summary>
        /// Gets the meter monthly consumption.
        /// </summary>
        /// <returns>The current month consumption.</returns>
        List<MeterMonthlyConsumptionModel> GetMeterMonthlyConsumption();

        /// <summary>
        /// Gets the meter daily consumption.
        /// </summary>
        /// <returns>The todays consumption.</returns>
        List<MeterDailyConsumptionModel> GetMeterDailyConsumption();

        /// <summary>
        /// Gets the month wise consumption.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>The month wise consumptions for given year/</returns>
        List<MeterMonthWiseConsumptionModel> GetMonthWiseConsumption(int year);

        /// <summary>
        /// Gets the month wise consumption for offset.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>The consumption for given period.</returns>
        List<MeterMonthWiseConsumptionModel> GetMonthWiseConsumptionForOffset(string month, int year, int offset);

        /// <summary>
        /// Gets the week wise monthly consumption.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns>The week wise consumption for given month and year.</returns>
        List<MeterWeekWiseMonthlyConsumptionModel> GetWeekWiseMonthlyConsumption(string month, int year);

        /// <summary>
        /// Gets the week wise monthly consumption for offset.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>The week wise consumption for given period.</returns>
        List<MeterWeekWiseMonthlyConsumptionModel> GetWeekWiseMonthlyConsumptionForOffset(string month, int year, int offset);

        /// <summary>
        /// Gets the day wise monthly consumption.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns>The day wise consumption for given month and year.</returns>
        List<MeterDayWiseMonthlyConsumptionModel> GetDayWiseMonthlyConsumption(string month, int year);

        /// <summary>
        /// Gets the day wise current month prediction.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns>The day wise prediction for given month and year.</returns>
        List<MeterDayWiseMonthlyConsumptionPredictionModel> GetDayWiseCurrentMonthPrediction(string month, int year);

        /// <summary>
        /// Gets the day wise next month prediction.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns>The day wise prediction of given's next month.</returns>
        List<MeterDayWiseMonthlyConsumptionPredictionModel> GetDayWiseNextMonthPrediction(string month, int year);
    }
}
