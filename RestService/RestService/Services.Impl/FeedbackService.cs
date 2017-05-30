﻿namespace RestService.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Enums;
    using RestService.Mappings;
    using RestService.Models;
    using RestService.Utilities;

    public sealed class FeedbackService : IFeedbackService, IDisposable
    {
        private readonly PowerGridEntities dbContext;
        private readonly IContextInfoAccessorService context;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackService"/> class.
        /// </summary>
        public FeedbackService()
        {
            this.dbContext = new PowerGridEntities();
            this.context = new ContextInfoAccessorService();
        }

        ResponseModel IFeedbackService.DeleteFeedback(int feedbackId)
        {
            var data = this.dbContext.Feedback.FirstOrDefault(f => f.FeedbackID == feedbackId);
            if (data == null)
            {
                return new ResponseModel { Message = "Invalid Feedback", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                this.dbContext.Feedback.Remove(data);
                this.dbContext.SaveChanges();
                return new ResponseModel { Message = "Feedback deleted", Status_Code = (int)StatusCode.Ok };
            }
        }

        ResponseModel IFeedbackService.UpdateFeedback(FeedbackModel feedbackDetail)
        {
            var data = this.dbContext.Feedback.FirstOrDefault(f => f.FeedbackID == feedbackDetail.FeedbackId);

            if (data == null)
            {
                return new ResponseModel { Message = "Invalid Feedback", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                if (feedbackDetail.AnswerID > 0)
                {
                    data.AnswerID = feedbackDetail.AnswerID;
                }

                if (!string.IsNullOrWhiteSpace(feedbackDetail.FeedbackDesc))
                {
                    data.FeedbackDesc = feedbackDetail.FeedbackDesc;
                }

                data.ModifiedBy = this.context.Current.UserId;
                data.ModiifiedOn = DateTime.UtcNow;
                this.dbContext.SaveChanges();

                return new ResponseModel { Message = "Feedback Updated", Status_Code = (int)StatusCode.Ok };
            }
        }

        List<FeedbackModel> IFeedbackService.GetAllFeedback()
        {
            var feedbacks = this.dbContext.Feedback;

            return new FeedbackModelMappings().Map(feedbacks).ToList();
        }

        List<FeedbackCountModel> IFeedbackService.GetFeedbackCount(int classId)
        {
            List<FeedbackCountModel> feedbackCount = new List<FeedbackCountModel>();
            var answerList = (from answer in this.dbContext.Answers select answer).ToList();

            var feedbackDetail = (from feedback in this.dbContext.Feedback where feedback.ClassID == classId select feedback).ToList();
            var classDetails = (from classData in this.dbContext.ClassroomDetails where classData.Class_Id == classId select classData).FirstOrDefault();

            answerList.All(answer =>
            {
                var answerCount = feedbackDetail.Where(feedback => feedback.AnswerID == answer.AnswerID).ToList().Count();
                feedbackCount.Add(new FeedbackCountModel { AnswerCount = answerCount, AnswerDesc = answer.AnswerDesc, AnswerId = answer.AnswerID, ClassId = classId, ClassName = classDetails.Class_Name });
                return true;
            });
            var threshold = feedbackCount.Sum(feedback => feedback.AnswerCount) * 0.6;
            feedbackCount.All(feedback =>
            {
                feedback.Threshold = Math.Round(threshold, 2);
                return true;
            });
            return feedbackCount;
        }

        ResponseModel IFeedbackService.ResetFeedback()
        {
            var feedbacks = this.dbContext.Feedback.Where(data => data.FeedbackID > 200 && data.ClassID == 2);
            if (feedbacks.Count() > 0)
            {
                this.dbContext.Feedback.RemoveRange(feedbacks);
                this.dbContext.SaveChanges();
                return new ResponseModel { Status_Code = (int)StatusCode.Ok, Message = "Feedback reset successful" };
            }
            else
            {
                return new ResponseModel { Status_Code = (int)StatusCode.Error, Message = "No rows found to reset" };
            }
        }

        ResponseModel IFeedbackService.StoreFeedback(FeedbackModel feedbackModel)
        {
            var responseModel = this.StoreFeedback(feedbackModel);
            var feedbackCount = this.GetFeedbackCount(feedbackModel);
            this.AlertFeedback(feedbackCount, feedbackModel);

            return responseModel;
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

        private ResponseModel StoreFeedback(FeedbackModel feedbackModel)
        {
            var feedback = new Feedback();
            feedback.ClassID = feedbackModel.ClassId;
            feedback.QuestionID = feedbackModel.QuestionId;
            feedback.AnswerID = feedback.AnswerID;
            feedback.FeedbackDesc = feedbackModel.FeedbackDesc == null ? string.Empty : feedbackModel.FeedbackDesc;
            feedback.CreatedBy = this.context.Current.UserId;
            feedback.ModifiedBy = this.context.Current.UserId;
            feedback.CreatedOn = DateTime.UtcNow;
            feedback.ModiifiedOn = DateTime.UtcNow;

            this.dbContext.Feedback.Add(feedback);
            this.dbContext.SaveChanges();

            // Add reward points
            var user = this.dbContext.User.FirstOrDefault(u => u.Id == this.context.Current.UserId);
            user.RewardPoints += 10;
            this.dbContext.SaveChanges();
            return new ResponseModel { Message = "Feedback successfully recorded", Status_Code = (int)StatusCode.Ok };
        }

        private List<FeedbackCountModel> GetFeedbackCount(FeedbackModel feedbackModel)
        {
            var feedbackCount = new List<FeedbackCountModel>();

            var answers = this.dbContext.Answers;
            var feedbackDetail = this.dbContext.Feedback.Where(f => f.ClassID == feedbackModel.ClassId);
            var classDetails = this.dbContext.ClassroomDetails.FirstOrDefault(c => c.Class_Id == feedbackModel.ClassId);

            foreach (var answer in answers)
            {
                var answerCount = feedbackDetail.Where(f => f.AnswerID == answer.AnswerID).Count();
                feedbackCount.Add(new FeedbackCountModel
                {
                    AnswerCount = answerCount,
                    AnswerDesc = answer.AnswerDesc,
                    AnswerId = answer.AnswerID,
                    ClassId = feedbackModel.ClassId,
                    ClassName = classDetails.Class_Name
                });
            }

            var threshold = feedbackCount.Sum(f => f.AnswerCount) * 0.6;
            feedbackCount.ForEach(f => f.Threshold = Math.Round(threshold, 2));

            return feedbackCount;
        }

        private void AlertFeedback(List<FeedbackCountModel> feedbackCount, FeedbackModel feedbackModel)
        {
            var exceptionData = feedbackCount.Where(f => f.AnswerCount > f.Threshold && f.AnswerId == feedbackModel.AnswerID);
            foreach (var exception in exceptionData)
            {
                var title = "Temperature Alert";
                var message = "Students are feeling " + exception.AnswerDesc + " in the class " + exception.ClassName + ". Take appropriate measures.";
                ServiceUtil.SendNotification(title, message);
                var alert = new Alerts
                {
                    Sensor_Id = 0,
                    Sensor_Log_Id = 0,
                    Alert_Type = title,
                    Description = message,
                    Is_Acknowledged = 0,
                    Timestamp = DateTime.UtcNow
                };

                this.dbContext.Alerts.Add(alert);
                this.dbContext.SaveChanges();
            }
        }
    }
}