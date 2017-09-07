namespace EnergyManagementScheduler.WebJob.Utilities
{
    using System;
    using System.Configuration;

    public static class ConfigurationSettings
    {
        public static readonly string NotificationUrl = GetConfigData("NotificationUrl");
        public static readonly string NotificationClickAction = GetConfigData("NotificationClickAction");
        public static readonly string DbConnectionString = GetConfigData("DbConnectionString");
        public static readonly double MeterKwhCost = Convert.ToDouble(GetConfigData("MeterKwhCost"));
        public static readonly double AnomalyThreshold = Convert.ToDouble(GetConfigData("AnomalyThreshold"));
        public static readonly string ApplicationInsightsInstrumentationKey = GetConfigData("applicationInsights:InstrumentationKey");

        public static bool IsValidConfig()
        {
            return
                !(string.IsNullOrWhiteSpace(DbConnectionString) ||
                string.IsNullOrWhiteSpace(GetConfigData("MeterKwhCost")) || string.IsNullOrWhiteSpace(GetConfigData("AnomalyThreshold"))
                || (ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"] == null || string.IsNullOrWhiteSpace(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString))
                || (ConfigurationManager.ConnectionStrings["AzureWebJobsDashboard"] == null
                || string.IsNullOrWhiteSpace(ConfigurationManager.ConnectionStrings["AzureWebJobsDashboard"].ConnectionString)));
        }

        private static string GetConfigData(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
