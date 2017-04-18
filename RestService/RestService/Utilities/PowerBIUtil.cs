namespace RestService.Utilities
{
    using System.Collections.Generic;
    using RestService.Models;

    public static class PowerBIUtil
    {
        public static readonly Dictionary<string, MeterURLKeyModel> MeterURLKeyDictionary = new Dictionary<string, MeterURLKeyModel>();
        private const string WeatherURL = "";

        static PowerBIUtil()
        {
            PopulateURL_P371602068();
            PopulateURL_P371602070();
            PopulateURL_P371602072();
            PopulateURL_P371602073();
            PopulateURL_P371602075();
            PopulateURL_P371602077();
            PopulateURL_P371602079();
        }

        private static void PopulateURL_P371602068()
        {
            MeterURLKeyModel keyValue = new MeterURLKeyModel();
            keyValue.Weather = WeatherURL;
            keyValue.QuarterlyConsumption = "";
            keyValue.LastQuarterlyConsumption = "";
            keyValue.MonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=d2ac4bab-1867-48eb-a640-22655ea791c4&tileId=a7d25902-9ab1-48c7-b256-ea7f5e50a8e1";
            keyValue.LastMonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=d2ac4bab-1867-48eb-a640-22655ea791c4&tileId=12d69022-7b7f-4eb5-997f-727e87ec19e4";
            keyValue.WeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=d2ac4bab-1867-48eb-a640-22655ea791c4&tileId=30a49d22-eb3e-4a12-b050-080e804527aa";
            keyValue.LastWeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=d2ac4bab-1867-48eb-a640-22655ea791c4&tileId=9f66f329-15a2-4d5b-82af-e74009a4aef1";
            keyValue.DailyConsumption = "https://app.powerbi.com/embed?dashboardId=d2ac4bab-1867-48eb-a640-22655ea791c4&tileId=9609cd02-715e-4d13-b7b3-14956350b67a";
            keyValue.YesterdayConsumption = "https://app.powerbi.com/embed?dashboardId=d2ac4bab-1867-48eb-a640-22655ea791c4&tileId=0781911b-d988-4b37-893a-5b9be41fe07e";

            keyValue.MonthlyKWh = "https://app.powerbi.com/embed?dashboardId=d2ac4bab-1867-48eb-a640-22655ea791c4&tileId=70bdef51-f6d6-4c91-a275-4bcc795f925a";
            keyValue.MonthlyCost = "https://app.powerbi.com/embed?dashboardId=d2ac4bab-1867-48eb-a640-22655ea791c4&tileId=7d206df8-9c25-4d86-b834-4c9c04d3ce83";
            keyValue.DayWiseConsumption = "https://app.powerbi.com/embed?dashboardId=d2ac4bab-1867-48eb-a640-22655ea791c4&tileId=89d88778-d35c-4ccd-965a-e23df46aa493";
            keyValue.PeriodWiseConsumption = "https://app.powerbi.com/embed?dashboardId=d2ac4bab-1867-48eb-a640-22655ea791c4&tileId=03b7765c-bf65-4481-a82f-763988b37791";

            keyValue.Report = "https://app.powerbi.com/reportEmbed?reportId=7aadbc0e-afb1-4c01-bf88-55d4971ddb40";

            MeterURLKeyDictionary.Add("P371602068", keyValue);
        }

        private static void PopulateURL_P371602070()
        {
            MeterURLKeyModel keyValue = new MeterURLKeyModel();
                keyValue.Weather = WeatherURL;
                keyValue.QuarterlyConsumption = "";
                keyValue.LastQuarterlyConsumption = "";
                keyValue.MonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=849cf81d-c017-4ebe-bc3c-8be47ece43d2";
                keyValue.LastMonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=ee726709-8aa3-4247-979c-e54d1ed824ba";
                keyValue.WeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=85b4389b-2652-4b2f-8f7c-d9e5104c8cf4";
                keyValue.LastWeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=e4ae2321-d888-491e-8b54-e8e0d4ab1e4f";
                keyValue.DailyConsumption = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=ab5a5c9b-7006-4fe7-a7f4-7f6b71a88a54";
                keyValue.YesterdayConsumption = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=c6497fd9-9b8d-4268-be17-a1e4f5612031";

                keyValue.MonthlyKWh = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=84e6fdd5-b164-4635-8cbc-1562ebd96d2c";
                keyValue.MonthlyCost = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=e7b6991e-7341-4a8d-969c-875ae7a35115";
                keyValue.DayWiseConsumption = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=9124bdae-f0a5-4edc-a326-4cdff9083640";
                keyValue.PeriodWiseConsumption = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=715f3524-8c91-49a0-b76a-4fe3f1b59103";

                keyValue.Report = "https://app.powerbi.com/reportEmbed?reportId=8c74c0fb-9fb7-453a-a79e-0508bba70db2";
            MeterURLKeyDictionary.Add("P371602070", keyValue);
        }

        private static void PopulateURL_P371602072()
        {
            MeterURLKeyModel keyValue = new MeterURLKeyModel();
                keyValue.Weather = WeatherURL;
                keyValue.QuarterlyConsumption = "";
                keyValue.LastQuarterlyConsumption = "";
                keyValue.MonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=849cf81d-c017-4ebe-bc3c-8be47ece43d2";
                keyValue.LastMonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=ee726709-8aa3-4247-979c-e54d1ed824ba";

                keyValue.WeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=85b4389b-2652-4b2f-8f7c-d9e5104c8cf4";
                keyValue.LastWeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=e4ae2321-d888-491e-8b54-e8e0d4ab1e4f";

                keyValue.DailyConsumption = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=ab5a5c9b-7006-4fe7-a7f4-7f6b71a88a54";
                keyValue.YesterdayConsumption = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=c6497fd9-9b8d-4268-be17-a1e4f5612031";

                keyValue.MonthlyKWh = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=84e6fdd5-b164-4635-8cbc-1562ebd96d2c";
                keyValue.MonthlyCost = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=e7b6991e-7341-4a8d-969c-875ae7a35115";
                keyValue.DayWiseConsumption = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=9124bdae-f0a5-4edc-a326-4cdff9083640";
                keyValue.PeriodWiseConsumption = "https://app.powerbi.com/embed?dashboardId=472d1742-e075-45c6-97cc-2d64a8f7c367&tileId=715f3524-8c91-49a0-b76a-4fe3f1b59103";

                keyValue.Report = "https://app.powerbi.com/reportEmbed?reportId=b24ab241-1a11-431d-bf2e-53ea05638c7c";

            MeterURLKeyDictionary.Add("P371602072", keyValue);
        }

        private static void PopulateURL_P371602073()
        {
            MeterURLKeyModel  keyValue = new MeterURLKeyModel();
                keyValue.Weather = WeatherURL;
                keyValue.QuarterlyConsumption = "";
                keyValue.LastQuarterlyConsumption = "";

                keyValue.MonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=6ed10559-97e2-48b8-ab17-d2b8a612c5dc&tileId=4a38360e-3d10-4d32-b413-3b3c65c6b9ec";
                keyValue.LastMonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=6ed10559-97e2-48b8-ab17-d2b8a612c5dc&tileId=36084b50-ff4c-4f48-9185-baa7be3d1db2";

                keyValue.WeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=6ed10559-97e2-48b8-ab17-d2b8a612c5dc&tileId=23ec9bc0-3aea-4195-aacd-846164fdde41";
                keyValue.LastWeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=6ed10559-97e2-48b8-ab17-d2b8a612c5dc&tileId=3b96e189-a0db-4a00-be71-a72cdb9d7a6f";

                keyValue.DailyConsumption = "https://app.powerbi.com/embed?dashboardId=6ed10559-97e2-48b8-ab17-d2b8a612c5dc&tileId=5f6ddc44-be5a-49f5-bf11-4c56451ca282";
                keyValue.YesterdayConsumption = "https://app.powerbi.com/embed?dashboardId=6ed10559-97e2-48b8-ab17-d2b8a612c5dc&tileId=d4dc1655-6690-4fd1-b227-408b0cd9508a";

                keyValue.MonthlyKWh = "https://app.powerbi.com/embed?dashboardId=6ed10559-97e2-48b8-ab17-d2b8a612c5dc&tileId=c9c95496-3bb7-4018-a998-e50c0bda9589";
                keyValue.MonthlyCost = "https://app.powerbi.com/embed?dashboardId=6ed10559-97e2-48b8-ab17-d2b8a612c5dc&tileId=c8e5bfb6-8ac0-463c-b20c-6f0534861113";
                keyValue.DayWiseConsumption = "https://app.powerbi.com/embed?dashboardId=6ed10559-97e2-48b8-ab17-d2b8a612c5dc&tileId=b17996c0-f89c-4f95-954f-d145ac3835b2";
                keyValue.PeriodWiseConsumption = "https://app.powerbi.com/embed?dashboardId=6ed10559-97e2-48b8-ab17-d2b8a612c5dc&tileId=b7fe4c91-b76a-4d68-93e8-166930b31273";

                keyValue.Report = "https://app.powerbi.com/reportEmbed?reportId=96f08b80-3aae-4f19-9860-1cbd8ffbaa2d";

            MeterURLKeyDictionary.Add("P371602073", keyValue);
        }

        private static void PopulateURL_P371602075()
        {
            MeterURLKeyModel keyValue = new MeterURLKeyModel();
                keyValue.Weather = WeatherURL;
                keyValue.QuarterlyConsumption = "";
                keyValue.LastQuarterlyConsumption = "";

                keyValue.MonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=4ef68b94-654e-461a-9203-ebe080e12ef7&tileId=93b31c5b-0b58-4f6f-985c-dc3979f14173";
                keyValue.LastMonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=4ef68b94-654e-461a-9203-ebe080e12ef7&tileId=9bd39a49-e905-4283-98e5-62ec53b2980c";

                keyValue.WeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=4ef68b94-654e-461a-9203-ebe080e12ef7&tileId=eb9209c6-86ad-4f56-8525-011d7b2609da";
                keyValue.LastWeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=4ef68b94-654e-461a-9203-ebe080e12ef7&tileId=7d78e21e-2712-4fc4-9b2d-8216d7568588";

                keyValue.DailyConsumption = "https://app.powerbi.com/embed?dashboardId=4ef68b94-654e-461a-9203-ebe080e12ef7&tileId=9c7f17ac-fac6-4d77-8de7-777e5b770b48";
                keyValue.YesterdayConsumption = "https://app.powerbi.com/embed?dashboardId=4ef68b94-654e-461a-9203-ebe080e12ef7&tileId=8f50762e-ff0b-4c66-a20b-0fd2da405ab2";

                keyValue.MonthlyKWh = "https://app.powerbi.com/embed?dashboardId=4ef68b94-654e-461a-9203-ebe080e12ef7&tileId=f8c9df68-0afa-44c8-88cd-4c171a9ab16e";
                keyValue.MonthlyCost = "https://app.powerbi.com/embed?dashboardId=4ef68b94-654e-461a-9203-ebe080e12ef7&tileId=65843992-39f6-4a3f-8a35-1a36c1376bc1";
                keyValue.DayWiseConsumption = "https://app.powerbi.com/embed?dashboardId=4ef68b94-654e-461a-9203-ebe080e12ef7&tileId=004a0217-8b54-4fb1-94ca-f5b34d8a909a";
                keyValue.PeriodWiseConsumption = "https://app.powerbi.com/embed?dashboardId=4ef68b94-654e-461a-9203-ebe080e12ef7&tileId=6b471ed4-9dbe-4069-b048-6476f355ffbd";

                keyValue.Report = "https://app.powerbi.com/reportEmbed?reportId=0b6a83f7-c168-4053-a173-6544332dfb96";

            MeterURLKeyDictionary.Add("P371602075", keyValue);
        }

        private static void PopulateURL_P371602077()
        {
            MeterURLKeyModel keyValue = new MeterURLKeyModel();
                keyValue.Weather = WeatherURL;
                keyValue.QuarterlyConsumption = "";
                keyValue.LastQuarterlyConsumption = "";

                keyValue.MonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=d43d8ae2-247d-4f01-8007-3562e36d1cfa&tileId=94451cce-6364-4ef7-ae0c-03af25a77dd4";
                keyValue.LastMonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=d43d8ae2-247d-4f01-8007-3562e36d1cfa&tileId=02b63744-24ab-4dbe-8d20-0a5ede912a79";

                keyValue.WeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=d43d8ae2-247d-4f01-8007-3562e36d1cfa&tileId=791498e5-6d6a-4795-94db-ed3245c0dedb";
                keyValue.LastWeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=d43d8ae2-247d-4f01-8007-3562e36d1cfa&tileId=18760432-4b7e-4707-a795-a14ae3361783";

                keyValue.DailyConsumption = "https://app.powerbi.com/embed?dashboardId=d43d8ae2-247d-4f01-8007-3562e36d1cfa&tileId=75161e40-f103-44a9-ad9e-50a3c9efa911";
                keyValue.YesterdayConsumption = "https://app.powerbi.com/embed?dashboardId=d43d8ae2-247d-4f01-8007-3562e36d1cfa&tileId=75f93c18-5688-4a8b-9111-4d61387b7a91";

                keyValue.MonthlyKWh = "https://app.powerbi.com/embed?dashboardId=d43d8ae2-247d-4f01-8007-3562e36d1cfa&tileId=7feef563-d247-4f02-afa8-4974ee8e1a2c";
                keyValue.MonthlyCost = "https://app.powerbi.com/embed?dashboardId=d43d8ae2-247d-4f01-8007-3562e36d1cfa&tileId=ad82b0c8-4942-4c96-a766-6bd5d0eb5b36";
                keyValue.DayWiseConsumption = "https://app.powerbi.com/embed?dashboardId=d43d8ae2-247d-4f01-8007-3562e36d1cfa&tileId=b0ca63ef-e290-471b-a711-308de2e3a0f9";
                keyValue.PeriodWiseConsumption = "https://app.powerbi.com/embed?dashboardId=d43d8ae2-247d-4f01-8007-3562e36d1cfa&tileId=3b94f102-7b1b-4071-9d4c-7ebb4f0d234d";

                keyValue.Report = "https://app.powerbi.com/reportEmbed?reportId=31db8bf7-0509-4413-aac5-1ef7bbd29a38";

            MeterURLKeyDictionary.Add("P371602077", keyValue);
        }

        private static void PopulateURL_P371602079()
        {
            MeterURLKeyModel keyValue = new MeterURLKeyModel();
                keyValue.Weather = WeatherURL;
                keyValue.QuarterlyConsumption = "";
                keyValue.LastQuarterlyConsumption = "";

                keyValue.MonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=a55133bd-3fda-4248-9ace-4852e8b07b2b&tileId=aeafb4ad-abf9-4eea-a257-41d975b9f73f";
                keyValue.LastMonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=a55133bd-3fda-4248-9ace-4852e8b07b2b&tileId=019e0199-35d0-40af-9725-e73addbcf361";

                keyValue.WeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=a55133bd-3fda-4248-9ace-4852e8b07b2b&tileId=1e7f55d8-372b-40c1-9baf-a6a66b6defa7";
                keyValue.LastWeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=a55133bd-3fda-4248-9ace-4852e8b07b2b&tileId=ea40551f-c79a-463d-b738-ab9d2735f7c6";

                keyValue.DailyConsumption = "https://app.powerbi.com/embed?dashboardId=a55133bd-3fda-4248-9ace-4852e8b07b2b&tileId=9b230ef1-f47f-42ed-aa19-1fea315e749f";
                keyValue.YesterdayConsumption = "https://app.powerbi.com/embed?dashboardId=a55133bd-3fda-4248-9ace-4852e8b07b2b&tileId=b948cfde-60ee-4125-9fb8-21807886bcf0";

                keyValue.MonthlyKWh = "https://app.powerbi.com/embed?dashboardId=a55133bd-3fda-4248-9ace-4852e8b07b2b&tileId=868f9549-63b3-47a1-a135-21abed867d03";
                keyValue.MonthlyCost = "https://app.powerbi.com/embed?dashboardId=a55133bd-3fda-4248-9ace-4852e8b07b2b&tileId=2d975f61-3103-4dcb-9a85-2453f55a438f";
                keyValue.DayWiseConsumption = "https://app.powerbi.com/embed?dashboardId=a55133bd-3fda-4248-9ace-4852e8b07b2b&tileId=510ebefd-786a-4c36-97b0-ad63f9a6c54f";
                keyValue.PeriodWiseConsumption = "https://app.powerbi.com/embed?dashboardId=a55133bd-3fda-4248-9ace-4852e8b07b2b&tileId=08415552-9de2-4936-812a-e4e3371ab76b";

                keyValue.Report = "https://app.powerbi.com/reportEmbed?reportId=d2d26e40-a0de-486d-b95b-118b1eed49b9";

            MeterURLKeyDictionary.Add("P371602079", keyValue);
        }
    }
}