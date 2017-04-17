namespace RestService.Utilities
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using Newtonsoft.Json;
    using RestService.Enums;

    public class ServiceUtil
    {
        public static int GetDayOfWeek(string Day)
        {
            switch (Day.ToLower())
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

        public static void SendNotification(string title, string body)
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
                        body = body,
                        title = title,
                        click_action = ApiConfiguration.NotificationClickActionURL,
                        icon = "./csu/Assets/logo.png",
                        sound = "default"
                    },
                    to = ApiConfiguration.NotificationReceiver
                };

                var json = JsonConvert.SerializeObject(data);
                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", ApiConfiguration.ApplicationId));
                tRequest.Headers.Add(string.Format("Sender: id={0}", ApiConfiguration.NotificationSender));
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