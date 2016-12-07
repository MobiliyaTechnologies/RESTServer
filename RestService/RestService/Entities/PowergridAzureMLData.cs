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
    
    public partial class PowergridAzureMLData
    {
        public int Id { get; set; }
        public Nullable<double> AMPS_L1 { get; set; }
        public Nullable<double> AMPS_L2 { get; set; }
        public Nullable<double> AMPS_L3 { get; set; }
        public Nullable<double> AMPS_SYSTEM_AVG { get; set; }
        public string Building { get; set; }
        public Nullable<int> ClassOccupanyRemaining { get; set; }
        public Nullable<int> ClassOccupiedValue { get; set; }
        public Nullable<int> TotalClassCapacity { get; set; }
        public Nullable<double> Daily_electric_cost { get; set; }
        public Nullable<double> Daily_KWH_System { get; set; }
        public Nullable<int> isClassOccupied { get; set; }
        public Nullable<double> KW_L1 { get; set; }
        public Nullable<double> KW_L2 { get; set; }
        public Nullable<double> KW_L3 { get; set; }
        public Nullable<double> Monthly_electric_cost { get; set; }
        public Nullable<double> Monthly_KWH_System { get; set; }
        public string PowerScout { get; set; }
        public Nullable<double> Rated_Amperage { get; set; }
        public Nullable<double> Pressure { get; set; }
        public Nullable<double> Relative_humidity { get; set; }
        public Nullable<double> Rolling_hourly_kwh_system { get; set; }
        public string Serial_number { get; set; }
        public Nullable<double> Temperature { get; set; }
        public Nullable<System.DateTime> Timestamp { get; set; }
        public string Type { get; set; }
        public Nullable<double> Visibility { get; set; }
        public Nullable<double> Volts_L1_to_neutral { get; set; }
        public Nullable<double> Volts_L2_to_neutral { get; set; }
        public Nullable<double> Volts_L3_to_neutral { get; set; }
        public Nullable<double> kW_System { get; set; }
        public Nullable<double> PredictedKwH { get; set; }
    }
}
