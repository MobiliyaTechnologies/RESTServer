using RestService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestService.Utilities
{
    public class PowerBIUtil : MeterURLKey
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        const string WeatherURL = "";

        public MeterURLKey GetURL_P371602068()
        {
            MeterURLKey keyValue;
            try
            {
                log.Debug("GetURL_P371602068 called");
                keyValue = new MeterURLKey();
                keyValue.Weather = WeatherURL;
                keyValue.QuarterlyConsumption = "";
                keyValue.LastQuarterlyConsumption = "";
                keyValue.MonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=40306835-aee6-4b6a-9c06-9725f0824103&tileId=0f8871ea-7682-49f1-8522-71fc9b23b7d6";
                keyValue.LastMonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=40306835-aee6-4b6a-9c06-9725f0824103&tileId=fda51db5-58f8-44ab-8501-cbf588c74e6b";
                keyValue.WeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=40306835-aee6-4b6a-9c06-9725f0824103&tileId=8ef4203a-1f0e-4603-acf2-61ba47f6dd62";
                keyValue.LastWeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=40306835-aee6-4b6a-9c06-9725f0824103&tileId=4a1442d4-8bc0-404d-8e30-cf747c71365c";
                keyValue.DailyConsumption = "https://app.powerbi.com/embed?dashboardId=40306835-aee6-4b6a-9c06-9725f0824103&tileId=795b7a1f-35f5-47ad-86cc-9985237e33a3";
                keyValue.YesterdayConsumption = "https://app.powerbi.com/embed?dashboardId=40306835-aee6-4b6a-9c06-9725f0824103&tileId=92be8229-a892-473d-93ab-2028dae7e66d";

                keyValue.MonthlyKWh = "https://app.powerbi.com/embed?dashboardId=40306835-aee6-4b6a-9c06-9725f0824103&tileId=5276948c-7cc8-488e-a635-319a9e8bb8b3";
                keyValue.MonthlyCost = "https://app.powerbi.com/embed?dashboardId=40306835-aee6-4b6a-9c06-9725f0824103&tileId=95eeaba9-24fb-45e7-a85b-06fabd096b19";
                keyValue.DayWiseConsumption = "https://app.powerbi.com/embed?dashboardId=40306835-aee6-4b6a-9c06-9725f0824103&tileId=5d573747-f736-4f29-af18-3c7d2ebc3d06";
                keyValue.PeriodWiseConsumption = "https://app.powerbi.com/embed?dashboardId=40306835-aee6-4b6a-9c06-9725f0824103&tileId=f91192e4-4343-4cf5-ace4-cfbd4bd01b06";

                keyValue.Report = "https://app.powerbi.com/reportEmbed?reportId=e42884df-a83a-4dc5-96d0-cb6080c9858d";
                return keyValue;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetURL_P371602068 as: " + ex);
                return null;
            }
        }

        public MeterURLKey GetURL_P371602070()
        {
            MeterURLKey keyValue;
            try
            {
                log.Debug("GetURL_P371602070 called");
                keyValue = new MeterURLKey();
                keyValue.Weather = WeatherURL;
                keyValue.QuarterlyConsumption = "";
                keyValue.LastQuarterlyConsumption = "";
                keyValue.MonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=45ea78d2-9320-4cd9-b4e0-6feafbc4aab9&tileId=568fc243-fedd-49e8-9cb6-d0a56f013a6b";
                keyValue.LastMonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=45ea78d2-9320-4cd9-b4e0-6feafbc4aab9&tileId=42a1acda-4978-4c0e-bc11-c51f0e54e0ef";
                keyValue.WeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=45ea78d2-9320-4cd9-b4e0-6feafbc4aab9&tileId=2f6d523b-fc45-4d0c-9734-78590706ebb3";
                keyValue.LastWeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=45ea78d2-9320-4cd9-b4e0-6feafbc4aab9&tileId=f9c49387-9e5a-4fe6-a143-4da8d9dee810";
                keyValue.DailyConsumption = "https://app.powerbi.com/embed?dashboardId=45ea78d2-9320-4cd9-b4e0-6feafbc4aab9&tileId=afe27612-f12c-46ba-8914-e091ee644cd7";
                keyValue.YesterdayConsumption = "https://app.powerbi.com/embed?dashboardId=45ea78d2-9320-4cd9-b4e0-6feafbc4aab9&tileId=432aa9e8-3ad6-4637-964b-b88ed6a38266";

                keyValue.MonthlyKWh = "https://app.powerbi.com/embed?dashboardId=45ea78d2-9320-4cd9-b4e0-6feafbc4aab9&tileId=beb9f365-7c48-4886-b1da-9e5814f15bf7";
                keyValue.MonthlyCost = "https://app.powerbi.com/embed?dashboardId=45ea78d2-9320-4cd9-b4e0-6feafbc4aab9&tileId=603896d4-1468-43d2-a91c-c76fda71c675";
                keyValue.DayWiseConsumption = "https://app.powerbi.com/embed?dashboardId=45ea78d2-9320-4cd9-b4e0-6feafbc4aab9&tileId=22a4ec9e-2cc2-4e34-b724-c5549c7d6754";
                keyValue.PeriodWiseConsumption = "https://app.powerbi.com/embed?dashboardId=45ea78d2-9320-4cd9-b4e0-6feafbc4aab9&tileId=39f7b1b1-1cbc-4102-a67b-74b13bbc3b4d";

                keyValue.Report = "https://app.powerbi.com/reportEmbed?reportId=7e3554a5-cded-4893-b956-6a83a2fcef58";
                return keyValue;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetURL_P371602070 as: " + ex);
                return null;
            }
        }

        public MeterURLKey GetURL_P371602072()
        {
            MeterURLKey keyValue;
            try
            {
                log.Debug("GetURL_P371602072 called");
                keyValue = new MeterURLKey();
                keyValue.Weather = WeatherURL;
                keyValue.QuarterlyConsumption = "";
                keyValue.LastQuarterlyConsumption = "";
                keyValue.MonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=7b93337b-422d-4403-b703-1be55f4f793f&tileId=7784efa0-a6b9-43d4-ba26-241c0965dbd9";
                keyValue.LastMonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=7b93337b-422d-4403-b703-1be55f4f793f&tileId=b6c2083d-68d6-4f25-a970-0fb8bd122910";

                keyValue.WeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=7b93337b-422d-4403-b703-1be55f4f793f&tileId=98e5fc70-a071-41db-b1c9-bf38ea9ce105";
                keyValue.LastWeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=7b93337b-422d-4403-b703-1be55f4f793f&tileId=015071b5-7e7e-4ecd-9c8b-aec4be7aa39d";

                keyValue.DailyConsumption = "https://app.powerbi.com/embed?dashboardId=7b93337b-422d-4403-b703-1be55f4f793f&tileId=5d9488af-ae6e-420c-90fe-a426cefb39c5";
                keyValue.YesterdayConsumption = "https://app.powerbi.com/embed?dashboardId=7b93337b-422d-4403-b703-1be55f4f793f&tileId=aeb16edb-3b46-4197-86c5-b8b6dddc7873";

                keyValue.MonthlyKWh = "https://app.powerbi.com/embed?dashboardId=7b93337b-422d-4403-b703-1be55f4f793f&tileId=249cabc6-5056-4527-b2c3-ed81954781fb";
                keyValue.MonthlyCost = "https://app.powerbi.com/embed?dashboardId=7b93337b-422d-4403-b703-1be55f4f793f&tileId=195bd57a-46d4-4cb7-a9bb-80bffe6838d0";
                keyValue.DayWiseConsumption = "https://app.powerbi.com/embed?dashboardId=7b93337b-422d-4403-b703-1be55f4f793f&tileId=dbcb976e-fc70-4f73-96cb-be241c6d81cd";
                keyValue.PeriodWiseConsumption = "https://app.powerbi.com/embed?dashboardId=7b93337b-422d-4403-b703-1be55f4f793f&tileId=d1415c76-66e8-419d-82b2-05009445f6ec";

                keyValue.Report = "https://app.powerbi.com/reportEmbed?reportId=4840e4df-37dd-4b40-8adb-bac55d250059";
                return keyValue;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetURL_P371602072 as: " + ex);
                return null;
            }
        }

        public MeterURLKey GetURL_P371602073()
        {
            MeterURLKey keyValue;
            try
            {
                log.Debug("GetURL_P371602073 called");
                keyValue = new MeterURLKey();
                keyValue.Weather = WeatherURL;
                keyValue.QuarterlyConsumption = "";
                keyValue.LastQuarterlyConsumption = "";

                keyValue.MonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=72400f08-77c2-499a-8521-c4cc9e6af55f&tileId=68128d85-11f9-46a7-8177-a1c9b4dc8127";
                keyValue.LastMonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=72400f08-77c2-499a-8521-c4cc9e6af55f&tileId=9e481a1d-2d3a-4b0c-9d6b-0007449e0ab5";

                keyValue.WeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=72400f08-77c2-499a-8521-c4cc9e6af55f&tileId=d15fd53d-b925-4d38-96d9-1a34c39dda3b";
                keyValue.LastWeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=72400f08-77c2-499a-8521-c4cc9e6af55f&tileId=3ff0f7ac-64fc-4cda-9716-59fcdb142490";

                keyValue.DailyConsumption = "https://app.powerbi.com/embed?dashboardId=72400f08-77c2-499a-8521-c4cc9e6af55f&tileId=7876ab96-a431-420a-80de-38acf82d3235";
                keyValue.YesterdayConsumption = "https://app.powerbi.com/embed?dashboardId=72400f08-77c2-499a-8521-c4cc9e6af55f&tileId=70849e9c-cd0b-4ec3-9959-87e88f38e60c";

                keyValue.MonthlyKWh = "https://app.powerbi.com/embed?dashboardId=72400f08-77c2-499a-8521-c4cc9e6af55f&tileId=c70668ca-da0e-4c8d-baea-214b04f36972";
                keyValue.MonthlyCost = "https://app.powerbi.com/embed?dashboardId=72400f08-77c2-499a-8521-c4cc9e6af55f&tileId=dea80679-4836-435b-85cb-a8fd786a53dd";
                keyValue.DayWiseConsumption = "https://app.powerbi.com/embed?dashboardId=72400f08-77c2-499a-8521-c4cc9e6af55f&tileId=815a01bf-f6bc-48b5-9913-f52e21afcd07";
                keyValue.PeriodWiseConsumption = "https://app.powerbi.com/embed?dashboardId=72400f08-77c2-499a-8521-c4cc9e6af55f&tileId=457bb8c2-c074-40d1-bf5e-9d01a88bf89f";

                keyValue.Report = "https://app.powerbi.com/reportEmbed?reportId=e1d6b6cc-9e9a-4a05-a6c1-cb079c833a0f";
                return keyValue;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetURL_P371602073 as: " + ex);
                return null;
            }
        }

        public MeterURLKey GetURL_P371602075()
        {
            MeterURLKey keyValue;
            try
            {
                log.Debug("GetURL_P371602075 called");
                keyValue = new MeterURLKey();
                keyValue.Weather = WeatherURL;
                keyValue.QuarterlyConsumption = "";
                keyValue.LastQuarterlyConsumption = "";

                keyValue.MonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=336abc2a-4412-4347-a140-e0f47485b24f&tileId=f00cce37-1d2f-45d8-a6fa-cae609dabf53";
                keyValue.LastMonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=336abc2a-4412-4347-a140-e0f47485b24f&tileId=1fb619bc-7bdb-431b-9ffd-1c384761a482";

                keyValue.WeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=336abc2a-4412-4347-a140-e0f47485b24f&tileId=ec70d2b8-998a-4595-b645-b97bdbc8d66f";
                keyValue.LastWeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=336abc2a-4412-4347-a140-e0f47485b24f&tileId=568d97d9-19cd-40ea-8e52-bad204e1d43a";

                keyValue.DailyConsumption = "https://app.powerbi.com/embed?dashboardId=336abc2a-4412-4347-a140-e0f47485b24f&tileId=f42c8567-fb63-478a-a84e-48dac0b41b32";
                keyValue.YesterdayConsumption = "https://app.powerbi.com/embed?dashboardId=336abc2a-4412-4347-a140-e0f47485b24f&tileId=cd2f209d-aa14-426a-b84c-b05dd0c5b4c8";

                keyValue.MonthlyKWh = "https://app.powerbi.com/embed?dashboardId=336abc2a-4412-4347-a140-e0f47485b24f&tileId=84090c8d-ea5d-4913-b1d8-4f98307567e8";
                keyValue.MonthlyCost = "https://app.powerbi.com/embed?dashboardId=336abc2a-4412-4347-a140-e0f47485b24f&tileId=78491c8a-5fe0-40d1-98ef-d9262eb15941";
                keyValue.DayWiseConsumption = "https://app.powerbi.com/embed?dashboardId=336abc2a-4412-4347-a140-e0f47485b24f&tileId=fb4a3f9d-b554-489c-926a-083af7646cb8";
                keyValue.PeriodWiseConsumption = "https://app.powerbi.com/embed?dashboardId=336abc2a-4412-4347-a140-e0f47485b24f&tileId=79183a53-84c0-4dea-b36c-bdd78249ce0e";

                keyValue.Report = "https://app.powerbi.com/reportEmbed?reportId=a9159cbb-e7e5-4fb7-8054-f25755c95f47";
                return keyValue;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetURL_P371602075 as: " + ex);
                return null;
            }
        }

        public MeterURLKey GetURL_P371602077()
        {
            MeterURLKey keyValue;
            try
            {
                log.Debug("GetURL_P371602077 called");
                keyValue = new MeterURLKey();
                keyValue.Weather = WeatherURL;
                keyValue.QuarterlyConsumption = "";
                keyValue.LastQuarterlyConsumption = "";

                keyValue.MonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=c54b10f7-2551-4d62-9e4d-0b8772383abe&tileId=c9025706-0509-4b9f-9c25-c978ad81dd5b";
                keyValue.LastMonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=c54b10f7-2551-4d62-9e4d-0b8772383abe&tileId=bb9d40ff-219f-4dc5-b3d3-f765103d31a9";

                keyValue.WeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=c54b10f7-2551-4d62-9e4d-0b8772383abe&tileId=966ac473-c323-4b36-b483-b5ff8b01cd8e";
                keyValue.LastWeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=c54b10f7-2551-4d62-9e4d-0b8772383abe&tileId=28f7d675-5f43-4271-ad9b-9ba23726cb5b";

                keyValue.DailyConsumption = "https://app.powerbi.com/embed?dashboardId=c54b10f7-2551-4d62-9e4d-0b8772383abe&tileId=d67ae53e-a005-4dbd-beac-ecda2a5ca6ab";
                keyValue.YesterdayConsumption = "https://app.powerbi.com/embed?dashboardId=c54b10f7-2551-4d62-9e4d-0b8772383abe&tileId=7c39da34-b2b2-4c4f-9bb8-ce77c90b18c1";

                keyValue.MonthlyKWh = "https://app.powerbi.com/embed?dashboardId=c54b10f7-2551-4d62-9e4d-0b8772383abe&tileId=c7b6c94f-37ad-48d2-b1ed-35bb1545d591";
                keyValue.MonthlyCost = "https://app.powerbi.com/embed?dashboardId=c54b10f7-2551-4d62-9e4d-0b8772383abe&tileId=f49dbe63-0b21-4946-9ce1-20a6d8c8d8e7";
                keyValue.DayWiseConsumption = "https://app.powerbi.com/embed?dashboardId=c54b10f7-2551-4d62-9e4d-0b8772383abe&tileId=a73e3f67-9081-441b-bfb5-cf39fa6fa89e";
                keyValue.PeriodWiseConsumption = "https://app.powerbi.com/embed?dashboardId=c54b10f7-2551-4d62-9e4d-0b8772383abe&tileId=ec321112-b0d5-49e8-bb53-cc4395f838d2";

                keyValue.Report = "https://app.powerbi.com/reportEmbed?reportId=d11751dd-4889-4880-8257-a5fb4aa8e1e5";
                return keyValue;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetURL_P371602077 as: " + ex);
                return null;
            }
        }

        public MeterURLKey GetURL_P371602079()
        {
            MeterURLKey keyValue;
            try
            {
                log.Debug("GetURL_P371602079 called");
                keyValue = new MeterURLKey();
                keyValue.Weather = WeatherURL;
                keyValue.QuarterlyConsumption = "";
                keyValue.LastQuarterlyConsumption = "";

                keyValue.MonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=939ac96d-e4a0-4e66-853b-d5dbfc1c3279&tileId=56dea74e-86c6-481e-b84f-27a04e68ccaf";
                keyValue.LastMonthlyConsumption = "https://app.powerbi.com/embed?dashboardId=939ac96d-e4a0-4e66-853b-d5dbfc1c3279&tileId=09954fb8-41e6-4112-b09d-aa5f0d024cca";

                keyValue.WeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=939ac96d-e4a0-4e66-853b-d5dbfc1c3279&tileId=ac3e5c37-6a2b-4142-98cc-babe5e720695";
                keyValue.LastWeeklyConsumption = "https://app.powerbi.com/embed?dashboardId=939ac96d-e4a0-4e66-853b-d5dbfc1c3279&tileId=be405191-2799-4c8f-90bd-c82025c542da";

                keyValue.DailyConsumption = "https://app.powerbi.com/embed?dashboardId=939ac96d-e4a0-4e66-853b-d5dbfc1c3279&tileId=cb1cfa6b-b02c-42c1-aecf-3da0cac1e31c";
                keyValue.YesterdayConsumption = "https://app.powerbi.com/embed?dashboardId=939ac96d-e4a0-4e66-853b-d5dbfc1c3279&tileId=c9a90343-c4c6-4ffb-8ea9-63beaa3e2090";

                keyValue.MonthlyKWh = "https://app.powerbi.com/embed?dashboardId=939ac96d-e4a0-4e66-853b-d5dbfc1c3279&tileId=a70fc3dd-ea0f-48f2-81fd-95e12b5b35b7";
                keyValue.MonthlyCost = "https://app.powerbi.com/embed?dashboardId=939ac96d-e4a0-4e66-853b-d5dbfc1c3279&tileId=0450955e-ad14-43c8-be05-8053b2fb3e50";
                keyValue.DayWiseConsumption = "https://app.powerbi.com/embed?dashboardId=939ac96d-e4a0-4e66-853b-d5dbfc1c3279&tileId=069526aa-3118-440b-9e73-e37f44c7f56d";
                keyValue.PeriodWiseConsumption = "https://app.powerbi.com/embed?dashboardId=939ac96d-e4a0-4e66-853b-d5dbfc1c3279&tileId=151702fc-33ac-48a3-8665-a9ef6ae69fd3";

                keyValue.Report = "https://app.powerbi.com/reportEmbed?reportId=6653ae58-27ad-4a50-b4f1-0cab4e8eb255";
                return keyValue;
            }
            catch (Exception ex)
            {
                log.Error("Exception occurred in GetURL_P371602079 as: " + ex);
                return null;
            }
        }
    }
}