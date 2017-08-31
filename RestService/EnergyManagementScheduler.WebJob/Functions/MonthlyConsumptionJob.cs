namespace EnergyManagementScheduler.WebJob.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using EnergyManagementScheduler.WebJob.Contracts;
    using EnergyManagementScheduler.WebJob.Logger;
    using EnergyManagementScheduler.WebJob.Services;
    using EnergyManagementScheduler.WebJob.Utilities;
    using Microsoft.Azure.WebJobs;

    /// <summary>
    /// Web jobs function to trigger monthly consumption.
    /// It run's at every 3 hours on every day.
    /// </summary>
    public static class MonthlyConsumptionJob
    {
        /// <summary>
        /// Processes the daily consumption and generate monthly consumption.
        /// </summary>
        /// <param name="timerInfo">The timer information.</param>
        public static void ProcessMonthlyConsumption([TimerTrigger("0 0/50 * * * *")] TimerInfo timerInfo)
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
                                var meterlist = meterService.GetMeterListByPiServer(sqlConnection, sqlTransaction, piServer.PiServerName);
                                var monthlyConsumptionStatus = meterService.GetMonthlyConsumptionProcessedStatus(sqlConnection, sqlTransaction, piServer.PiServerName);
                                int rows = 0;

                                foreach (var meter in meterlist)
                                {
                                    DateTime currentDate = default(DateTime);

                                    if (!monthlyConsumptionStatus.ContainsKey(meter))
                                    {
                                        // if monthly consumption does not exists then process from start of daily consumption.
                                        currentDate = meterService.GetDailyConsumptionInitialStatus(sqlConnection, sqlTransaction, piServer.PiServerName, meter);
                                    }
                                    else
                                    {
                                        // process from processed last month of monthly consumption.
                                        currentDate = monthlyConsumptionStatus[meter];
                                    }

                                    do
                                    {
                                        var query = "SELECT Max(building) building, max(breaker_Details) breaker_Details, sum(Daily_electric_cost) Monthly_electric_cost, sum(Daily_KWH_System) Monthly_KWH_System, max(Timestamp) Timestamp FROM DailyConsumptionDetails WHERE PowerScout = @meter and PiServerName = @PiServerName and month([timestamp])= @CurrentMonth and year([timestamp])= @CurrentYear group by month([timestamp])";

                                        MonthlyConsumptionData monthlyConsumptionData = null;

                                        using (SqlCommand cmd = new SqlCommand(query, sqlConnection, sqlTransaction))
                                        {
                                            cmd.Parameters.Add(new SqlParameter("@meter", meter));
                                            cmd.Parameters.Add(new SqlParameter("@CurrentMonth", currentDate.Month));
                                            cmd.Parameters.Add(new SqlParameter("@CurrentYear", currentDate.Year));
                                            cmd.Parameters.Add(new SqlParameter("@PiServerName", piServer.PiServerName));
                                            SqlDataReader result = cmd.ExecuteReader();

                                            while (result.Read())
                                            {
                                                monthlyConsumptionData = new MonthlyConsumptionData();

                                                monthlyConsumptionData.Building = SqlTypeConverter.ToString(result["Building"]);

                                                monthlyConsumptionData.Breaker_details = SqlTypeConverter.ToString(result["Breaker_details"]);

                                                monthlyConsumptionData.Monthly_electric_cost = SqlTypeConverter.ToDouble(result["Monthly_electric_cost"]);

                                                monthlyConsumptionData.Monthly_KWH_System = SqlTypeConverter.ToDouble(result["Monthly_KWH_System"]);

                                                monthlyConsumptionData.Timestamp = SqlTypeConverter.ToDateTime(result["Timestamp"]);
                                            }

                                            result.Close();
                                        }

                                        if (monthlyConsumptionData != null)
                                        {
                                            using (SqlCommand cmdNew = new SqlCommand("SP_InsertUpdateMonthlyConsumptionDetails", sqlConnection, sqlTransaction))
                                            {
                                                cmdNew.Parameters.Add(new SqlParameter("@Building", monthlyConsumptionData.Building));
                                                cmdNew.Parameters.Add(new SqlParameter("@Breaker_details", monthlyConsumptionData.Breaker_details));
                                                cmdNew.Parameters.Add(new SqlParameter("@Month", currentDate.ToString("MMM")));
                                                cmdNew.Parameters.Add(new SqlParameter("@Year", currentDate.Year.ToString()));
                                                cmdNew.Parameters.Add(new SqlParameter("@Monthly_electric_cost", monthlyConsumptionData.Monthly_electric_cost));
                                                cmdNew.Parameters.Add(new SqlParameter("@Monthly_KWH_System", monthlyConsumptionData.Monthly_KWH_System));
                                                cmdNew.Parameters.Add(new SqlParameter("@PowerScout", meter));
                                                cmdNew.Parameters.Add(new SqlParameter("@Timestamp", monthlyConsumptionData.Timestamp));
                                                cmdNew.Parameters.Add(new SqlParameter("@PiServerName", piServer.PiServerName));
                                                cmdNew.CommandType = CommandType.StoredProcedure;
                                                cmdNew.ExecuteNonQuery();
                                            }

                                            rows++;
                                        }

                                        currentDate = currentDate.AddMonths(1);
                                    }
                                    while (currentDate.Year < piServer.PiServerCurrentDateTime.Year ? true : currentDate.Year == piServer.PiServerCurrentDateTime.Year && currentDate.Month <= piServer.PiServerCurrentDateTime.Month); // process up-to current month and year.
                                }

                                if (rows > 0)
                                {
                                    Console.WriteLine("MonthlyConsumptionJob RowInserted : Piserver - {0}, Rows - {1}", piServer.PiServerName, rows);
                                }

                                sqlTransaction.Commit();
                            }
                        }
                        catch (Exception e)
                        {
                            var errorMsg = string.Format("MonthlyConsumptionJob Error : monthly consumption failed for pi server - {0}", piServer.PiServerName);

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
