namespace EnergyManagementScheduler.WebJob.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using EnergyManagementScheduler.WebJob.Contracts;
    using EnergyManagementScheduler.WebJob.Logger;
    using EnergyManagementScheduler.WebJob.Services;
    using EnergyManagementScheduler.WebJob.Utilities;
    using Microsoft.Azure.WebJobs;

    /// <summary>
    /// Web jobs function to trigger Anomaly Detection Alert.
    /// It run's at 12 midnight every day.
    /// </summary>
    public class AnomalyDetectionJob
    {
        /// <summary>
        /// Detects the anomalies and generate alert.
        /// </summary>
        /// <param name="timerInfo">The timer information.</param>
        /// <returns>
        /// A <see cref="Task" /> representing the asynchronous operation.
        /// </returns>
        public static async Task ProcessAnomalyDetection([TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo)
        {
            try
            {
                var alertService = new AlertService();
                var meterService = new MeterService();
                var requestModel = new AnomalyServiceRequest();

                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationSettings.DbConnectionString))
                {
                    sqlConnection.Open();

                    var piServers = meterService.GetPiServerList(sqlConnection);

                    foreach (var piServer in piServers)
                    {
                        try
                        {
                            DateTime anomalyDetectionStartTime = piServer.PiServerCurrentDateTime.AddDays(-1).Date;
                            DateTime anomalyDetectionEndTime = anomalyDetectionStartTime.AddHours(24).AddSeconds(-1);

                            using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                            {
                                var query = "SELECT PowerScout, Temperature, Timestamp, Visibility, kW_System, days, Breaker_details FROM LiveData WHERE Timestamp > @startTime AND Timestamp <= @endTime and PiServerName = @PiServerName  ORDER BY Timestamp";

                                requestModel.GlobalParameters = new GlobalParameter();
                                Inputs inputs = new Inputs();
                                Input1 input = new Input1()
                                {
                                    ColumnNames = new List<string> { "PowerScout", "Temperature", "Timestamp", "Visibility", "kW_System", "days", "Breaker_details" }
                                };
                                List<List<string>> values = new List<List<string>>();

                                using (SqlCommand selectDataCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
                                {
                                    selectDataCommand.Parameters.AddWithValue("@startTime", anomalyDetectionStartTime.ToString(Constants.DATE_TIME_FORMAT));
                                    selectDataCommand.Parameters.AddWithValue("@endTime", anomalyDetectionEndTime.ToString(Constants.DATE_TIME_FORMAT));
                                    selectDataCommand.Parameters.AddWithValue("@PiServerName", piServer.PiServerName);

                                    using (SqlDataReader result = selectDataCommand.ExecuteReader())
                                    {
                                        List<string> rowValues = null;

                                        while (result.Read())
                                        {
                                            rowValues = new List<string>();

                                            rowValues.Add(SqlTypeConverter.ToString(result["PowerScout"]));
                                            rowValues.Add(SqlTypeConverter.ToString(result["Temperature"]));
                                            rowValues.Add(SqlTypeConverter.ToDateTime(result["Timestamp"]).ToString(Constants.DATE_TIME_FORMAT));
                                            rowValues.Add(SqlTypeConverter.ToString(result["Visibility"]));
                                            rowValues.Add(SqlTypeConverter.ToString(result["kW_System"]));
                                            rowValues.Add(SqlTypeConverter.ToString(result["days"]));
                                            rowValues.Add(SqlTypeConverter.ToString(result["Breaker_details"]));
                                            values.Add(rowValues);
                                        }

                                        result.Close();
                                    }
                                }

                                if (values.Count > 0)
                                {
                                    input.Values = values;
                                    inputs.input1 = input;
                                    requestModel.Inputs = inputs;
                                    var azureMLConfigurationService = new AzureMLConfigurationService();
                                    var anomalyConfig = azureMLConfigurationService.GetAnomalyConfigData(sqlConnection, sqlTransaction);
                                    var responseModel = await AzureMLClient.CallAzureMLAsync(requestModel, anomalyConfig.AzureMlAnomalyDetectionApiUrl, anomalyConfig.AzureMlAnomalyDetectionApiKey);

                                    if (responseModel != null)
                                    {
                                        AddAnomalyToDatabase(sqlConnection, sqlTransaction, responseModel, piServer.PiServerName, anomalyDetectionStartTime, anomalyDetectionEndTime, alertService);
                                    }
                                }

                                sqlTransaction.Commit();
                            }
                        }
                        catch (Exception e)
                        {
                            var errorMsg = string.Format("AnomalyDetectionJob Error : Anomaly detection failed for pi server - {0}", piServer.PiServerName);

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

        private static void AddAnomalyToDatabase(SqlConnection sqlConnection, SqlTransaction sqlTransaction, AnomalyServiceResponse anomalyServiceResponseModel, string piServerName, DateTime anomalyDetectionStartTime, DateTime anomalyDetectionEndTime, AlertService alertService)
        {
            List<List<string>> filteredAnomalies = anomalyServiceResponseModel.Results.Output1.Value.Values.Where(x => Convert.ToDouble(x.ElementAt(8)) < ConfigurationSettings.AnomalyThreshold).ToList();

            string anomalyoutputInsertQuery = "INSERT INTO AnomalyOutput(PowerScout, Temperature,Timestamp,Visibility,days,Breaker_details,kW_System,ScoredLabels,ScoredProbabilities, PiServername) VALUES (@PowerScout,@Temperature,@Timestamp,@Visibility,@days,@Breaker_details,@kW_System,@ScoredLabels,@ScoredProbabilities, @PiServerName)";

            foreach (var rowData in filteredAnomalies)
            {
                using (SqlCommand cmd = new SqlCommand(anomalyoutputInsertQuery, sqlConnection, sqlTransaction))
                {
                    cmd.Parameters.AddWithValue("@PowerScout", rowData.ElementAt(0).ToString());
                    cmd.Parameters.AddWithValue("@Temperature", Convert.ToDouble(rowData.ElementAt(1)));
                    cmd.Parameters.AddWithValue("@Timestamp", Convert.ToDateTime(rowData.ElementAt(2), CultureInfo.InvariantCulture).ToString(Constants.DATE_TIME_FORMAT));
                    cmd.Parameters.AddWithValue("@Visibility", Convert.ToDouble(rowData.ElementAt(3)));
                    cmd.Parameters.AddWithValue("@days", rowData.ElementAt(4).ToString());
                    cmd.Parameters.AddWithValue("@Breaker_details", rowData.ElementAt(5).ToString());
                    cmd.Parameters.AddWithValue("@kW_System", Convert.ToDouble(rowData.ElementAt(6)));
                    cmd.Parameters.AddWithValue("@ScoredLabels", Convert.ToDouble(rowData.ElementAt(7)));
                    cmd.Parameters.AddWithValue("@ScoredProbabilities", Convert.ToDouble(rowData.ElementAt(8)));
                    cmd.Parameters.AddWithValue("@PiServerName", piServerName);

                    cmd.ExecuteNonQuery();
                }
            }

            if (filteredAnomalies.Count > 0)
            {
                string anomalyAlertMessage = string.Format(Constants.ANOMALY_ALERT_MESSAGE, filteredAnomalies.Count, anomalyDetectionStartTime, anomalyDetectionEndTime);

                var alert = new Alert
                {
                    AlertType = Constants.ALERT_TYPE_ANOMALY,
                    Description = anomalyAlertMessage,
                    TimeStamp = anomalyDetectionStartTime,
                    PiServerName = piServerName
                };
                alertService.AddNewAlert(sqlConnection, sqlTransaction, alert);
            }

            Console.WriteLine("AnomalyDetectionJob RowInserted : Created alert and anomaly for PiServer - {0}.", piServerName);
        }
    }
}
