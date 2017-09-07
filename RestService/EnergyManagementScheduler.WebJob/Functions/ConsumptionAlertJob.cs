namespace EnergyManagementScheduler.WebJob.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using EnergyManagementScheduler.WebJob.Contracts;
    using EnergyManagementScheduler.WebJob.Logger;
    using EnergyManagementScheduler.WebJob.Services;
    using EnergyManagementScheduler.WebJob.Utilities;
    using Microsoft.Azure.WebJobs;

    /// <summary>
    /// Web jobs function to trigger consumption alert.
    /// It run's at 12 midnight every day.
    /// </summary>
    public class ConsumptionAlertJob
    {
        /// <summary>
        /// Process electricity consumption to generate recommendation for power saving.
        /// </summary>
        /// <param name="timerInfo">The timer information.</param>
        public static void ProcessConsumptionAlert([TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo)
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
                            DateTime prevWeekDay = piServer.PiServerCurrentDateTime.Date.AddDays(-1);
                            DateTime lastWeekendSunday = prevWeekDay.AddDays(-(int)prevWeekDay.DayOfWeek);

                            // if today is weekend(saturday) move back to Friday.
                            if (prevWeekDay.DayOfWeek == DayOfWeek.Saturday)
                            {
                                prevWeekDay = prevWeekDay.AddDays(-1);
                                lastWeekendSunday = prevWeekDay.AddDays(-5);
                            }

                            // if today is weekend(sunday) move back to Friday.
                            else if (prevWeekDay.DayOfWeek == DayOfWeek.Sunday)
                            {
                                prevWeekDay = prevWeekDay.AddDays(-2);
                                lastWeekendSunday = prevWeekDay.AddDays(-5);
                            }

                            // Weekend Logic
                            DateTime lastWeekendSaturday = lastWeekendSunday.AddDays(-1);
                            List<DateTime> lastWeekendDays = new List<DateTime>();
                            lastWeekendDays.Add(lastWeekendSaturday);
                            lastWeekendDays.Add(lastWeekendSunday);

                            using (SqlTransaction sqlTransaction = sqlConnection.BeginTransaction())
                            {
                                long nonWorkingHourWeekendConsumption = 0;

                                foreach (var day in lastWeekendDays)
                                {
                                    DateTime startdayTime = day.AddHours(5);
                                    DateTime endDayTime = startdayTime.AddHours(24).AddSeconds(-1);
                                    DayConsumptionInfo info = GetConsumption(startdayTime, endDayTime, sqlConnection, sqlTransaction, piServer.PiServerName);
                                    nonWorkingHourWeekendConsumption += info.NonWorkingHourConsumption;
                                }

                                if (nonWorkingHourWeekendConsumption > 0)
                                {
                                    long weekendConsumption = (nonWorkingHourWeekendConsumption / 2) * 52; // here 52 is for weeks in a year

                                    GenerateConsumptionAlertAndNotification(sqlConnection, sqlTransaction, weekendConsumption, piServer);
                                }

                                // Previous Week Day Logic
                                DateTime startTime = prevWeekDay.AddHours(5);
                                DateTime endTime = startTime.AddHours(24).AddSeconds(-1);
                                DayConsumptionInfo weekDayInfo = GetConsumption(startTime, endTime, sqlConnection, sqlTransaction, piServer.PiServerName);

                                if (weekDayInfo.NonWorkingHourConsumption > 0)
                                {
                                    long weekdayConsumption = (weekDayInfo.NonWorkingHourConsumption / 2) * 365;

                                    GenerateConsumptionAlertAndNotification(sqlConnection, sqlTransaction, weekdayConsumption, piServer, false);
                                }

                                sqlTransaction.Commit();
                            }
                        }
                        catch (Exception e)
                        {
                            var errorMsg = string.Format("ConsumptionAlertJob Error : Consumption alert failed for pi server - {0}", piServer.PiServerName);

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

        private static DayConsumptionInfo GetConsumption(DateTime startTime, DateTime endTime, SqlConnection sqlConnection, SqlTransaction sqlTransaction, string piServerName)
        {
            var query = "SELECT Timestamp, Daily_KWH_System FROM LiveData WHERE Timestamp > @startTime AND Timestamp <= @endTime and PiServerName = @PiServerName ORDER BY Timestamp";
            List<AzureLiveData> meterDataList = new List<AzureLiveData>();

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
            {
                sqlCommand.Parameters.Add(new SqlParameter("@startTime", startTime.ToString(Constants.DATE_TIME_FORMAT)));
                sqlCommand.Parameters.Add(new SqlParameter("@endTime", endTime.ToString(Constants.DATE_TIME_FORMAT)));
                sqlCommand.Parameters.AddWithValue("@PiServerName", piServerName);

                using (SqlDataReader result = sqlCommand.ExecuteReader())
                {
                    while (result.Read())
                    {
                        AzureLiveData data = new AzureLiveData();

                        data.Timestamp = SqlTypeConverter.ToDateTime(result["Timestamp"]);
                        data.Daily_KWH_System = SqlTypeConverter.ToDouble(result["Daily_KWH_System"]);

                        meterDataList.Add(data);
                    }

                    result.Close();
                }
            }

            if (meterDataList.Count == 0)
            {
                return new DayConsumptionInfo();
            }

            DateTime startDate = meterDataList.First().Timestamp.Date;
            DateTime endDate = meterDataList.Last().Timestamp.Date;

            var activeHourddataOfDay = meterDataList.Where(x => x.Timestamp.Date == startDate && x.Timestamp.Hour < 22).Select(x => x.Daily_KWH_System);
            var inActiveHourSameDayData = meterDataList.Where(x => x.Timestamp.Date == startDate && x.Timestamp.Hour >= 22).Select(x => x.Daily_KWH_System);
            var inActiveHourDifferentDayData = meterDataList.Where(x => x.Timestamp.Date == endDate).Select(x => x.Daily_KWH_System);

            long activeHourConsumption = activeHourddataOfDay.Count() > 0 ? Convert.ToInt64(activeHourddataOfDay.Max() - activeHourddataOfDay.Min()) : default(long);
            long inActiveHourSameDayConsumption = inActiveHourSameDayData.Count() > 0 ? Convert.ToInt64(inActiveHourSameDayData.Max() - inActiveHourSameDayData.Min()) : default(long);
            long inActiveHourDifferentDayConsumption = inActiveHourDifferentDayData.Count() > 0 ? Convert.ToInt64(inActiveHourDifferentDayData.Max() - inActiveHourDifferentDayData.Min()) : default(long);

            return new DayConsumptionInfo()
            {
                ActiveHourConsumption = activeHourConsumption,
                NonWorkingHourConsumption = inActiveHourSameDayConsumption + inActiveHourDifferentDayConsumption
            };
        }

        private static void GenerateConsumptionAlertAndNotification(SqlConnection sqlConnection, SqlTransaction sqlTransaction, double consumption, PiServer piServer, bool isWeekEndConsumption = true)
        {
            var alertService = new AlertService();
            var notificationService = new NotificationService();

            double costTobeSaved = consumption * ConfigurationSettings.MeterKwhCost;
            string alertMessage = string.Format(Constants.RECOMMENDATION_ALERT_MESSAGE, costTobeSaved, isWeekEndConsumption ? "weekend" : "weekday");

            var alert = new Alert
            {
                AlertType = Constants.ALERT_TYPE_RECOMMENDATION,
                Description = alertMessage,
                TimeStamp = piServer.PiServerCurrentDateTime,
                PiServerName = piServer.PiServerName
            };
            alertService.AddNewAlert(sqlConnection, sqlTransaction, alert);
            notificationService.SendNotification(alertMessage, sqlConnection, sqlTransaction);

            Console.WriteLine("ConsumptionAlertJob RowInserted : Created alert and notification for PiServer - {0}.", piServer.PiServerName);
        }
    }
}
