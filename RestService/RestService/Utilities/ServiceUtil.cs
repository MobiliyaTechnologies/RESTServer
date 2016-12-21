﻿using RestService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
    }
}