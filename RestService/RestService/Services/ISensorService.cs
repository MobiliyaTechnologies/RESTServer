namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Models;

    /// <summary>
    /// Provides sensors operations.
    /// </summary>
    public interface ISensorService
    {
        /// <summary>
        /// Gets all sensors.
        /// </summary>
        /// <returns>All sensors list.</returns>
        List<SensorModel> GetAllSensors();

        /// <summary>
        /// Maps the sensor to classroom.
        /// </summary>
        /// <param name="sensorDetail">The sensor detail to be mapped.</param>
        /// <returns>The sensor mapped confirmation.</returns>
        ResponseModel MapSensor(SensorModel sensorDetail);

        /// <summary>
        /// Gets all sensors for class.
        /// </summary>
        /// <param name="sensorData">The sensor data.</param>
        /// <returns>The class sensors.</returns>
        List<SensorModel> GetAllSensorsForClass(SensorModel sensorData);

        /// <summary>
        /// Resets or remove all sensors.
        /// </summary>
        /// <returns>The reset sensor confirmation. </returns>
        ResponseModel ResetSensors();
    }
}
