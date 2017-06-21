namespace RestService.Utilities
{
    using System;
    using System.Configuration;

    public static class ApiConfiguration
    {
        public static readonly string NotificationURL = GetConfigData("NotificationURL");
        public static readonly string NotificationClickAction = GetConfigData("NotificationClickAction");
        public static readonly string NotificationIcon = GetConfigData("NotificationIcon");

        public static readonly string B2cAadInstance = GetConfigData("b2c:AadInstance");
        public static readonly string B2cTenant = GetConfigData("b2c:Tenant");
        public static readonly string B2cClientId = GetConfigData("b2c:ClientId");
        public static readonly string B2cSignUpPolicy = GetConfigData("b2c:SignUpPolicyId");
        public static readonly string B2cSignInPolicy = GetConfigData("b2c:SignInPolicyId");
        public static readonly string B2cClientSecret = GetConfigData("b2c:ClientSecret");
        public static readonly string B2cChangePasswordPolicy = GetConfigData("b2c:ChangePasswordPolicy");

        public static readonly string B2cMobileAuthorizeURL = GetConfigData("b2c:MobileAuthorizeURL");
        public static readonly string B2cMobileTokenURL = GetConfigData("b2c:MobileTokenURL");
        public static readonly string B2cMobileTokenURLIOS = GetConfigData("b2c:MobileTokenURLIOS");
        public static readonly string B2cMobileChangePasswordURL = GetConfigData("b2c:MobileChangePasswordURL");
        public static readonly string B2cMobileRedirectUrl = GetConfigData("b2c:MobileRedirectUrl");

        public static readonly string EmailHost = GetConfigData("EmailHost");
        public static readonly string EmailSender = GetConfigData("EmailSender");
        public static readonly string EmailHostPassword = GetConfigData("EmailHostPassword");
        public static readonly int EmailHostPort = Convert.ToInt32(GetConfigData("EmailHostPort"));

        public static readonly string BlobStorageConnectionString = GetConfigData("BlobStorageConnectionString");
        public static readonly string BlobPrivateContainer = GetConfigData("BlobPrivateContainer");
        public static readonly string BlobPrefix = GetConfigData("BlobPrefix");

        public static readonly string BlobPublicContainer = GetConfigData("BlobPublicContainer");

        private static string GetConfigData(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}