namespace RestService.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class AlertModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Alert_Id { get; set; }

        public int Sensor_Log_Id { get; set; }

        public int Sensor_Id { get; set; }

        public string Alert_Type { get; set; }

        public string Alert_Desc { get; set; }

        public int? Room_Id { get; set; }

        public string Class_Name { get; set; }

        public string Class_Desc { get; set; }

        public DateTime Timestamp { get; set; }

        public bool Is_Acknowledged { get; set; }

        [Required]
        public string Acknowledged_By { get; set; }

        public DateTime Acknowledged_Timestamp { get; set; }
    }
}