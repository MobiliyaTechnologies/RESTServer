namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Models;

    /// <summary>
    /// Provides classroom questions operation.
    /// </summary>
    public interface IQuestionService
    {
        /// <summary>
        /// Gets the question answers for all classroom.
        /// </summary>
        /// <returns>The clasroom question answer details.</returns>
        List<QuestionModel> GetQuestionAnswers();
    }
}
