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
    
    public partial class DimDate
    {
        public int DateKey { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string ReportingDate { get; set; }
        public string DayOfMonth { get; set; }
        public string DayName { get; set; }
        public string DayOfWeekInMonth { get; set; }
        public string DayOfWeekInYear { get; set; }
        public string DayOfQuarter { get; set; }
        public string DayOfYear { get; set; }
        public string WeekOfMonth { get; set; }
        public string WeekOfQuarter { get; set; }
        public string WeekOfYear { get; set; }
        public string Month { get; set; }
        public string MonthName { get; set; }
        public string MonthOfQuarter { get; set; }
        public string Quarter { get; set; }
        public string QuarterName { get; set; }
        public string Year { get; set; }
        public string YearName { get; set; }
        public string MonthYear { get; set; }
        public string MMYYYY { get; set; }
        public Nullable<System.DateTime> FirstDayOfMonth { get; set; }
        public Nullable<System.DateTime> LastDayOfMonth { get; set; }
        public Nullable<System.DateTime> FirstDayOfQuarter { get; set; }
        public Nullable<System.DateTime> LastDayOfQuarter { get; set; }
        public Nullable<System.DateTime> FirstDayOfYear { get; set; }
        public Nullable<System.DateTime> LastDayOfYear { get; set; }
        public string ReportingWeekOFMonth { get; set; }
    }
}