namespace RestService.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class FeedbackModel
    {
        public int FeedbackId { get; set; }

        public int ClassId { get; set; }

        public int QuestionId { get; set; }

        [Range(1, int.MaxValue)]
        public int AnswerID { get; set; }

        [Required]
        [MaxLength(500)]
        public string FeedbackDesc { get; set; }
    }
}