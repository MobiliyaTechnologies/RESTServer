namespace EnergyManagementScheduler.WebJob.Utilities
{
    public class Constants
    {
        public const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public const string ANOMALY_ALERT_MESSAGE = "There are {0}(s) detected in past weekday({1} to {2})";
        public const string ALERT_TYPE_ANOMALY = "Anomaly";
        public const string ALERT_TYPE_RECOMMENDATION = "Recommendation";
        public const string RECOMMENDATION_ALERT_MESSAGE = "You can save ${0} per annum,if you reduce the consumption on {1} nights between (10 PM to 5 AM) by 50%";
        public const string CONSUMPTION_ALERT_NOTIFICATION_HEADING = "Consumption Alert";

        public const string MONTHLY_CONSUMPTION_MONTH_FORMAT = "MMM";
        public const string DEFAULT_DATE = "1990-01-01 00:00:00.000";

        public const string FirebaseApplicationConfiguration = "Firebase";

        public const string NotificationAuthorizationEntryKey = "ApiKey";
        public const string NotificationSenderEntryKey = "NotificationSender";
        public const string NotificationReceiverEntryKey = "NotificationReceiver";

    }
}
