﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PowerGridEntities : DbContext
    {
        public PowerGridEntities()
            : base("name=PowerGridEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Alert> Alert { get; set; }
        public virtual DbSet<DailyConsumptionDetails> DailyConsumptionDetails { get; set; }
        public virtual DbSet<DailyConsumptionPrediction> DailyConsumptionPrediction { get; set; }
        public virtual DbSet<HalfHourlyData> HalfHourlyData { get; set; }
        public virtual DbSet<HalfYearlyData> HalfYearlyData { get; set; }
        public virtual DbSet<MeterDetails> MeterDetails { get; set; }
        public virtual DbSet<MonthlyConsumptionDetails> MonthlyConsumptionDetails { get; set; }
        public virtual DbSet<MSLiveData> MSLiveData { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<TempMSLiveData> TempMSLiveData { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<UserSession> UserSession { get; set; }
        public virtual DbSet<WeeklyConsumptionPrediction> WeeklyConsumptionPrediction { get; set; }
        public virtual DbSet<PowergridAzureMLData> PowergridAzureMLData { get; set; }
        public virtual DbSet<TempMLData> TempMLData { get; set; }
    }
}
