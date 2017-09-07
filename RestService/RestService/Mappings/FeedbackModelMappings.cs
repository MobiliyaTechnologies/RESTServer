namespace RestService.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

    public class FeedbackModelMappings
    {
        public IQueryable<FeedbackModel> Map(IQueryable<Feedback> source)
        {
            return from s in source
                   select new FeedbackModel
                   {
                       AnswerID = s.AnswerID ?? default(int),
                       RoomId = s.RoomID ?? default(int),
                       FeedbackId = s.FeedbackID,
                       FeedbackDesc = s.FeedbackDesc
                   };
        }

        public FeedbackModel Map(Feedback source)
        {
            return source == null ? null : this.Map(new List<Feedback> { source }.AsQueryable()).First();
        }
    }
}