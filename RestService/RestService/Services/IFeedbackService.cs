namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Models;

    /// <summary>
    /// Provides student feedback operations.
    /// </summary>
    public interface IFeedbackService
    {
        /// <summary>
        /// Stores the student feedback for a classroom.
        /// </summary>
        /// <param name="feedbackModel">The feedbackModel to be stored.</param>
        /// <returns>
        /// The feedback stored confirmation.
        /// </returns>
        ResponseModel StoreFeedback( FeedbackModel feedbackModel);

        /// <summary>
        /// Gets all feedback.
        /// </summary>
        /// <returns>All stored feedbacks.</returns>
        List<FeedbackModel> GetAllFeedback();

        /// <summary>
        /// Deletes the feedback.
        /// </summary>
        /// <param name="feedbackId">The feedback identifier to be deleted.</param>
        /// <returns>The feedback deleted confirmation.</returns>
        ResponseModel DeleteFeedback(int feedbackId);

        /// <summary>
        /// Updates the feedback.
        /// </summary>
        /// <param name="feedbackModel">The feedbackModel to be updated.</param>
        /// <returns>
        /// The feedback updated confirmation
        /// </returns>
        ResponseModel UpdateFeedback(FeedbackModel feedbackModel);

        /// <summary>
        /// Gets the feedback for given classroom.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <returns>
        /// The class feedback.
        /// </returns>
        List<FeedbackCountModel> GetFeedbackCount(int classId);

        /// <summary>
        /// Resets or remove all feedback.
        /// </summary>
        /// <returns>The Feedback reset confirmation.</returns>
        ResponseModel ResetFeedback();
    }
}
