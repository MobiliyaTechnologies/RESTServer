namespace RestService.Services.Impl
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using RestService.Enums;
    using RestService.Models;
    using RestService.Utilities;

    public class EmailService : IEmailService
    {
        ResponseModel IEmailService.SendMail(EmailModel emailModel)
        {
            try
            {
                SmtpClient client = new SmtpClient(ApiConfiguration.EmailHost, ApiConfiguration.EmailHostPort);

                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(ApiConfiguration.EmailSender, ApiConfiguration.EmailHostPassword);

                var mail = new MailMessage(ApiConfiguration.EmailSender.Trim(), emailModel.Recipient.Trim(), emailModel.Subject, emailModel.Body);
                mail.IsBodyHtml = emailModel.IsBodyHtml;
                client.Send(mail);

                return new ResponseModel(StatusCode.Ok, "Email Sent successfully");
            }
            catch (Exception ex)
            {
                return new ResponseModel(StatusCode.Error, string.Format("Failed to send mail, error - {0}", ex.Message));
            }
        }
    }
}