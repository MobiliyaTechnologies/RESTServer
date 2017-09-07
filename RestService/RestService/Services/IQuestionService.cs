namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Models;

    /// <summary>
    /// Provides room questions operation.
    /// </summary>
    public interface IQuestionService
    {
        /// <summary>
        /// Gets the question answers for all room.
        /// </summary>
        /// <returns>The room question answer details.</returns>
        List<QuestionModel> GetQuestionAnswers();
    }
}
