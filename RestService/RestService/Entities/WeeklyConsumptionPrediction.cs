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
    
    public partial class WeeklyConsumptionPrediction
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> Start_Time { get; set; }
        public Nullable<System.DateTime> End_Time { get; set; }
        public string PowerScout { get; set; }
        public string Breaker_details { get; set; }
        public Nullable<double> Weekly_Predicted_KWH_System { get; set; }
        public string Building { get; set; }
        public string PiServerName { get; set; }
    }
}
