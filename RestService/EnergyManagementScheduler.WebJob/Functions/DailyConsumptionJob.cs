namespace EnergyManagementScheduler.WebJob.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using EnergyManagementScheduler.WebJob.Contracts;
    using EnergyManagementScheduler.WebJob.Logger;
    using EnergyManagementScheduler.WebJob.Services;
    using EnergyManagementScheduler.WebJob.Utilities;
    using Microsoft.Azure.WebJobs;

    /// <summary>
    /// Web jobs function to trigger daily consumption.
    /// It run's at every 2 hours on every day.
    /// </summary>
    public static class DailyConsumptionJob
    {
        /// <summary>
        /// Processes half hourly data to generate daily consumption details .
        /// </summary>
        /// <param name="timerInfo">The timer information.</param>
        public static void ProcessDailyConsumption([TimerTrigger("0 0/40 * * * *")] TimerInfo timerInfo)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationSettings.DbConnectionString))
                {
                    sqlConnection.Open();
                    var meterService = new MeterService();
                    var piServerList = meterService.GetPiServerList(sqlConnection);

                    foreach (var piServer in piServerList)
                    {
                        try
                        {
                            using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                            {
                                var meterList = meterService.GetMeterListByPiServer(sqlConnection, sqlTransaction, piServer.PiServerName);
                                var dailyConsumptionDataList = new List<AzureLiveData>();
                                var processedDataHistory = meterService.GetDailyConsumptionProcessedStatus(sqlConnection, sqlTransaction, piServer.PiServerName);

                                foreach (var meter in meterList)
                                {
                                    List<AzureLiveData> azureLiveDataList = new List<AzureLiveData>();

                                    var query = "select AMPS_SYSTEM_AVG, Breaker_details, Building, Daily_electric_cost, Daily_KWH_System, Monthly_electric_cost," +
                                            "Monthly_KWH_System, PowerScout, Temperature, Timestamp, Visibility, kW_System, PiServerName  from " +
                                            "(SELECT CAST(Timestamp AS DATE) DailyTime, Max(Daily_KWH_System) DKS FROM LiveData " +
                                            "WHERE {0} " +
                                            "group by CAST(Timestamp AS DATE)) AS dailyData " +
                                            "inner join LiveData as ALD " +
                                            "on(dailyData.DKS = ALD.Daily_KWH_System and dailyData.DailyTime = CAST(ALD.Timestamp AS DATE)) " +
                                            "WHERE {0} ";

                                    if (processedDataHistory.ContainsKey(meter))
                                    {
                                        query = string.Format(query, "PowerScout = @meter AND PiServerName = @PiServerName AND Timestamp > @processedTime ");
                                    }
                                    else
                                    {
                                        query = string.Format(query, "PowerScout = @meter AND PiServerName = @PiServerName ");
                                    }

                                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
                                    {
                                        sqlCommand.Parameters.Add(new SqlParameter("@meter", meter));
                                        sqlCommand.Parameters.Add(new SqlParameter("@PiserverName", piServer.PiServerName));

                                        if (processedDataHistory.ContainsKey(meter))
                                        {
                                            sqlCommand.Parameters.Add(new SqlParameter("@processedTime", processedDataHistory[meter]));
                                        }

                                        using (SqlDataReader result = sqlCommand.ExecuteReader())
                                        {
                                            while (result.Read())
                                            {
                                                AzureLiveData azureLiveData = new AzureLiveData();

                                                azureLiveData.AMPS_SYSTEM_AVG = SqlTypeConverter.ToDouble(result["AMPS_SYSTEM_AVG"]);

                                                azureLiveData.Breaker_details = SqlTypeConverter.ToString(result["Breaker_details"]);

                                                azureLiveData.Building = SqlTypeConverter.ToString(result["Building"]);

                                                azureLiveData.Daily_electric_cost = SqlTypeConverter.ToDouble(result["Daily_electric_cost"]);

                                                azureLiveData.Daily_KWH_System = SqlTypeConverter.ToDouble(result["Daily_KWH_System"]);

                                                azureLiveData.Monthly_electric_cost = SqlTypeConverter.ToDouble(result["Monthly_electric_cost"]);

                                                azureLiveData.Monthly_KWH_System = SqlTypeConverter.ToDouble(result["Monthly_KWH_System"]);

                                                azureLiveData.PowerScout = SqlTypeConverter.ToString(result["PowerScout"]);

                                                azureLiveData.Temperature = SqlTypeConverter.ToDouble(result["Temperature"]);

                                                azureLiveData.Timestamp = SqlTypeConverter.ToDateTime(result["Timestamp"]);

                                                azureLiveData.Visibility = SqlTypeConverter.ToDouble(result["Visibility"]);

                                                azureLiveData.KW_System = SqlTypeConverter.ToDouble(result["kW_System"]);

                                                azureLiveData.PiServerName = SqlTypeConverter.ToString(result["PiServerName"]);

                                                azureLiveDataList.Add(azureLiveData);
                                            }

                                            result.Close();
                                        }
                                    }

                                    var dailyData = azureLiveDataList.OrderByDescending(m => m.Timestamp).GroupBy(m => new { m.Timestamp.Date, m.Daily_KWH_System }).Select(s => s.First());


                                    dailyConsumptionDataList.AddRange(dailyData);
                                }

                                if (dailyConsumptionDataList.Count > 0)
                                {
                                    SaveDailyConsumptionData(sqlConnection, sqlTransaction, dailyConsumptionDataList);

                                    Console.WriteLine("DailyConsumptionJob RowInserted :  Piserver - {0},  Entries - {1}.", piServer.PiServerName, dailyConsumptionDataList.Count);
                                }

                                sqlTransaction.Commit();
                            }
                        }
                        catch (Exception e)
                        {
                            var errorMsg = string.Format("DailyConsumptionJob Error : Daily consumption failed for PiServer - {0}", piServer.PiServerName);

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

        private static void SaveDailyConsumptionData(SqlConnection sqlConnection, SqlTransaction sqlTransaction, List<AzureLiveData> dailyConsumptionDatas)
        {
            foreach (var record in dailyConsumptionDatas)
            {
                using (SqlCommand cmdNew = new SqlCommand("SP_InsertUpdateDailyConsumptionDetails", sqlConnection, sqlTransaction))
                {
                    cmdNew.Parameters.Add(new SqlParameter("@AMPS_SYSTEM_AVG", record.AMPS_SYSTEM_AVG));
                    cmdNew.Parameters.Add(new SqlParameter("@Building", record.Building));
                    cmdNew.Parameters.Add(new SqlParameter("@Breaker_details", record.Breaker_details));
                    cmdNew.Parameters.Add(new SqlParameter("@Daily_electric_cost", record.Daily_electric_cost));
                    cmdNew.Parameters.Add(new SqlParameter("@Daily_KWH_System", record.Daily_KWH_System));
                    cmdNew.Parameters.Add(new SqlParameter("@Monthly_electric_cost", record.Monthly_electric_cost));
                    cmdNew.Parameters.Add(new SqlParameter("@Monthly_KWH_System", record.Monthly_KWH_System));
                    cmdNew.Parameters.Add(new SqlParameter("@PowerScout", record.PowerScout));
                    cmdNew.Parameters.Add(new SqlParameter("@Temperature", record.Temperature));
                    cmdNew.Parameters.Add(new SqlParameter("@Timestamp", record.Timestamp));
                    cmdNew.Parameters.Add(new SqlParameter("@Visibility", record.Visibility));
                    cmdNew.Parameters.Add(new SqlParameter("@kW_System", record.KW_System));
                    cmdNew.Parameters.Add(new SqlParameter("@PiServerName", record.PiServerName));
                    cmdNew.Parameters.Add(new SqlParameter("@DayTimestamp", ((DateTime)record.Timestamp).Day));
                    cmdNew.Parameters.Add(new SqlParameter("@MonthTimestamp", ((DateTime)record.Timestamp).Month));
                    cmdNew.Parameters.Add(new SqlParameter("@YearTimestamp", ((DateTime)record.Timestamp).Year));

                    cmdNew.CommandType = CommandType.StoredProcedure;

                    cmdNew.ExecuteNonQuery();
                }
            }
        }
    }
}
