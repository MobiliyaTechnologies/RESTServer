namespace RestService.Utilities
{
    public static class ApiConstant
    {
        public static readonly string B2cClaimObjectIdentifier = "http://schemas.microsoft.com/identity/claims/objectidentifier";

        public static readonly string B2cClaimEmail = "emails";

        public static readonly string B2cClaimFirstName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";

        public static readonly string B2cClaimLastName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";

        public static readonly string BlobStorageApplicationConfiguration = "BlobStorage";

        public static readonly string BlobStorageConnectionStringKey = "BlobStorageConnectionString";

        public static readonly string FirebaseApplicationConfiguration = "Firebase";

        public static readonly string NotificationClickActionKey = "NotificationClickAction";

        public static readonly string CsvFileType = ".csv";
    }
}