namespace RestService.Models
{
    public class FeedbackCountModel : AnswerModel
    {
        public int RoomId { get; set; }

        public string RoomName { get; set; }

        public int AnswerCount { get; set; }

        public double Threshold { get; set; }
    }
}