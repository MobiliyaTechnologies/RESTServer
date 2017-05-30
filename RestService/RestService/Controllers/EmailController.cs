namespace RestService.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;
    using RestService.Models;
    using RestService.Services;
    using RestService.Services.Impl;

    [RoutePrefix("api")]
    [AllowAnonymous]
    public class EmailController : ApiController
    {
        private readonly IEmailService emailService;
        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailController"/> class.
        /// </summary>
        public EmailController()
        {
            this.emailService = new EmailService();
            this.userService = new UserService();
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="emailModel">The email model.</param>
        /// <returns>The email send confirmation, or bad request error response if invalid parameters.</returns>
        [Route("SendMail")]
        [HttpPost]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage SendMail(EmailModel emailModel)
        {
            if (emailModel == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Email model can not be null.");
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
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, messages);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.userService != null)
            {
                (this.userService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
