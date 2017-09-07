namespace RestService.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Mappings;
    using RestService.Models;

    public sealed class QuestionService : IQuestionService, IDisposable
    {
        private readonly PowerGridEntities dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionService"/> class.
        /// </summary>
        public QuestionService()
        {
            this.dbContext = new PowerGridEntities();
        }

        List<QuestionModel> IQuestionService.GetQuestionAnswers()
        {
            var questionList = this.dbContext.Questions;
            return new QuestionModelMapping().Map(questionList).ToList();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            if (this.dbContext != null)
            {
                this.dbContext.Dispose();
            }
        }
    }
}