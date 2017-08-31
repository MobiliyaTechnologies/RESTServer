namespace EnergyManagementScheduler.WebJob.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using EnergyManagementScheduler.WebJob.Contracts;
    using EnergyManagementScheduler.WebJob.Logger;
    using EnergyManagementScheduler.WebJob.Services;
    using EnergyManagementScheduler.WebJob.Utilities;
    using Microsoft.Azure.WebJobs;

    /// <summary>
    /// Web jobs function to trigger weekly prediction.
    /// It run's at every 3 hours on every day.
    /// </summary>
    public static class WeeklyConsumptionPredictionJob
    {
        /// <summary>
        /// Process current day consumption and predict current week electricity consumption.
        /// </summary>
        /// <param name="timerInfo">The timer information.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ProcessWeeklyConsumptionPrediction([TimerTrigger("0 0/50 * * * *")] TimerInfo timerInfo)
        {
            try
            {
                var meterService = new MeterService();

                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationSettings.DbConnectionString))
                {
                    sqlConnection.Open();

                    var piServers = meterService.GetPiServerList(sqlConnection);

                    foreach (var piServer in piServers)
                    {
                        try
                        {
                            DateTime today = piServer.PiServerCurrentDateTime.AddDays(-1);
                            int prev = (int)(today.DayOfWeek - 1);
                            int next = (int)(7 - today.DayOfWeek);
                            var rowModified = 0;

                            using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                            {
                                var powerScouts = meterService.GetMeterListByPiServer(sqlConnection, sqlTransaction, piServer.PiServerName);

                                foreach (var powerScout in powerScouts)
                                {
                                    double weeklyPredictedKWH = 0;
                                    var dailyConsumptionDetails = meterService.GetTodaysDailyConsumptionDetails(sqlConnection, sqlTransaction, powerScout, piServer);

                                    if (dailyConsumptionDetails != null)
                                    {
                                        DateTime initialDate = dailyConsumptionDetails.Timestamp;

                                        if (prev > 0)
                                        {
                                            for (int i = 1; i <= prev; i++)
                                            {
                                                dailyConsumptionDetails.Timestamp = today.AddDays(-i);
                                                var response = await meterService.GetAnomalyPrediction(dailyConsumptionDetails, sqlConnection, sqlTransaction, piServer.PiServerName, false);
                                                if (response != null)
                                                {
                                                    List<string> columnNames = response.Results.Output1.Value.ColumnNames.ToList();
                                                    weeklyPredictedKWH += Convert.ToDouble(response.Results.Output1.Value.Values[0][columnNames.IndexOf("Scored Labels")]);
                                                }
                                            }
                                        }

                                        for (int i = 0; i <= next; i++)
                                        {
                                            dailyConsumptionDetails.Timestamp = today.AddDays(i);
                                            var response = await meterService.GetAnomalyPrediction(dailyConsumptionDetails, sqlConnection, sqlTransaction, piServer.PiServerName, false);
                                            if (response != null)
                                            {
                                                List<string> columnNames = response.Results.Output1.Value.ColumnNames.ToList();
                                                weeklyPredictedKWH += Convert.ToDouble(response.Results.Output1.Value.Values[0][columnNames.IndexOf("Scored Labels")]);
                                            }
                                        }

                                        if (weeklyPredictedKWH != 0)
                                        {
                                            var weeklyConsumptionPrediction = new WeeklyConsumptionPrediction();

                                            weeklyConsumptionPrediction.Start_Time = today.AddDays(-prev).Date;
                                            weeklyConsumptionPrediction.End_Time = today.AddDays(next).Date;
                                            weeklyConsumptionPrediction.PowerScout = powerScout;
                                            weeklyConsumptionPrediction.Breaker_details = dailyConsumptionDetails.Breaker_details;
                                            weeklyConsumptionPrediction.Weekly_Predicted_KWH_System = weeklyPredictedKWH;
                                            weeklyConsumptionPrediction.Building = dailyConsumptionDetails.Building;

                                            SaveWeeklyPrediction(weeklyConsumptionPrediction, sqlConnection, sqlTransaction, piServer.PiServerName);
                                            rowModified++;
                                        }
                                    }
                                }

                                sqlTransaction.Commit();
                            }

                            Console.WriteLine("WeeklyConsumptionPredictionJob Insert/Update  - PiServer - {0}, Rows {1}", piServer.PiServerName, rowModified);
                        }
                        catch (Exception e)
                        {
                            var errorMsg = string.Format("WeeklyConsumptionPredictionJob Error : Weekly consumption failed for PiServer - {0}", piServer.PiServerName);

                            Console.WriteLine(errorMsg);
                            Console.WriteLine("Error Message - {0}", e.Message);
                            Console.WriteLine("StackTrace - {0}", e.StackTrace);

                            ApplicationInsightsLogger.LogException(e, new Dictionary<string, string> { { "Job Error Message", errorMsg } });
                        }
                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception e)
            {
                ApplicationInsightsLogger.LogException(e);

                throw;
            }
        }

        private static void SaveWeeklyPrediction(WeeklyConsumptionPrediction weeklyConsumptionPrediction, SqlConnection sqlConnection, SqlTransaction sqlTransaction, string piServerName)
        {
            var meterService = new MeterService();

            var weeklyPrediction = meterService.GetWeeklyConsumptionPredictionForGivenDate(sqlConnection, sqlTransaction, weeklyConsumptionPrediction.PowerScout, weeklyConsumptionPrediction.Start_Time, piServerName);

            if (weeklyPrediction == null)
            {
                meterService.AddWeeklyConsumptionPrediction(weeklyConsumptionPrediction, sqlConnection, sqlTransaction, piServerName);
            }
            else
            {
                meterService.UpdateWeeklyConsumptionPrediction(weeklyConsumptionPrediction, sqlConnection, sqlTransaction);
            }
        }
    }
}
