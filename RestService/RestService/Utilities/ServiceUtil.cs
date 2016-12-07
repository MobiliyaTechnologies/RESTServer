using RestService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace RestService.Utilities
{
    public class ServiceUtil
    {
        public static ResponseModel SendEmail(string emailId)
        {
            ResponseModel response = new ResponseModel();
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add(emailId);
            mail.From = new MailAddress("", "Password Reset Request", System.Text.Encoding.UTF8);
            mail.Subject = "Password Reset Request";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = "Please click on the link below to reset you password\n\nhttp://reset.csupoc.com/";
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("", "");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            try
            {
                client.Send(mail);
                response.Status_Code = Convert.ToInt16(Constants.StatusCode.Ok);
                response.Message = "Email sent successfully";
            }
            catch (Exception ex)
            {
                response.Status_Code = Convert.ToInt16(Constants.StatusCode.Error);
                response.Message = ex.Message;
            }
            return response;
        }
    }
}