namespace EnergyManagementScheduler.WebJob.Services
{
    using System;
    using System.Data.SqlClient;
    using EnergyManagementScheduler.WebJob.Contracts;

    public class AlertService
    {
        public void AddNewAlert(SqlConnection sqlConnection, SqlTransaction sqlTransaction, Alert alert)
        {
            var query = "INSERT INTO [dbo].[Alerts]([Sensor_Log_Id], [Sensor_Id], [Alert_Type], [Description], [Timestamp],[Is_Acknowledged], [PiServername])" +
                    "  VALUES(@param1, @param2, @param3, @param4, @param5, @param6, @param7)";

            using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection, sqlTransaction))
            {
                sqlCommand.Parameters.AddWithValue("@param1", alert.SensorLogId);
                sqlCommand.Parameters.AddWithValue("@param2", alert.SensorId);
                sqlCommand.Parameters.AddWithValue("@param3", alert.AlertType);
                sqlCommand.Parameters.AddWithValue("@param4", alert.Description);
                sqlCommand.Parameters.AddWithValue("@param5", alert.TimeStamp);
                sqlCommand.Parameters.AddWithValue("@param6", (byte)alert.IsAcknowledged);
                sqlCommand.Parameters.AddWithValue("@param7", alert.PiServerName);

                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}
