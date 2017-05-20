namespace RestService.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Models;

    public class ApplicationConfigurationModelMapping
    {
        public IQueryable<ApplicationConfigurationModel> Map(IQueryable<ApplicationConfiguration> source)
        {
            return from s in source
                   select new ApplicationConfigurationModel
                   {
                       ApplicationConfigurationType = s.ConfigurationType,
                       ApplicationConfigurationEntries = (from entry in s.ApplicationConfigurationEntry
                                                          select new ApplicationConfigurationEntryModel
                                                          {
                                                              Id = entry.Id,
                                                              ConfigurationKey = entry.ConfigurationKey,
                                                              ConfigurationValue = entry.ConfigurationValue
                                                          }).ToList()
                   };
        }

        public ApplicationConfigurationModel Map(ApplicationConfiguration source)
        {
            return source == null ? null : this.Map(new List<ApplicationConfiguration> { source }.AsQueryable()).FirstOrDefault();
        }
    }
}