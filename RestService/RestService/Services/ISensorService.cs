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
        /// <param name="sensorId">The sensor identifier.</param>
        /// <param name="classId">The class identifier.</param>
        /// <returns>
        /// The sensor mapped confirmation.
        /// </returns>
        ResponseModel MapSensor(int sensorId, int classId);

        /// <summary>
        /// Gets all sensors for class.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <returns>
        /// The class sensors.
        /// </returns>
        List<SensorModel> GetAllSensorsForClass(int classId);

        /// <summary>
        /// Resets or remove all sensors.
        /// </summary>
        /// <returns>The reset sensor confirmation. </returns>
        ResponseModel ResetSensors();
    }
}
