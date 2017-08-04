namespace RestService.Utilities
{
    using System;
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
            var count = 0;

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

                if (queryParam.ContainsKey("Count"))
                {
                    count = Convert.ToInt32(queryParam["Count"]);
                }
            }
            catch (Exception)
            {
            }

            switch (dateFilter)
            {
                case DateFilter.Day:
                    startDate = endDate = DateTime.Today.AddDays(count);
                    break;

                case DateFilter.Week:
                    startDate = DateTime.Today.AddDays(-((int)DateTime.Today.DayOfWeek));
                    var lastDayOfWeek = 6 + (7 * count);
                    endDate = startDate.AddDays(lastDayOfWeek);
                    break;

                case DateFilter.Month:
                    startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    endDate = startDate.AddMonths(count);
                    endDate = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(startDate.Year, endDate.Month));
                    break;

                case DateFilter.Year:
                    startDate = new DateTime(DateTime.Today.Year, 1, 1);
                    var year = startDate.Year + count;
                    endDate = new DateTime(year, 12, 31);
                    break;

                default:
                    break;
            }
        }
    }
}