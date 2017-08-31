namespace EnergyManagementScheduler.WebJob.Contracts
{
    using EnergyManagementScheduler.WebJob.Utilities;

    public class NotificatioConfiguration
    {
        public string NotificationAuthorizationKey { get; set; }

        public string NotificationSender { get; set; }

        public string NotificationReceiver { get; set; }

        public string NotificationClickAction
        {
            get
            {
                return ConfigurationSettings.NotificationClickAction;
            }
        }

        public string NotificationURL
        {
            get
            {
                return ConfigurationSettings.NotificationUrl;
            }
        }

        public string NotificationTitle
        {
            get
            {
                return Constants.CONSUMPTION_ALERT_NOTIFICATION_HEADING;
            }
        }
    }
}
