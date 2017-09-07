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
        /// Gets all mapped sensors.
        /// </summary>
        /// <returns>All mapped sensors.</returns>
        List<SensorModel> GetAllMapSensors();

        /// <summary>
        /// Gets all unmapped sensors.
        /// </summary>
        /// <returns>All unmapped sensors.</returns>
        List<SensorModel> GetAllUnMapSensors();

        /// <summary>
        /// Maps the sensor to room.
        /// </summary>
        /// <param name="sensorId">The sensor identifier.</param>
        /// <param name="roomId">The room identifier.</param>
        /// <returns>
        /// The sensor mapped confirmation.
        /// </returns>
        ResponseModel MapSensor(int sensorId, int roomId);

        /// <summary>
        /// Gets all sensors for room.
        /// </summary>
        /// <param name="roomId">The room identifier.</param>
        /// <returns>
        /// The room sensors.
        /// </returns>
        List<SensorModel> GetAllSensorsForRoom(int roomId);

        /// <summary>
        /// Resets or remove all sensors.
        /// </summary>
        /// <returns>The reset sensor confirmation. </returns>
        ResponseModel ResetSensors();
    }
}
