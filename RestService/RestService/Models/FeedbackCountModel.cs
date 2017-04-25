namespace RestService.Models
{
    public class FeedbackCountModel : AnswerModel
    {
        public int ClassId { get; set; }

        public string ClassName { get; set; }

        public int AnswerCount { get; set; }

        public double Threshold { get; set; }
    }
}