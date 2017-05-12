namespace RestService.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using RestService.Models;
    using RestService.Services;
    using RestService.Services.Impl;

    [RoutePrefix("api")]
    [AllowAnonymous]
    public class EmailController : ApiController
    {
        private readonly IEmailService emailService;
        private readonly IUserService userService;

        public EmailController()
        {
            this.emailService = new EmailService();
            this.userService = new UserService();
        }

        [Route("SendMail")]
        [HttpPost]
        public HttpResponseMessage SendMail(EmailModel emailModel)
        {
            if (emailModel == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Email model can not be null.");
            }

            if (string.IsNullOrWhiteSpace(emailModel.Subject) || string.IsNullOrWhiteSpace(emailModel.Body))
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid email subject or body.");
            }

            if (this.ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(emailModel.Recipient))
                {
                    var superAdmin = this.userService.GetAllUser().Where(u => u.RoleType == Enums.UserRole.SuperAdmin).FirstOrDefault();
                    if (superAdmin == null || string.IsNullOrWhiteSpace(superAdmin.Email))
                    {
                        return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Email recipient not found.");
                    }

                    emailModel.Recipient = superAdmin.Email;
                }

                var response = this.emailService.SendMail(emailModel);

                return this.Request.CreateResponse(HttpStatusCode.OK, response);
            }

            string messages = string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse((HttpStatusCode)613, messages);
        }
    }
}
