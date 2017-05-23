namespace RestService.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Enums;
    using RestService.Models;

    public sealed class SensorService : ISensorService, IDisposable
    {
        private readonly PowerGridEntities dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SensorService"/> class.
        /// </summary>
        public SensorService()
        {
            this.dbContext = new PowerGridEntities();
        }

        List<SensorModel> ISensorService.GetAllSensors()
        {
            var sensorModels = (from sensor in this.dbContext.SensorMaster
                                let classRoomDetail = this.dbContext.ClassroomDetails.FirstOrDefault(c => c.Class_Id == sensor.Class_Id)
                                select new SensorModel
                                {
                                    Class_Id = classRoomDetail.Class_Id,
                                    Class_X = classRoomDetail.X,
                                    Class_Y = classRoomDetail.Y,
                                    Class_Name = classRoomDetail.Class_Name,
                                    X = sensor.X,
                                    Y = sensor.Y,
                                    Sensor_Id = sensor.Sensor_Id,
                                    Sensor_Name = sensor.Sensor_Name
                                }).ToList();

            this.SetSensorLiveData(sensorModels);

            return sensorModels;
        }

        List<SensorModel> ISensorService.GetAllMapSensors()
        {
            var sensorModels = (from sensor in this.dbContext.SensorMaster.Where(s => s.Class_Id.HasValue && s.Class_Id > 0)
                                let classRoomDetail = this.dbContext.ClassroomDetails.FirstOrDefault(c => c.Class_Id == sensor.Class_Id)
                                select new SensorModel
                                {
                                    Class_Id = classRoomDetail.Class_Id,
                                    Class_X = classRoomDetail.X,
                                    Class_Y = classRoomDetail.Y,
                                    Class_Name = classRoomDetail.Class_Name,
                                    X = sensor.X,
                                    Y = sensor.Y,
                                    Sensor_Id = sensor.Sensor_Id,
                                    Sensor_Name = sensor.Sensor_Name
                                }).ToList();

            this.SetSensorLiveData(sensorModels);

            return sensorModels;
        }

        List<SensorModel> ISensorService.GetAllUnMapSensors()
        {
            var sensorModels = (from sensor in this.dbContext.SensorMaster
                               where sensor.Class_Id == null || sensor.Class_Id < 1
                                select new SensorModel
                                {
                                    Sensor_Id = sensor.Sensor_Id,
                                    Sensor_Name = sensor.Sensor_Name,
                                    X = sensor.X,
                                    Y = sensor.Y
                                }).ToList();

            this.SetSensorLiveData(sensorModels);

            return sensorModels;
        }

        List<SensorModel> ISensorService.GetAllSensorsForClass(int classId)
        {
            var sensorModels = (from sensor in this.dbContext.SensorMaster.Where(s => s.Class_Id == classId)
                                let classRoomDetail = this.dbContext.ClassroomDetails.FirstOrDefault(c => c.Class_Id == sensor.Class_Id)
                                select new SensorModel
                                {
                                    Class_Id = classRoomDetail.Class_Id,
                                    Class_Name = classRoomDetail.Class_Name,
                                    Class_X = classRoomDetail.X,
                                    Class_Y = classRoomDetail.Y,
                                    Sensor_Id = sensor.Sensor_Id,
                                    Sensor_Name = sensor.Sensor_Name,
                                    X = sensor.X,
                                    Y = sensor.Y
                                }).ToList();

            this.SetSensorLiveData(sensorModels);

            return sensorModels;
        }

        ResponseModel ISensorService.MapSensor(int sensorId, int classId)
        {
            ResponseModel responseModel = new ResponseModel();
            var sensorData = this.dbContext.SensorMaster.FirstOrDefault(s => s.Sensor_Id == sensorId);

            if (sensorData == null)
            {
                responseModel.Message = "Sensor not found";
                responseModel.Status_Code = (int)StatusCode.Error;
            }
            else
            {
                sensorData.Class_Id = classId;
                this.dbContext.SaveChanges();

                responseModel.Message = "Sensor mapped successfully";
                responseModel.Status_Code = (int)StatusCode.Ok;
            }

            return responseModel;
        }

        ResponseModel ISensorService.ResetSensors()
        {
            var sensorMasters = this.dbContext.SensorMaster;
            ResponseModel responseModel = new ResponseModel();

            if (sensorMasters.Count() > 0)
            {
                this.dbContext.SensorMaster.RemoveRange(sensorMasters);
                this.dbContext.SaveChanges();

                responseModel.Status_Code = (int)StatusCode.Ok;
                responseModel.Message = "Sensors reset successful";
            }
            else
            {
                responseModel.Status_Code = (int)StatusCode.Error;
                responseModel.Message = "No rows found to reset";
            }

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

        private void SetSensorLiveData(List<SensorModel> sensorModels)
        {
            foreach (var sensorModel in sensorModels)
            {
                var sensorLiveData = this.dbContext.SensorLiveData.Where(s => s.Sensor_Id == sensorModel.Sensor_Id).OrderByDescending(s => s.Sensor_Log_Id).FirstOrDefault();

                if (sensorLiveData != null)
                {
                    sensorModel.Temperature = sensorLiveData.Temperature ?? default(double);
                    sensorModel.Humidity = sensorLiveData.Humidity ?? default(double);
                    sensorModel.Brightness = sensorLiveData.Brightness ?? default(double);
                }
            }
        }
    }
}