﻿namespace RestService.Services
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
        /// <param name="buildingId">The building identifier for which to get electricity consumption.</param>
        /// <returns>
        /// All meter details.
        /// </returns>
        List<MeterDetailsModel> GetMeterList(int buildingId);

        /// <summary>
        /// Gets the meter monthly consumption.
        /// </summary>
        /// <param name="buildingId">The building identifier for which to get electricity consumption.</param>
        /// <returns>The current month consumption.</returns>
        List<MeterMonthlyConsumptionModel> GetMeterMonthlyConsumption(int buildingId);

        /// <summary>
        /// Gets the meter monthly consumption and prediction per premise.
        /// </summary>
        /// <param name="premiseID">The premise identifier.</param>
        /// <returns>
        /// The current month electricity consumption and prediction per premise
        /// </returns>
        ConsumptionPredictionModel GetMonthlyConsumptionPredictionPerPremise(int? premiseID = null);

        /// <summary>
        /// Gets the meter monthly consumption and prediction per building.
        /// </summary>
        /// <param name="buildingId">The building identifier.</param>
        /// <returns>
        /// The current month electricity consumption and prediction per building.
        /// </returns>
        ConsumptionPredictionModel GetMonthlyConsumptionPredictionPerBuildings(int buildingId);

        /// <summary>
        /// Gets the meter daily consumption.
        /// </summary>
        /// <param name="buildingId">The building identifier for which to get electricity consumption.</param>
        /// <returns>The todays consumption.</returns>
        List<MeterDailyConsumptionModel> GetMeterDailyConsumption(int buildingId);

        /// <summary>
        /// Gets the month wise consumption.
        /// </summary>
        /// <param name="buildingId">The building identifier for which to get electricity consumption.</param>
        /// <param name="year">The year.</param>
        /// <returns>The month wise consumptions for given year/</returns>
        List<MeterMonthWiseConsumptionModel> GetMonthWiseConsumption(int buildingId, int year);

        /// <summary>
        /// Gets the week wise monthly consumption.
        /// </summary>
        /// <param name="buildingId">The building identifier for which to get electricity consumption.</param>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns>The week wise consumption for given month and year.</returns>
        List<MeterWeekWiseMonthlyConsumptionModel> GetWeekWiseMonthlyConsumption(int buildingId, string month, int year);

        /// <summary>
        /// Gets the day wise monthly consumption.
        /// </summary>
        /// <param name="buildingId">The building identifier for which to get electricity consumption.</param>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns>The day wise consumption for given month and year.</returns>
        List<MeterDayWiseMonthlyConsumptionModel> GetDayWiseMonthlyConsumption(int buildingId, string month, int year);

        /// <summary>
        /// Gets the day wise current month prediction.
        /// </summary>
        /// <param name="buildingId">The building identifier for which to get electricity consumption.</param>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns>The day wise prediction for given month and year.</returns>
        List<MeterDayWiseMonthlyConsumptionPredictionModel> GetDayWiseCurrentMonthPrediction(int buildingId, string month, int year);

        /// <summary>
        /// Gets the day wise next month prediction.
        /// </summary>
        /// <param name="buildingId">The building identifier for which to get electricity consumption.</param>
        /// <param name="month">The month.</param>
        /// <param name="year">The year.</param>
        /// <returns>The day wise prediction of given's next month.</returns>
        List<MeterDayWiseMonthlyConsumptionPredictionModel> GetDayWiseNextMonthPrediction(int buildingId, string month, int year);
    }
}
