namespace RestService.Models
{
    using System.Collections.Generic;

    public class QuestionModel
    {
        public QuestionModel()
        {
            this.Answers = new List<AnswerModel>();
        }

        public int QuestionId { get; set; }

        public string QuestionDesc { get; set; }

        public List<AnswerModel> Answers { get; set; }
    }
}