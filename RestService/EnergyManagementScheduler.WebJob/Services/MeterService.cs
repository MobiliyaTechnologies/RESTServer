namespace EnergyManagementScheduler.WebJob.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using EnergyManagementScheduler.WebJob.Contracts;
    using EnergyManagementScheduler.WebJob.Utilities;

    public class MeterService
    {
        public List<PiServer> GetPiServerList(SqlConnection sqlConnection)
        {
            var piServerList = new List<PiServer>();
            var piserverQuery = "SELECT PiServerName, UTCConversionTime FROM PiServer where datalength(PiServerName) > 0";

            using (SqlCommand cmd = new SqlCommand(piserverQuery, sqlConnection))
            {
                using (SqlDataReader result = cmd.ExecuteReader())
                {
                    while (result.Read())
                    {
                        piServerList.Add(new PiServer
                        {
                            PiServerName = SqlTypeConverter.ToString(result["PiServerName"]),
                            UTCConversionTime = SqlTypeConverter.ToDouble(result["UTCConversionTime"]),
                        });
                    }

                    result.Close();
                }
            }

            return piServerList;
        }

        public List<string> GetMeterListByPiServer(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string piServerName)
        {
            List<string> meterList = new List<string>();
            string query = "select PowerScout from MeterDetails where PiServerName = @PiServerName";

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
            {
                sqlCommand.Parameters.AddWithValue("@PiServerName", piServerName);

                using (SqlDataReader result = sqlCommand.ExecuteReader())
                {
                    while (result.Read())
                    {
                        meterList.Add(SqlTypeConverter.ToString(result["PowerScout"]));
                    }

                    result.Close();
                }
            }

            return meterList;
        }

        public List<DailyConsumptionDetail> GetDailyConsumptionDetails(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string powerScout, string piServerName, DateTime? timestamp = null)
        {
            var query = "select Breaker_details, Timestamp, Building, Temperature, PowerScout, Visibility from DailyConsumptionDetails where PowerScout = @param1 and  PiServerName = @PiServerName";

            if (timestamp.HasValue)
            {
                query += " And Timestamp > @param2";
            }

            var detailConsumptionDetails = new List<DailyConsumptionDetail>();
            DailyConsumptionDetail detailConsumptionDetail = null;

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
            {
                sqlCommand.Parameters.AddWithValue("@param1", powerScout);
                sqlCommand.Parameters.AddWithValue("@PiServerName", piServerName);

                if (timestamp.HasValue)
                {
                    sqlCommand.Parameters.AddWithValue("@param2", timestamp);
                }

                using (SqlDataReader result = sqlCommand.ExecuteReader())
                {
                    while (result.Read())
                    {
                        detailConsumptionDetail = new DailyConsumptionDetail();

                        detailConsumptionDetail.Breaker_details = SqlTypeConverter.ToString(result["Breaker_details"]);
                        detailConsumptionDetail.Timestamp = SqlTypeConverter.ToDateTime(result["Timestamp"]);
                        detailConsumptionDetail.Temperature = SqlTypeConverter.ToDouble(result["Temperature"]);
                        detailConsumptionDetail.Building = SqlTypeConverter.ToString(result["Building"]);
                        detailConsumptionDetail.PowerScout = SqlTypeConverter.ToString(result["PowerScout"]);
                        detailConsumptionDetail.Visibility = SqlTypeConverter.ToDouble(result["Visibility"]);

                        detailConsumptionDetails.Add(detailConsumptionDetail);
                    }

                    result.Close();
                }
            }

            return detailConsumptionDetails;
        }

        public DailyConsumptionDetail GetTodaysDailyConsumptionDetails(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string powerScout, PiServer piServer)
        {
            var query = "select top(1) Breaker_details, Timestamp, Building, Temperature, PowerScout, Visibility from DailyConsumptionDetails where PowerScout = @param1 and CAST(Timestamp AS DATE) = @param2 and  PiServerName = @PiServerName";

            DailyConsumptionDetail detailConsumptionDetail = null;

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
            {
                sqlCommand.Parameters.AddWithValue("@param1", powerScout);
                sqlCommand.Parameters.AddWithValue("@param2", piServer.PiServerCurrentDateTime.AddDays(-1).Date.ToString("yyyy-MM-dd"));

                sqlCommand.Parameters.AddWithValue("@PiServerName", piServer.PiServerName);

                using (SqlDataReader result = sqlCommand.ExecuteReader())
                {
                    while (result.Read())
                    {
                        detailConsumptionDetail = new DailyConsumptionDetail();

                        detailConsumptionDetail.Breaker_details = SqlTypeConverter.ToString(result["Breaker_details"]);
                        detailConsumptionDetail.Timestamp = SqlTypeConverter.ToDateTime(result["Timestamp"]);
                        detailConsumptionDetail.Temperature = SqlTypeConverter.ToDouble(result["Temperature"]);
                        detailConsumptionDetail.Building = SqlTypeConverter.ToString(result["Building"]);
                        detailConsumptionDetail.PowerScout = SqlTypeConverter.ToString(result["PowerScout"]);
                        detailConsumptionDetail.Visibility = SqlTypeConverter.ToDouble(result["Visibility"]);
                    }

                    result.Close();
                }
            }

            return detailConsumptionDetail;
        }

        public void AddDailyConsumptionPrediction(DailyConsumptionPrediction dailyConsumptionPrediction, SqlConnection sqlConnection, SqlTransaction sqlTransaction, string piServerName)
        {

            using (SqlCommand sqlCommand = new SqlCommand("SP_InsertUpdateDailyConsumptionPrediction", sqlConnection, sqlTransaction))
            {
                sqlCommand.Parameters.AddWithValue("@PowerScout", dailyConsumptionPrediction.PowerScout);
                sqlCommand.Parameters.AddWithValue("@Breaker_details", dailyConsumptionPrediction.Breaker_details);
                sqlCommand.Parameters.AddWithValue("@Timestamp", dailyConsumptionPrediction.Timestamp);
                sqlCommand.Parameters.AddWithValue("@Daily_Predicted_KWH_System", dailyConsumptionPrediction.Daily_Predicted_KWH_System);
                sqlCommand.Parameters.AddWithValue("@Building", dailyConsumptionPrediction.Building);
                sqlCommand.Parameters.AddWithValue("@PiServerName", piServerName);

                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.ExecuteNonQuery();
            }
        }

        public WeeklyConsumptionPrediction GetWeeklyConsumptionPredictionForGivenDate(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string powerScout, DateTime startDate, string piServerName)
        {
            var query = "select top(1) * from WeeklyConsumptionPrediction where PowerScout = @param1 and CAST(Start_Time AS DATE) = @param2 and PiServerName = @PiServerName";

            WeeklyConsumptionPrediction weeklyConsumptionPrediction = null;

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
            {
                sqlCommand.Parameters.AddWithValue("@param1", powerScout);
                sqlCommand.Parameters.AddWithValue("@param2", startDate.Date.ToString("yyyy-MM-dd"));
                sqlCommand.Parameters.AddWithValue("@PiServerName", piServerName);

                using (SqlDataReader result = sqlCommand.ExecuteReader())
                {
                    while (result.Read())
                    {
                        weeklyConsumptionPrediction = new WeeklyConsumptionPrediction();

                        weeklyConsumptionPrediction.Breaker_details = SqlTypeConverter.ToString(result["Breaker_details"]);
                        weeklyConsumptionPrediction.Start_Time = SqlTypeConverter.ToDateTime(result["Start_Time"]);
                        weeklyConsumptionPrediction.End_Time = SqlTypeConverter.ToDateTime(result["End_Time"]);
                        weeklyConsumptionPrediction.Building = SqlTypeConverter.ToString(result["Building"]);
                        weeklyConsumptionPrediction.PowerScout = SqlTypeConverter.ToString(result["PowerScout"]);
                        weeklyConsumptionPrediction.Id = SqlTypeConverter.ToInt32(result["Id"]);
                        weeklyConsumptionPrediction.Weekly_Predicted_KWH_System = SqlTypeConverter.ToDouble(result["Weekly_Predicted_KWH_System"]);
                    }

                    result.Close();
                }
            }

            return weeklyConsumptionPrediction;
        }

        public void AddWeeklyConsumptionPrediction(WeeklyConsumptionPrediction weeklyConsumptionPrediction, SqlConnection sqlConnection, SqlTransaction sqlTransaction, string piServerName)
        {
            var query = "INSERT INTO [dbo].[WeeklyConsumptionPrediction]([PowerScout],[Breaker_details],[Start_Time],[End_Time],[Weekly_Predicted_KWH_System],[Building], [PiServerName]) VALUES(@param1, @param2, @param3, @param4, @param5,@param6, @PiServerName)";

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
            {
                sqlCommand.Parameters.AddWithValue("@param1", weeklyConsumptionPrediction.PowerScout);
                sqlCommand.Parameters.AddWithValue("@param2", weeklyConsumptionPrediction.Breaker_details);
                sqlCommand.Parameters.AddWithValue("@param3", weeklyConsumptionPrediction.Start_Time);
                sqlCommand.Parameters.AddWithValue("@param4", weeklyConsumptionPrediction.End_Time);
                sqlCommand.Parameters.AddWithValue("@param5", weeklyConsumptionPrediction.Weekly_Predicted_KWH_System);
                sqlCommand.Parameters.AddWithValue("@param6", weeklyConsumptionPrediction.Building ?? string.Empty);
                sqlCommand.Parameters.AddWithValue("@PiServerName", piServerName);

                sqlCommand.ExecuteNonQuery();
            }
        }

        public void UpdateWeeklyConsumptionPrediction(WeeklyConsumptionPrediction weeklyConsumptionPrediction, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            var query = "UPDATE [dbo].[WeeklyConsumptionPrediction] set [Start_Time] = @param1, [End_Time] = @param2, [Weekly_Predicted_KWH_System] = @param3 where [Id] = @param4";

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
            {
                sqlCommand.Parameters.AddWithValue("@param1", weeklyConsumptionPrediction.Start_Time);
                sqlCommand.Parameters.AddWithValue("@param2", weeklyConsumptionPrediction.End_Time);
                sqlCommand.Parameters.AddWithValue("@param3", weeklyConsumptionPrediction.Weekly_Predicted_KWH_System);
                sqlCommand.Parameters.AddWithValue("@param4", weeklyConsumptionPrediction.Id);

                sqlCommand.ExecuteNonQuery();
            }
        }

        public ReadOnlyDictionary<string, DateTime> GetDailyPredictionProcessedStatus(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string piServerName)
        {
            var query = "select PowerScout, Max(Timestamp) as Timestamp from DailyConsumptionPrediction where PiServerName = @PiServerName group by PowerScout";

            var dailyPredictionStatus = new Dictionary<string, DateTime>();

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
            {
                sqlCommand.Parameters.AddWithValue("@PiServerName", piServerName);

                using (SqlDataReader result = sqlCommand.ExecuteReader())
                {
                    while (result.Read())
                    {
                        var meter = SqlTypeConverter.ToString(result["PowerScout"]);
                        var timestamp = SqlTypeConverter.ToDateTime(result["Timestamp"]);

                        dailyPredictionStatus.Add(meter, timestamp);
                    }

                    result.Close();
                }
            }

            return new ReadOnlyDictionary<string, DateTime>(dailyPredictionStatus);
        }

        public ReadOnlyDictionary<string, DateTime> GetDailyConsumptionProcessedStatus(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string piServerName)
        {
            var query = "select PowerScout, Max(Timestamp) as Timestamp from DailyConsumptionDetails where PiServerName = @PiServerName group by PowerScout";

            var dailyConsumptionStatus = new Dictionary<string, DateTime>();

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
            {
                sqlCommand.Parameters.AddWithValue("@PiServerName", piServerName);

                using (SqlDataReader result = sqlCommand.ExecuteReader())
                {
                    while (result.Read())
                    {
                        var meter = SqlTypeConverter.ToString(result["PowerScout"]);
                        var timestamp = SqlTypeConverter.ToDateTime(result["Timestamp"]);

                        dailyConsumptionStatus.Add(meter, timestamp);
                    }

                    result.Close();
                }
            }

            return new ReadOnlyDictionary<string, DateTime>(dailyConsumptionStatus);
        }

        public ReadOnlyDictionary<string, DateTime> GetMonthlyConsumptionProcessedStatus(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string piServerName)
        {
            var query = "select PowerScout, Max(Timestamp) as Timestamp from MonthlyConsumptionDetails where PiServerName = @PiServerName group by PowerScout";

            var monthlyConsumptionStatus = new Dictionary<string, DateTime>();

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
            {
                sqlCommand.Parameters.AddWithValue("@PiServerName", piServerName);

                using (SqlDataReader result = sqlCommand.ExecuteReader())
                {
                    while (result.Read())
                    {
                        var meter = SqlTypeConverter.ToString(result["PowerScout"]);
                        var timestamp = SqlTypeConverter.ToDateTime(result["Timestamp"]);

                        monthlyConsumptionStatus.Add(meter, timestamp);
                    }

                    result.Close();
                }
            }

            return new ReadOnlyDictionary<string, DateTime>(monthlyConsumptionStatus);
        }

        public DateTime GetDailyConsumptionInitialStatus(SqlConnection sqlConnection, SqlTransaction sqlTransaction, string piServerName, string meter)
        {
            var query = "select Min(Timestamp) as Timestamp from DailyConsumptionDetails where PiServerName = @PiServerName and Powerscout = @PowerScout group by PowerScout";

            DateTime dailyConsumptioninitialStatus = default(DateTime);

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
            {
                sqlCommand.Parameters.AddWithValue("@PiServerName", piServerName);
                sqlCommand.Parameters.AddWithValue("@PowerScout", meter);

                using (SqlDataReader result = sqlCommand.ExecuteReader())
                {
                    while (result.Read())
                    {
                        dailyConsumptioninitialStatus = SqlTypeConverter.ToDateTime(result["Timestamp"]);
                    }

                    result.Close();
                }
            }

            return dailyConsumptioninitialStatus;
        }

        public async Task<AnomalyServiceResponse> GetAnomalyPrediction(DailyConsumptionDetail dailyConsumptionDetail, SqlConnection sqlConnection, SqlTransaction sqlTransaction, string piServerName, bool isDailyPrediction = true)
        {
            string predictionDate = dailyConsumptionDetail.Timestamp.ToString("yyyy-MM-ddTHH:mm:ssZ");

            if (isDailyPrediction)
            {
                predictionDate = dailyConsumptionDetail.Timestamp.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ssZ");
            }

            AnomalyServiceRequest requestmodel = new AnomalyServiceRequest()
            {
                Inputs = new Inputs()
                {
                    input1 = new Input1()
                    {
                        ColumnNames = new List<string> {"Id", "AMPS_SYSTEM_AVG", "Building","Breaker_details", "Daily_electric_cost",
                                    "Daily_KWH_System", "Monthly_electric_cost", "Monthly_KWH_System", "PowerScout",
                                    "Temperature", "Timestamp", "Visibility", "kW_System", "PiServerName"},

                        Values = new List<List<string>>
                            {
                                new List<string>
                                {
                                    "0", "0", dailyConsumptionDetail.Building, dailyConsumptionDetail.Breaker_details, "0", "0",
                                        "0", "0",
                                        dailyConsumptionDetail.PowerScout, dailyConsumptionDetail.Temperature.ToString(), predictionDate,
                                        dailyConsumptionDetail.Visibility.ToString(), "0", piServerName
                                }
                            }
                    }
                }
            };

            var azureMLConfigurationService = new AzureMLConfigurationService();
            var predictionConfig = azureMLConfigurationService.GetPredictionConfigData(sqlConnection, sqlTransaction);
            return await AzureMLClient.CallAzureMLAsync(requestmodel, predictionConfig.AzureMlDailyPredictionApiURL, predictionConfig.AzureMlDailyPredictionApiKey);
        }
    }
}
