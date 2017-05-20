﻿namespace RestService.Utilities
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
            source = source.Where(b => b.IsActive && !b.IsDeleted && b.Campus != null && b.Campus.IsActive && !b.Campus.IsDeleted &&
            b.Campus.Role.Any(r => r.IsActive && !r.IsDeleted && r.Id == Context.Current.RoleId));

            if (predicate != null)
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<Campus> WhereActiveCampus(this IQueryable<Campus> source, Expression<Func<Campus, bool>> predicate = null)
        {
            source = source.Where(s => s.IsActive && !s.IsDeleted);
            if (predicate != null)
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<Campus> WhereActiveAccessibleCampus(this IQueryable<Campus> source, Expression<Func<Campus, bool>> predicate = null)
        {
            source = source.Where(s => s.IsActive && !s.IsDeleted && s.Role.Any(r => r.IsActive && !r.IsDeleted && r.Id == Context.Current.RoleId));
            if (predicate != null)
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<University> WhereActiveUniversity(this IQueryable<University> source, Expression<Func<University, bool>> predicate = null)
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

        public static IQueryable<PiServer> WhereActiveAccessiblePiServer(this IQueryable<PiServer> source, Expression<Func<PiServer, bool>> predicate = null)
        {
            source = source.Where(b => b.IsActive && !b.IsDeleted && b.Campus != null && b.Campus.IsActive && !b.Campus.IsDeleted &&
            b.Campus.Role.Any(r => r.IsActive && !r.IsDeleted && r.Id == Context.Current.RoleId));

            if (predicate != null)
            {
                source = source.Where(predicate);
            }

            return source;
        }

        public static IQueryable<MeterDetails> WhereActiveAccessibleMeterDetails(this IQueryable<MeterDetails> source, Expression<Func<MeterDetails, bool>> predicate = null)
        {
            source = source.Where(m => m.Building.IsActive && !m.Building.IsDeleted && m.Building.Campus != null && m.Building.Campus.IsActive && !m.Building.Campus.IsDeleted &&
          m.Building.Campus.Role.Any(r => r.IsActive && !r.IsDeleted && r.Id == Context.Current.RoleId));

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
    }
}