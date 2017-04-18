namespace RestService.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class FeedbackModel
    {
        [Required]
        public int FeedbackId { get; set; }

        public int ClassId { get; set; }

        public int QuestionId { get; set; }

        public int AnswerID { get; set; }

        [MaxLength(500)]
        public string FeedbackDesc { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}