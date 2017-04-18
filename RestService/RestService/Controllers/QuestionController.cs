namespace RestService.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using RestService.Services;
    using RestService.Services.Impl;

    public class QuestionController : ApiController
    {
        private IQuestionService questionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionController"/> class.
        /// </summary>
        public QuestionController()
        {
            this.questionService = new QuestionService();
        }

        [Route("api/getquestionanswers")]
        public HttpResponseMessage GetQuestionAnswers()
        {
            var data = this.questionService.GetQuestionAnswers();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                (this.questionService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
