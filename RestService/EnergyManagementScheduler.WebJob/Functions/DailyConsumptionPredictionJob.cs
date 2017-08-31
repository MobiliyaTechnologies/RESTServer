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
    /// Web jobs function to trigger daily prediction.
    /// It run's at every 3 hours on every day.
    /// </summary>
    public static class DailyConsumptionPredictionJob
    {
        /// <summary>
        /// Processes the daily consumption and predict next day electricity consumption.
        /// </summary>
        /// <param name="timerInfo">The timer information.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ProcessDailyConsumptionPrediction([TimerTrigger("0 0/50 * * * *")] TimerInfo timerInfo)
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
                            using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                            {
                                var predictionProcessedStatus = meterService.GetDailyPredictionProcessedStatus(sqlConnection, sqlTransaction, piServer.PiServerName);
                                var meterlist = meterService.GetMeterListByPiServer(sqlConnection, sqlTransaction, piServer.PiServerName);
                                DateTime? timestamp = null;
                                int rows = 0;

                                foreach (var meter in meterlist)
                                {
                                    if (predictionProcessedStatus.ContainsKey(meter))
                                    {
                                        timestamp = predictionProcessedStatus[meter].AddDays(-1);
                                    }
                                    else
                                    {
                                        timestamp = null;
                                    }

                                    var dailyConsumptionDetails = meterService.GetDailyConsumptionDetails(sqlConnection, sqlTransaction, meter, piServer.PiServerName, timestamp);

                                    foreach (var dailyConsumptionDetail in dailyConsumptionDetails)
                                    {
                                        var response = await meterService.GetAnomalyPrediction(dailyConsumptionDetail, sqlConnection, sqlTransaction, piServer.PiServerName);
                                        if (response != null)
                                        {
                                            rows++;
                                            var dailyConsumptionPrediction = new DailyConsumptionPrediction();
                                            List<string> columnNames = response.Results.Output1.Value.ColumnNames.ToList();
                                            var values = response.Results.Output1.Value.Values.First();

                                            dailyConsumptionPrediction.PowerScout = values[columnNames.IndexOf("PowerScout")];
                                            dailyConsumptionPrediction.Timestamp = dailyConsumptionDetail.Timestamp.AddDays(1);
                                            dailyConsumptionPrediction.Daily_Predicted_KWH_System = Convert.ToDouble(values[columnNames.IndexOf("Scored Labels")]);
                                            dailyConsumptionPrediction.Building = dailyConsumptionDetail.Building;
                                            dailyConsumptionPrediction.Breaker_details = dailyConsumptionDetail.Breaker_details;

                                            meterService.AddDailyConsumptionPrediction(dailyConsumptionPrediction, sqlConnection, sqlTransaction, piServer.PiServerName);
                                        }
                                    }
                                }

                                if (rows > 0)
                                {
                                    Console.WriteLine("DailyConsumptionPredictionJob RowInserted : PiServer - {0}, Rows - {1}", piServer.PiServerName, rows);
                                }

                                sqlTransaction.Commit();
                            }
                        }
                        catch (Exception e)
                        {
                            var errorMsg = string.Format("DailyConsumptionPredictionJob Error : Daily consumption prediction failed for PiServer - {0}", piServer.PiServerName);

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
    }
}
