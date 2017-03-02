using Newtonsoft.Json;
using RestService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace RestService.Utilities
{
    public class ServiceUtil
    {

        static string _sender = "itadmin@admindomain.onmicrosoft.com";
        static string _password = "Microsoft!@#$";
        
        public static ResponseModel SendMail(string recipient, string subject, string message)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                SmtpClient client = new SmtpClient("smtp.office365.com");

                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                System.Net.NetworkCredential credentials =
                    new System.Net.NetworkCredential(_sender, _password);
                client.EnableSsl = true;
                client.Credentials = credentials;


                var mail = new MailMessage(_sender.Trim(), recipient.Trim());
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = message;
                client.Send(mail);
                response.Status_Code = (int)Constants.StatusCode.Ok;
                response.Message = "Email Sent successfully";
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                response.Status_Code = (int)Constants.StatusCode.Error;
                response.Message = ex.Message;
                return response;
            }
        }

        public static int GetDayOfWeek(string Day)
        {
            switch (Day.ToLower())
            {
                case "mon":
                    return (int)Constants.DayofWeek.Mon;

                case "tue":
                    return (int)Constants.DayofWeek.Tue;

                case "wed":
                    return (int)Constants.DayofWeek.Wed;

                case "thu":
                    return (int)Constants.DayofWeek.Thu;

                case "fri":
                    return (int)Constants.DayofWeek.Fri;

                case "sat":
                    return (int)Constants.DayofWeek.Sat;

                case "sun":
                    return (int)Constants.DayofWeek.Sun;
            }
            return 1;
        }

        public static void SendNotification(string title, string body)
        {
            try
            {
                var applicationID = "AAAAGQBSH1c:APA91bEcYFZQMez7DyNTgphhxk1Sw4uKgss0xW7qBqiMX9QBHPNeIItIrw8VhvCJVWi8WUGUMPdRrx64P82lUtzmPUdvKFKYdr_UJHQl6lnWrXeK0J6-QHZaqkhsAKw1J3TwUievGRA2";
                //var applicationID = appSettings["ApplicationId"];

                var senderId = "107379564375";
                //var senderId = appSettings["SenderId"];

                string receiver = "/topics/Alerts";
                //string receiver = appSettings["Receiver"];

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                //WebRequest tRequest = WebRequest.Create(appSettings["FCMURL"]);
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                var data = new
                {
                    //data = new
                    //{
                    notification = new
                    {
                        body = body,
                        title = title,
                        click_action = "https://cloud.csupoc.com/csu/",
                        icon = "./csu/Assets/logo.png",
                        sound = "default"
                    },
                    to = receiver
                };

                var json = JsonConvert.SerializeObject(data);

                //var json = "{ \"notification\": {\"title\": \"Notification from csu\",\"text\": \"Notification from csu\",\"click_action\": \"http://localhost:65159/#/login \"},\"to\" : \"/topics/Alerts\"}";

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
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
                                String sResponseFromServer = tReader.ReadToEnd();
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
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime;
        }
    }
}