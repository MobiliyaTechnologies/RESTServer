//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RestService.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class SensorData
    {
        public int Sensor_Log_Id { get; set; }
        public int Sensor_Id { get; set; }
        public Nullable<int> Room_Id { get; set; }
        public Nullable<byte> Is_Light_ON { get; set; }
        public Nullable<double> Temperature { get; set; }
        public Nullable<double> Light_Intensity { get; set; }
        public Nullable<double> Humidity { get; set; }
        public Nullable<double> Battery_Remaining { get; set; }
        public Nullable<System.DateTime> Timestamp { get; set; }
        public Nullable<System.DateTime> Last_Updated { get; set; }
    }
}
