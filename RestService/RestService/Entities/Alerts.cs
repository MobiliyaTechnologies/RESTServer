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
    
    public partial class Alerts
    {
        public int Id { get; set; }
        public int Sensor_Log_Id { get; set; }
        public int Sensor_Id { get; set; }
        public string Alert_Type { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> Timestamp { get; set; }
        public Nullable<byte> Is_Acknowledged { get; set; }
        public string Acknowledged_By { get; set; }
        public Nullable<System.DateTime> Acknowledged_Timestamp { get; set; }
    }
}