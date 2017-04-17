namespace RestService.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

    public class AnswerModelMapping
    {
        public IQueryable<AnswerModel> Map(IQueryable<Answers> source)
        {
            return from s in source
                   select new AnswerModel()
                   {
                       AnswerDesc = s.AnswerDesc,
                       AnswerId = s.AnswerID
                   };
        }

        public AnswerModel Map(Answers source)
        {
            return source == null ? null : this.Map(new List<Answers> { source }.AsQueryable()).First();
        }
    }
}