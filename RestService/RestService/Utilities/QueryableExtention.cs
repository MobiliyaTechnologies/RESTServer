namespace RestService.Utilities
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using RestService.Entities;
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
    }
}