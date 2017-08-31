// <copyright file="AzureMLConfigurationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EnergyManagementScheduler.WebJob.Services
{
    using System.Data.SqlClient;
    using EnergyManagementScheduler.WebJob.Contracts;
    using EnergyManagementScheduler.WebJob.Utilities;

    public class AzureMLConfigurationService
    {
        public AnomalyConfigInfo GetAnomalyConfigData(SqlConnection getConfigurationConnection, SqlTransaction sqlTransaction)
        {
            AnomalyConfigInfo configInfo = new AnomalyConfigInfo();

            string query = string.Format("Select ConfigurationValue,ConfigurationKey from ApplicationConfigurationEntry where ConfigurationKey IN('AzureMlAnomalyDetectionApiKey','AzureMlAnomalyDetectionApiUrl') ");
            using (SqlCommand sqlCommandConfiguration = new SqlCommand(query, getConfigurationConnection, sqlTransaction))
            {
                using (SqlDataReader configurationsqlDataReader = sqlCommandConfiguration.ExecuteReader())
                {
                    while (configurationsqlDataReader.Read())
                    {
                        switch (SqlTypeConverter.ToString(configurationsqlDataReader["ConfigurationKey"]))
                        {
                            case "AzureMlAnomalyDetectionApiKey":
                                configInfo.AzureMlAnomalyDetectionApiKey = SqlTypeConverter.ToString(configurationsqlDataReader["ConfigurationValue"]);
                                break;
                            case "AzureMlAnomalyDetectionApiUrl":
                                configInfo.AzureMlAnomalyDetectionApiUrl = SqlTypeConverter.ToString(configurationsqlDataReader["ConfigurationValue"]);
                                break;
                        }
                    }
                }
            }

            return configInfo;
        }

        public PredictionConfigInfo GetPredictionConfigData(SqlConnection getConfigurationConnection, SqlTransaction sqlTransaction)
        {
            PredictionConfigInfo configInfo = new PredictionConfigInfo();

            string query = string.Format("Select ConfigurationValue,ConfigurationKey from ApplicationConfigurationEntry where ConfigurationKey IN('AzureMlDailyPredictionApiKey','AzureMlDailyPredictionApiURL')");
            using (SqlCommand sqlCommandConfiguration = new SqlCommand(query, getConfigurationConnection, sqlTransaction))
            {
                using (SqlDataReader configurationsqlDataReader = sqlCommandConfiguration.ExecuteReader())
                {
                    while (configurationsqlDataReader.Read())
                    {
                        switch (SqlTypeConverter.ToString(configurationsqlDataReader["ConfigurationKey"]))
                        {
                            case "AzureMlDailyPredictionApiKey":
                                configInfo.AzureMlDailyPredictionApiKey = SqlTypeConverter.ToString(configurationsqlDataReader["ConfigurationValue"]);
                                break;
                            case "AzureMlDailyPredictionApiURL":
                                configInfo.AzureMlDailyPredictionApiURL = SqlTypeConverter.ToString(configurationsqlDataReader["ConfigurationValue"]);
                                break;
                        }
                    }
                }
            }

            return configInfo;
        }
    }
}
