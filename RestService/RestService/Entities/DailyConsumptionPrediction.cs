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
    
    public partial class DailyConsumptionPrediction
    {
        public int Id { get; set; }
        public string PowerScout { get; set; }
        public string Breaker_details { get; set; }
        public Nullable<System.DateTime> Timestamp { get; set; }
        public Nullable<double> Daily_KWH_System { get; set; }
        public Nullable<double> Daily_Predicted_KWH_System { get; set; }
        public Nullable<double> TomorrowPredictedKWH { get; set; }
    }
}
