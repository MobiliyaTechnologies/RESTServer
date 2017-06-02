namespace RestService.Utilities
{
    using System;
    using System.IO;
    using System.Net;
    using System.Security.Claims;
    using System.Text;
    using Newtonsoft.Json;
    using RestService.Enums;
    using RestService.Models;

    public class ServiceUtil
    {
        public static int GetDayOfWeek(string day)
        {
            switch (day.ToLower())
            {
                case "mon":
                    return (int)DayofWeek.Mon;

                case "tue":
                    return (int)DayofWeek.Tue;

                case "wed":
                    return (int)DayofWeek.Wed;

                case "thu":
                    return (int)DayofWeek.Thu;

                case "fri":
                    return (int)DayofWeek.Fri;

                case "sat":
                    return (int)DayofWeek.Sat;

                case "sun":
                    return (int)DayofWeek.Sun;

                default:
                    return 1;
            }
        }

        public static void SendNotification(NotificationModel notificationModel)
        {
            try
            {
                WebRequest tRequest = WebRequest.Create(ApiConfiguration.NotificationURL);
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    notification = new
                    {
                        body = notificationModel.NotificationMessage,
                        title = notificationModel.NotificationTitle,
                        click_action = notificationModel.NotificationClickAction,
                        icon = notificationModel.NotificationIcon,
                        sound = "default"
                    },
                    to = notificationModel.NotificationReceiver
                };

                var json = JsonConvert.SerializeObject(data);
                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", notificationModel.NotificationAuthorizationKey));
                tRequest.Headers.Add(string.Format("Sender: id={0}", notificationModel.NotificationSender));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                string sResponseFromServer = tReader.ReadToEnd();
                                string str = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime;
        }
    }
}