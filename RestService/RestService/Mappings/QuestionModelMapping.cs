namespace RestService.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

    public class QuestionModelMapping
    {
        public IQueryable<QuestionModel> Map(IQueryable<Questions> source)
        {
            return from s in source
                   select new QuestionModel
                   {
                       QuestionId = s.QuestionID,
                       QuestionDesc = s.QuestionDec,
                       Answers = (from a in s.Answers
                                    select new AnswerModel()
                                    {
                                        AnswerDesc = a.AnswerDesc,
                                        AnswerId = a.AnswerID
                                    }).ToList()
                   };
        }

        public QuestionModel Map(Questions source)
        {
            return this.Map(new List<Questions> { source }.AsQueryable()).FirstOrDefault();
        }
    }
}