namespace RestService.Utilities
{
    using System.Configuration;

    public static class ApiConfiguration
    {
        public static readonly string ApplicationId = GetConfigData("ApplicationId");

        public static readonly string NotificationSender = GetConfigData("NotificationSender");

        public static readonly string NotificationReceiver = GetConfigData("NotificationReceiver");

        public static readonly string NotificationURL = GetConfigData("NotificationURL");

        public static readonly string NotificationClickActionURL = GetConfigData("NotificationClickActionURL");

        public static readonly string MonthlyConsumptionKWh = GetConfigData("MonthlyConsumptionKWh");

        public static readonly string MonthlyConsumptionCost = GetConfigData("MonthlyConsumptionCost");

        private static string GetConfigData(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}