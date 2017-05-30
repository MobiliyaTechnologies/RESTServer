namespace RestService.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;
    using RestService.Models;
    using RestService.Services;
    using RestService.Services.Impl;

    [RoutePrefix("api")]
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

        /// <summary>
        /// Deletes the feedback for given identifier.
        /// </summary>
        /// <param name="feedbackId">The feedback identifier.</param>
        /// <returns>The feedback deleted confirmation, or bad request error response if invalid parameters.</returns>
        [Route("DeleteFeedback/{feedbackId}")]
        [HttpDelete]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage DeleteFeedback(int feedbackId)
        {
            if (feedbackId < 1)
            {
                this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Feedback id must be grater than 0.");
            }

            var data = this.feedbackService.DeleteFeedback(feedbackId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Updates the feedback.
        /// Feedback id is required to update feedback other fields are optional, passed only fields required to update.
        /// </summary>
        /// <param name="feedbackdetail">The feedback detail.</param>
        /// <returns>The feedback updated confirmation, or bad request error response if invalid parameters.</returns>
        [Route("UpdateFeedback")]
        [HttpPut]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage UpdateFeedback([FromBody] FeedbackModel feedbackdetail)
        {
            if (feedbackdetail != null && feedbackdetail.FeedbackId > 0)
            {
                var data = this.feedbackService.UpdateFeedback(feedbackdetail);
                return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            string messages = feedbackdetail == null ? "Invalid feedback model" : "Invalid feedback id.";
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, messages);
        }

        /// <summary>
        /// Stores the feedback.
        /// </summary>
        /// <param name="feedbackdetail">The feedback detail.</param>
        /// <returns>The feedback created confirmation, or bad request error response if invalid parameters.</returns>
        [Route("StoreFeedback")]
        [HttpPost]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage StoreFeedback([FromBody] FeedbackModel feedbackdetail)
        {
            if (feedbackdetail != null && this.ModelState.IsValid)
            {
                    var data = this.feedbackService.StoreFeedback(feedbackdetail);
                    return this.Request.CreateResponse(HttpStatusCode.OK, data);
            }

            // Create an error message for returning
            string messages = feedbackdetail == null ? "Invalid feedback model" : string.Join("; ", this.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, messages);
        }

        /// <summary>
        /// Gets all feedback.
        /// </summary>
        /// <returns>The feedback details.</returns>
        [Route("GetAllFeedback")]
        [ResponseType(typeof(List<FeedbackModel>))]
        public HttpResponseMessage GetAllFeedback()
        {
            var data = this.feedbackService.GetAllFeedback();
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Gets the feedback count details for given class identifier.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <returns>The feedback details, or bad request error response if invalid parameters.</returns>
        [Route("GetFeedbackCount/{classId}")]
        [ResponseType(typeof(List<FeedbackModel>))]
        public HttpResponseMessage GetFeedbackCount(int classId)
        {
            if (classId < 1)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Class id must be grater than 0.");
            }

            var data = this.feedbackService.GetFeedbackCount(classId);
            return this.Request.CreateResponse(HttpStatusCode.OK, data);
        }

        /// <summary>
        /// Resets the feedback.
        /// </summary>
        /// <returns>The feedback reset confirmation.</returns>
        [Route("ResetFeedback")]
        [HttpDelete]
        [ResponseType(typeof(ResponseModel))]
        public HttpResponseMessage ResetFeedback()
        {
            var data = this.feedbackService.ResetFeedback();
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
                (this.feedbackService as IDisposable).Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
