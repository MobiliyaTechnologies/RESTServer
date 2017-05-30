﻿namespace RestService.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

    public class UniversityModelMapping
    {
        public IQueryable<UniversityModel> Map(IQueryable<University> source)
        {
            return from s in source
                   select new UniversityModel
                   {
                       UniversityID = s.UniversityID,
                       UniversityName = s.UniversityName,
                       UniversityDesc = s.UniversityDesc,
                       UniversityAddress = s.UniversityAddress
                   };
        }

        public UniversityModel Map(University source)
        {
            return source == null ? null : this.Map(new List<University> { source }.AsQueryable()).First();
        }
    }
}