namespace RestService.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Enums;
    using RestService.Models;

    public sealed class AlertService : IAlertService, IDisposable
    {
        private readonly PowerGridEntities dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertService"/> class.
        /// </summary>
        public AlertService()
        {
            this.dbContext = new PowerGridEntities();
        }

        ResponseModel IAlertService.AcknowledgeAlert(AlertModel alertDetail)
        {
            var alert = this.dbContext.Alerts.FirstOrDefault(a => a.Id == alertDetail.Alert_Id);

            if (alert == null)
            {
                return new ResponseModel { Message = "Invalid Alert", Status_Code = (int)StatusCode.Error };
            }
            else
            {
                alert.Acknowledged_By = alertDetail.Acknowledged_By;
                alert.Is_Acknowledged = 1;
                alert.Acknowledged_Timestamp = DateTime.UtcNow;
                this.dbContext.SaveChanges();
                return new ResponseModel { Message = "Acknowledgment successful", Status_Code = (int)StatusCode.Ok };
            }
        }

        AlertDetailsModel IAlertService.GetAlertDetails(int sensorLogId)
        {
            var alertDetailsModel = (from data in this.dbContext.SensorLiveData
                                     join sensorData in this.dbContext.SensorMaster on data.Sensor_Id equals sensorData.Sensor_Id
                                     join classData in this.dbContext.ClassroomDetails on sensorData.Class_Id equals classData.Class_Id
                                     where data.Sensor_Log_Id == sensorLogId
                                     select new AlertDetailsModel
                                     {
                                         Sensor_Id = data.Sensor_Id ?? default(int),
                                         Class_Id = classData.Class_Id,
                                         Class_Name = classData.Class_Name,
                                         Class_Desc = classData.Class_Desc,
                                         Humidity = data.Humidity.HasValue ? Math.Round(data.Humidity.Value, 2) : default(double),
                                         Light_Intensity = data.Brightness.HasValue ? Math.Round(data.Brightness.Value, 2) : default(double),
                                         Temperature = data.Temperature.HasValue ? Math.Round(data.Temperature.Value, 2) : default(double),
                                         Timestamp = data.Timestamp ?? default(DateTime)
                                     }).FirstOrDefault() ?? new AlertDetailsModel();

            return alertDetailsModel;
        }

        List<AlertModel> IAlertService.GetAllAlerts()
        {
            var alertList = (from alerts in this.dbContext.Alerts
                             where alerts.Alert_Type != "Recommendation"
                             join sensorData in this.dbContext.SensorMaster on alerts.Sensor_Id equals sensorData.Sensor_Id into temp1
                             from subsensor in temp1.DefaultIfEmpty()
                             join classData in this.dbContext.ClassroomDetails on subsensor.Class_Id equals classData.Class_Id into temp
                             from subclass in temp.DefaultIfEmpty()
                             orderby alerts.Timestamp descending // left outer join
                             select new AlertModel
                             {
                                 Alert_Id = alerts.Id,
                                 Acknowledged_By = alerts.Acknowledged_By == null ? string.Empty : alerts.Acknowledged_By,
                                 Acknowledged_Timestamp = alerts.Acknowledged_Timestamp ?? default(DateTime),
                                 Alert_Desc = alerts.Description,
                                 Alert_Type = alerts.Alert_Type,
                                 Is_Acknowledged = alerts.Is_Acknowledged == 0 ? false : true,
                                 Sensor_Id = alerts.Sensor_Id,
                                 Sensor_Log_Id = alerts.Sensor_Log_Id,
                                 Timestamp = alerts.Timestamp ?? default(DateTime),
                                 Class_Id = subclass.Class_Id,
                                 Class_Name = subclass.Class_Name == null ? string.Empty : subclass.Class_Name
                             }).ToList();

            return alertList;
        }

        List<AlertModel> IAlertService.GetRecommendations()
        {
            var alertList = (from alerts in this.dbContext.Alerts
                             where alerts.Alert_Type == "Recommendation"
                             join sensorData in this.dbContext.SensorMaster on alerts.Sensor_Id equals sensorData.Sensor_Id into temp1
                             from subsensor in temp1.DefaultIfEmpty()
                             join classData in this.dbContext.ClassroomDetails on subsensor.Class_Id equals classData.Class_Id into temp
                             from subclass in temp.DefaultIfEmpty()
                             orderby alerts.Timestamp descending // left outer join
                             select new AlertModel
                             {
                                 Alert_Id = alerts.Id,
                                 Acknowledged_By = alerts.Acknowledged_By == null ? string.Empty : alerts.Acknowledged_By,
                                 Acknowledged_Timestamp = alerts.Acknowledged_Timestamp ?? default(DateTime),
                                 Alert_Desc = alerts.Description,
                                 Alert_Type = alerts.Alert_Type,
                                 Is_Acknowledged = alerts.Is_Acknowledged == 0 ? false : true,
                                 Sensor_Id = alerts.Sensor_Id,
                                 Sensor_Log_Id = alerts.Sensor_Log_Id,
                                 Timestamp = (DateTime)alerts.Timestamp,
                                 Class_Id = subclass.Class_Id,
                                 Class_Name = subclass.Class_Name == null ? string.Empty : subclass.Class_Name
                             }).ToList();

            return alertList;
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
    }
}