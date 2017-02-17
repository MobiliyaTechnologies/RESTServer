using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestService.Models
{
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

    public class QuestionModel
    {
        public int QuestionId { get; set; }

        public string QuestionDesc { get; set; }

        public List<AnswerModel> Answers { get; set; }

        public QuestionModel()
        {
            Answers = new List<AnswerModel>();
        }
    }

    public class AnswerModel
    {
        public int AnswerId { get; set; }

        public string AnswerDesc { get; set; }
    }

    public class FeedbackCountModel
    {
        public int ClassId { get; set; }

        public int AnswerId { get; set; }

        public int AnswerCount { get; set; }

        public double Threshold { get; set; }
    }
}