namespace RestService.Services
{
    using RestService.Models;

    /// <summary>
    /// Provides operation to send mail.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="emailModel">The email model.</param>
        /// <returns>
        /// Mail send confirmation.
        /// </returns>
        ResponseModel SendMail(EmailModel emailModel);
    }
}
