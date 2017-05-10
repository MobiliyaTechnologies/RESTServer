namespace RestService.Utilities
{
    using System.Configuration;

    public static class ApiConfiguration
    {
        public static readonly string NotificationAuthorizationKey = GetConfigData("NotificationAuthorizationKey");
        public static readonly string NotificationSender = GetConfigData("NotificationSender");
        public static readonly string NotificationReceiver = GetConfigData("NotificationReceiver");
        public static readonly string NotificationURL = GetConfigData("NotificationURL");
        public static readonly string NotificationClickAction = GetConfigData("NotificationClickAction");
        public static readonly string NotificationIcon = GetConfigData("NotificationIcon");

        public static readonly string B2cAadInstance = GetConfigData("b2c:AadInstance");
        public static readonly string B2cTenant = GetConfigData("b2c:Tenant");
        public static readonly string B2cClientId = GetConfigData("b2c:ClientId");
        public static readonly string B2cSignUpPolicy = GetConfigData("b2c:SignUpPolicyId");
        public static readonly string B2cSignInPolicy = GetConfigData("b2c:SignInPolicyId");

        private static string GetConfigData(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}