namespace RestService.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
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
        private readonly IApplicationConfigurationService applicationConfigurationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackService"/> class.
        /// </summary>
        public FeedbackService()
        {
            this.dbContext = new PowerGridEntities();
            this.context = new ContextInfoAccessorService();
            this.applicationConfigurationService = new ApplicationConfigurationService();
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
            var feedbacks = this.dbContext.Feedback.WhereInDateRange();

            return new FeedbackModelMappings().Map(feedbacks).ToList();
        }

        List<FeedbackCountModel> IFeedbackService.GetFeedbackCount(int roomId)
        {
            List<FeedbackCountModel> feedbackCount = new List<FeedbackCountModel>();
            var answerList = (from answer in this.dbContext.Answers select answer).ToList();

            var feedbackDetail = (from feedback in this.dbContext.Feedback where feedback.RoomID == roomId select feedback).ToList();
            var roomDetails = (from roomData in this.dbContext.RoomDetail where roomData.Room_Id == roomId select roomData).FirstOrDefault();

            answerList.All(answer =>
            {
                var answerCount = feedbackDetail.Where(feedback => feedback.AnswerID == answer.AnswerID).ToList().Count();
                feedbackCount.Add(new FeedbackCountModel { AnswerCount = answerCount, AnswerDesc = answer.AnswerDesc, AnswerId = answer.AnswerID, RoomId = roomId, RoomName = roomDetails.Room_Name });
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
            var feedbacks = this.dbContext.Feedback.Where(data => data.FeedbackID > 200 && data.RoomID == 2);
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
            feedback.RoomID = feedbackModel.RoomId;
            feedback.QuestionID = feedbackModel.QuestionId;
            feedback.AnswerID = feedbackModel.AnswerID;
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

            // get only todays feedback.
            var feedbackDetail = this.dbContext.Feedback.Where(f => f.RoomID == feedbackModel.RoomId && f.CreatedOn.HasValue && DbFunctions.TruncateTime(f.CreatedOn.Value) == DbFunctions.TruncateTime(DateTime.UtcNow));

            var classDetails = this.dbContext.RoomDetail.FirstOrDefault(c => c.Room_Id == feedbackModel.RoomId);

            foreach (var answer in answers)
            {
                var answerCount = feedbackDetail.Where(f => f.AnswerID == answer.AnswerID).Count();
                feedbackCount.Add(new FeedbackCountModel
                {
                    AnswerCount = answerCount,
                    AnswerDesc = answer.AnswerDesc,
                    AnswerId = answer.AnswerID,
                    RoomId = feedbackModel.RoomId,
                    RoomName = classDetails.Room_Name
                });
            }

            var threshold = feedbackCount.Sum(f => f.AnswerCount) * 0.6;
            feedbackCount.ForEach(f => f.Threshold = Math.Round(threshold, 2));

            return feedbackCount;
        }

        private void AlertFeedback(List<FeedbackCountModel> feedbackCount, FeedbackModel feedbackModel)
        {
            var exceptionData = feedbackCount.Where(f => f.AnswerCount > f.Threshold && f.AnswerId == feedbackModel.AnswerID);

            var applicationConfigurationEntry = this.applicationConfigurationService.GetApplicationConfiguration(ApiConstant.FirebaseApplicationConfiguration);

            var notificationAuthEntry = applicationConfigurationEntry.ApplicationConfigurationEntries.FirstOrDefault(a => a.ConfigurationKey.Equals("ApiKey", StringComparison.InvariantCultureIgnoreCase));

            var notificationSenderEntry = applicationConfigurationEntry.ApplicationConfigurationEntries.FirstOrDefault(a => a.ConfigurationKey.Equals("NotificationSender", StringComparison.InvariantCultureIgnoreCase));

            var notificationReceiverEntry = applicationConfigurationEntry.ApplicationConfigurationEntries.FirstOrDefault(a => a.ConfigurationKey.Equals("NotificationReceiver", StringComparison.InvariantCultureIgnoreCase));

            var notificationModel = new NotificationModel
            {
                NotificationAuthorizationKey = notificationAuthEntry != null ? notificationAuthEntry.ConfigurationValue : null,
                NotificationSender = notificationSenderEntry != null ? notificationSenderEntry.ConfigurationValue : null,
                NotificationReceiver = notificationReceiverEntry != null ? notificationReceiverEntry.ConfigurationValue : null,
                NotificationTitle = "Temperature Alert"
            };

            foreach (var exception in exceptionData)
            {
                if (exception.AnswerDesc.Contains("Hot"))
                {
                    notificationModel.NotificationMessage = "Temperature in room " + exception.RoomName + " seems to be warmer than average, Take appropriate measures.";
                }
                else if (exception.AnswerDesc.Contains("Cold"))
                {
                    notificationModel.NotificationMessage = "Temperature in room " + exception.RoomName + " seems to be colder than average, Take appropriate measures.";
                }
                else
                {
                    notificationModel.NotificationMessage = "Temperature in room " + exception.RoomName + " seems to be normal, No action required.";
                }

                if (!string.IsNullOrWhiteSpace(notificationModel.NotificationAuthorizationKey) && !string.IsNullOrWhiteSpace(notificationModel.NotificationSender) && !string.IsNullOrWhiteSpace(notificationModel.NotificationReceiver))
                {
                    ServiceUtil.SendNotification(notificationModel);
                }

                var alert = new Alerts
                {
                    Sensor_Id = 0,
                    Sensor_Log_Id = 0,
                    Alert_Type = notificationModel.NotificationTitle,
                    Description = notificationModel.NotificationMessage,
                    Is_Acknowledged = 0,
                    Timestamp = DateTime.UtcNow
                };

                this.dbContext.Alerts.Add(alert);
                this.dbContext.SaveChanges();
            }
        }
    }
}