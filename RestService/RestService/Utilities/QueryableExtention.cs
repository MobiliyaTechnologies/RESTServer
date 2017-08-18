namespace RestService.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Web;
    using RestService.Entities;
    using RestService.Enums;
    using RestService.Services;
    using RestService.Services.Impl;

    public static class QueryableExtention
    {
        private static readonly IContextInfoAccessorService Context = new ContextInfoAccessorService();

        public static IQueryable<Building> WhereActiveBuilding(this IQueryable<Building> source, Expression<Func<Building, bool>> predicate = null)
        {
            source = source.Where(s => s.IsActive && !s.IsDeleted);
            if (predicate != null)
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<Building> WhereActiveAccessibleBuilding(this IQueryable<Building> source, Expression<Func<Building, bool>> predicate = null)
        {
            source = source.Where(b => b.IsActive && !b.IsDeleted && b.Premise != null && b.Premise.IsActive && !b.Premise.IsDeleted &&
            b.Premise.Role.Any(r => r.IsActive && !r.IsDeleted && r.Id == Context.Current.RoleId));

            if (predicate != null)
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<Premise> WhereActivePremise(this IQueryable<Premise> source, Expression<Func<Premise, bool>> predicate = null)
        {
            source = source.Where(s => s.IsActive && !s.IsDeleted);
            if (predicate != null)
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<Premise> WhereActiveAccessiblePremise(this IQueryable<Premise> source, Expression<Func<Premise, bool>> predicate = null)
        {
            source = source.Where(s => s.IsActive && !s.IsDeleted && s.Role.Any(r => r.IsActive && !r.IsDeleted && r.Id == Context.Current.RoleId));
            if (predicate != null)
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<Organization> WhereActiveOrganization(this IQueryable<Organization> source, Expression<Func<Organization, bool>> predicate = null)
        {
            source = source.Where(s => s.IsActive && !s.IsDeleted);
            if (predicate != null)
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<Role> WhereActiveAccessibleRole(this IQueryable<Role> source, Expression<Func<Role, bool>> predicate = null)
        {
            source = source.Where(s => s.IsActive && !s.IsDeleted && s.Id == Context.Current.RoleId);
            if (predicate != null)
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<Role> WhereActiveRole(this IQueryable<Role> source, Expression<Func<Role, bool>> predicate = null)
        {
            source = source.Where(s => s.IsActive && !s.IsDeleted);
            if (predicate != null)
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<PiServer> WhereActivePiServer(this IQueryable<PiServer> source, Expression<Func<PiServer, bool>> predicate = null)
        {
            source = source.Where(b => b.IsActive && !b.IsDeleted);

            if (predicate != null)
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<MeterDetails> WhereActiveAccessibleMeterDetails(this IQueryable<MeterDetails> source, Expression<Func<MeterDetails, bool>> predicate = null)
        {
            source = source.Where(m => m.Building.IsActive && !m.Building.IsDeleted && m.Building.Premise != null && m.Building.Premise.IsActive && !m.Building.Premise.IsDeleted &&
          m.Building.Premise.Role.Any(r => r.IsActive && !r.IsDeleted && r.Id == Context.Current.RoleId));

            if (predicate != null)
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<User> WhereActiveUser(this IQueryable<User> source, Expression<Func<User, bool>> predicate = null)
        {
            source = source.Where(s => !s.IsDeleted);

            if (predicate != null)
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<RoomDetail> WhereActiveAccessibleRoom(this IQueryable<RoomDetail> source, Expression<Func<RoomDetail, bool>> predicate = null)
        {
            source = source.Where(b => b.Building != null && b.Building.IsActive && !b.Building.IsDeleted && b.Building.Premise != null && b.Building.Premise.IsActive && !b.Building.Premise.IsDeleted &&
            b.Building.Premise.Role.Any(r => r.IsActive && !r.IsDeleted && r.Id == Context.Current.RoleId));

            if (predicate != null)
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<RoomDetail> WhereActiveRoom(this IQueryable<RoomDetail> source, Expression<Func<RoomDetail, bool>> predicate = null)
        {
            source = source.Where(b => b.Building != null && b.Building.IsActive && !b.Building.IsDeleted && b.Building.Premise != null && b.Building.Premise.IsActive && !b.Building.Premise.IsDeleted);

            if (predicate != null)
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<Feedback> WhereInDateRange(this IQueryable<Feedback> source)
        {
            DateTime startDate, endDate;
            GetStartAndEndDate(out startDate, out endDate);

            if (startDate != DateTime.MinValue)
            {
                source = source.Where(s => DbFunctions.TruncateTime(s.CreatedOn) >= startDate);
            }

            if (endDate != DateTime.MinValue)
            {
                source = source.Where(s => DbFunctions.TruncateTime(s.CreatedOn) <= endDate);
            }

            return source;
        }

        public static IQueryable<Alerts> WhereInDateRange(this IQueryable<Alerts> source)
        {
            DateTime startDate, endDate;
            GetStartAndEndDate(out startDate, out endDate);

            var noFilter = new List<string> { "Device Alert", "Temperature Alert" };

            if (startDate != DateTime.MinValue)
            {
                source = source.Where(s => DbFunctions.TruncateTime(s.Timestamp) >= startDate || noFilter.Contains(s.Alert_Type));
            }

            if (endDate != DateTime.MinValue)
            {
                source = source.Where(s => DbFunctions.TruncateTime(s.Timestamp) <= endDate || noFilter.Contains(s.Alert_Type));
            }

            return source;
        }

        public static IQueryable<DailyConsumptionDetails> WhereInDateRange(this IQueryable<DailyConsumptionDetails> source)
        {
            DateTime startDate, endDate;
            GetStartAndEndDate(out startDate, out endDate);

            if (startDate != DateTime.MinValue)
            {
                source = source.Where(s => DbFunctions.TruncateTime(s.Timestamp) >= startDate);
            }

            if (endDate != DateTime.MinValue)
            {
                source = source.Where(s => DbFunctions.TruncateTime(s.Timestamp) <= endDate);
            }

            return source;
        }

        public static IQueryable<WeeklyConsumptionPrediction> WhereInDateRange(this IQueryable<WeeklyConsumptionPrediction> source)
        {
            DateTime startDate, endDate;
            GetStartAndEndDate(out startDate, out endDate);
            if (startDate != DateTime.MinValue)
            {
                source = source.Where(s => DbFunctions.TruncateTime(s.Start_Time) >= startDate);
            }

            if (endDate != DateTime.MinValue)
            {
                source = source.Where(s => DbFunctions.TruncateTime(s.End_Time) <= endDate);
            }

            return source;
        }

        public static void GetStartAndEndDate(out DateTime startDate, out DateTime endDate)
        {
            startDate = DateTime.MinValue;
            endDate = DateTime.MinValue;

            DateFilter dateFilter = default(DateFilter);

            try
            {
                var request = (HttpRequestMessage)HttpContext.Current.Items["MS_HttpRequestMessage"];
                var queryParam = request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);

                if (queryParam.ContainsKey("DateFilter"))
                {
                    var value = Convert.ToInt32(queryParam["DateFilter"]);
                    if (typeof(DateFilter).IsEnumDefined(value))
                    {
                        dateFilter = (DateFilter)value;
                    }
                }
            }
            catch (Exception)
            {
            }

            var lastDayOfWeek = 0;
            var monthDate = default(DateTime);

            switch (dateFilter)
            {
                case DateFilter.Yesterday:
                    var lastMonth = DateTime.Today.AddMonths(-1);
                    startDate = endDate = new DateTime(lastMonth.Year, lastMonth.Month, DateTime.DaysInMonth(lastMonth.Year, lastMonth.Month));
                    break;

                case DateFilter.Todaty:
                    startDate = endDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    break;

                case DateFilter.Week1:
                    startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                     lastDayOfWeek = 6 - (int)startDate.DayOfWeek;
                    endDate = startDate.AddDays(lastDayOfWeek);
                    break;

                case DateFilter.Week2:
                    startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                     lastDayOfWeek = (6 - (int)startDate.DayOfWeek) + 7;
                    endDate = startDate.AddDays(lastDayOfWeek);
                    break;

                case DateFilter.Week3:
                    startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    lastDayOfWeek = (6 - (int)startDate.DayOfWeek) + 14;
                    endDate = startDate.AddDays(lastDayOfWeek);
                    break;

                case DateFilter.Week4:
                    startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    lastDayOfWeek = (6 - (int)startDate.DayOfWeek) + 21;
                    endDate = startDate.AddDays(lastDayOfWeek);
                    break;

                case DateFilter.Month1:
                    startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    endDate = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));
                    break;

                case DateFilter.Month2:
                    monthDate = DateTime.Today.AddMonths(1);
                    startDate = new DateTime(monthDate.Year, monthDate.Month, 1);
                    endDate = new DateTime(monthDate.Year, monthDate.Month, DateTime.DaysInMonth(monthDate.Year, monthDate.Month));
                    break;

                case DateFilter.Month3:
                    monthDate = DateTime.Today.AddMonths(2);
                    startDate = new DateTime(monthDate.Year, monthDate.Month, 1);
                    endDate = new DateTime(monthDate.Year, monthDate.Month, DateTime.DaysInMonth(monthDate.Year, monthDate.Month));
                    break;

                case DateFilter.Month4:
                    monthDate = DateTime.Today.AddMonths(3);
                    startDate = new DateTime(monthDate.Year, monthDate.Month, 1);
                    endDate = new DateTime(monthDate.Year, monthDate.Month, DateTime.DaysInMonth(monthDate.Year, monthDate.Month));
                    break;

                case DateFilter.Month5:
                    monthDate = DateTime.Today.AddMonths(4);
                    startDate = new DateTime(monthDate.Year, monthDate.Month, 1);
                    endDate = new DateTime(monthDate.Year, monthDate.Month, DateTime.DaysInMonth(monthDate.Year, monthDate.Month));
                    break;

                case DateFilter.Month6:
                    monthDate = DateTime.Today.AddMonths(5);
                    startDate = new DateTime(monthDate.Year, monthDate.Month, 1);
                    endDate = new DateTime(monthDate.Year, monthDate.Month, DateTime.DaysInMonth(monthDate.Year, monthDate.Month));
                    break;

                default:
                    break;
            }
        }
    }
}