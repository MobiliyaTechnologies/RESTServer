namespace RestService.Models
{
    using RestService.Utilities;

    public class NotificationModel
    {
        public string NotificationAuthorizationKey { get; set; }

        public string NotificationSender { get; set; }

        public string NotificationReceiver { get; set; }

        public string NotificationClickAction
        {
            get
            {
                return ApiConfiguration.NotificationClickAction;
            }
        }

        public string NotificationURL
        {
            get
            {
                return ApiConfiguration.NotificationURL;
            }
        }

        public string NotificationTitle { get; set; }

        public string NotificationMessage { get; set; }
    }
}