namespace RestService.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using RestService.Models;
    using RestService.Services;
    using RestService.Services.Impl;

    public class FeedbackController : ApiController
    {
        private IFeedbackService feedbackService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackController"/> class.
        /// </summary>
        public FeedbackController()
        {
            this.feedbackService = new FeedbackService();
        }

        [Route("api/deletefeedback")]
        [HttpPost]
        public HttpResponseMessage DeleteFeedback([FromBody] FeedbackModel feedbackdetail)
        {
            var data = this.feedbackService.DeleteFeedback(feedbackdetail.FeedbackId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("api/updatefeedback")]
        [HttpPost]
        public HttpResponseMessage UpdateFeedback([FromBody] FeedbackModel feedbackdetail)
        {
            if (feedbackdetail != null && this.ModelState.IsValid)
            {
                // TODO : get user id from claim.
                int userId = 0;
                var data = this.feedbackService.UpdateFeedback(userId, feedbackdetail);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = feedbackdetail == null ? "Invalid feedback model" : string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, messages);
        }

        [Route("api/storefeedback")]
        [HttpPost]
        public HttpResponseMessage StoreFeedback([FromBody] FeedbackModel feedbackdetail)
        {
            if (feedbackdetail != null && this.ModelState.IsValid)
            {
                if (feedbackdetail.AnswerID == 0 && string.IsNullOrWhiteSpace(feedbackdetail.FeedbackDesc))
                {
                    // Create an error message for returning
                    return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please choose any answer or provide description");
                }
                else
                {
                    // TODO : get user id from claim.
                    int userId = 0;
                    var data = this.feedbackService.StoreFeedback(userId, feedbackdetail);
                    return this.Request.CreateResponse(HttpStatusCode.OK, data);
                }
            }

            // Create an error message for returning
            string messages = feedbackdetail == null ? "Invalid feedback model" : string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, messages);
        }

        [Route("api/getallfeedback")]
        public HttpResponseMessage GetAllFeedback()
        {
            var data = this.feedbackService.GetAllFeedback();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("api/getfeedbackcount")]
        [HttpPost]
        public HttpResponseMessage GetFeedbackCount( [FromBody] FeedbackCountModel feedbackCountModel)
        {
            if (feedbackCountModel == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid feedback count model");
            }

            var data = this.feedbackService.GetFeedbackCount(feedbackCountModel);

            if (data != null && data.Count() > 0)
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("Feedback does not exists for given class id - {0}", feedbackCountModel.ClassId));
        }

        [Route("api/resetfeedback")]
        [HttpGet]
        public HttpResponseMessage ResetFeedback()
        {
            var data = this.feedbackService.ResetFeedback();
            return this.Request.CreateResponse(HttpStatusCode.OK, data.Message);
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                (this.feedbackService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
