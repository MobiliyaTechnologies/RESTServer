namespace RestService.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using RestService.Entities;
    using RestService.Enums;
    using RestService.Mappings;
    using RestService.Models;
    using RestService.Utilities;

    public class ApplicationConfigurationService : IApplicationConfigurationService, IDisposable
    {
        private readonly PowerGridEntities dbContext;
        private readonly IContextInfoAccessorService context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationConfigurationService"/> class.
        /// </summary>
        public ApplicationConfigurationService()
        {
            this.dbContext = new PowerGridEntities();
            this.context = new ContextInfoAccessorService();
        }

        ResponseModel IApplicationConfigurationService.AddApplicationConfiguration(ApplicationConfigurationModel applicationConfigurationModel)
        {
            var applicationConfiguration = this.dbContext.ApplicationConfiguration.FirstOrDefault(a => a.ConfigurationType.Equals(applicationConfigurationModel.ApplicationConfigurationType.Trim(), StringComparison.InvariantCultureIgnoreCase));

            if (applicationConfiguration == null)
            {
                return new ResponseModel(StatusCode.Error, "Invalid application configuration type.");
            }

            // adding notificationClickActionEntry in database required for data services component.
            if (applicationConfigurationModel.ApplicationConfigurationType.Trim().Equals(ApiConstant.FirebaseApplicationConfiguration, StringComparison.InvariantCultureIgnoreCase))
            {
                var notificationClickActionEntry = applicationConfiguration.ApplicationConfigurationEntry.FirstOrDefault(e => e.ConfigurationKey.Equals(ApiConstant.NotificationClickActionKey, StringComparison.InvariantCultureIgnoreCase));

                if (notificationClickActionEntry == null)
                {
                    var configEntry = new ApplicationConfigurationEntry()
                    {
                        ConfigurationKey = ApiConstant.NotificationClickActionKey,
                        ConfigurationValue = ApiConfiguration.NotificationClickAction,
                        ApplicationConfigurationId = applicationConfiguration.Id,
                        CreatedBy = this.context.Current.UserId,
                        CreatedOn = DateTime.UtcNow
                    };

                    this.dbContext.ApplicationConfigurationEntry.Add(configEntry);
                }
                else if (!notificationClickActionEntry.ConfigurationValue.Equals(ApiConfiguration.NotificationClickAction, StringComparison.InvariantCultureIgnoreCase))
                {
                    notificationClickActionEntry.ConfigurationValue = ApiConfiguration.NotificationClickAction;
                    notificationClickActionEntry.ModifiedBy = this.context.Current.UserId;
                    notificationClickActionEntry.ModifiedOn = DateTime.UtcNow;
                }
            }

            foreach (var applicationConfigurationEntry in applicationConfigurationModel.ApplicationConfigurationEntries)
            {
                var updateConfig = applicationConfiguration.ApplicationConfigurationEntry.FirstOrDefault(e => e.ConfigurationKey.Equals(applicationConfigurationEntry.ConfigurationKey, StringComparison.InvariantCultureIgnoreCase));

                if (updateConfig != null)
                {
                    updateConfig.ConfigurationValue = applicationConfigurationEntry.ConfigurationValue;
                    updateConfig.ModifiedBy = this.context.Current.UserId;
                    updateConfig.ModifiedOn = DateTime.UtcNow;
                }
                else
                {
                    var configEntry = new ApplicationConfigurationEntry()
                    {
                        ConfigurationKey = applicationConfigurationEntry.ConfigurationKey,
                        ConfigurationValue = applicationConfigurationEntry.ConfigurationValue,
                        ApplicationConfigurationId = applicationConfiguration.Id,
                        CreatedBy = this.context.Current.UserId,
                        CreatedOn = DateTime.UtcNow
                    };

                    this.dbContext.ApplicationConfigurationEntry.Add(configEntry);
                }
            }

            this.dbContext.SaveChanges();
            return new ResponseModel(StatusCode.Ok, "Application configuration added successfully.");
        }

        ResponseModel IApplicationConfigurationService.DeleteApplicationConfiguration(string applicationConfigurationType)
        {
            var applicationConfiguration = this.dbContext.ApplicationConfiguration.FirstOrDefault(a => a.ConfigurationType.Equals(applicationConfigurationType.Trim(), StringComparison.InvariantCultureIgnoreCase));

            if (applicationConfiguration == null)
            {
                return new ResponseModel(StatusCode.Error, "Invalid application configuration type.");
            }

            this.dbContext.ApplicationConfigurationEntry.RemoveRange(applicationConfiguration.ApplicationConfigurationEntry);
            this.dbContext.SaveChanges();

            return new ResponseModel(StatusCode.Ok, "Application configuration deleted successfully.");
        }

        ResponseModel IApplicationConfigurationService.DeleteApplicationConfigurationEntry(int applicationConfigurationEntryId)
        {
            var applicationConfigurationEntry = this.dbContext.ApplicationConfigurationEntry.FirstOrDefault(a => a.Id == applicationConfigurationEntryId);

            if (applicationConfigurationEntry == null)
            {
                return new ResponseModel(StatusCode.Error, "Application configuration entry does not exists.");
            }

            this.dbContext.ApplicationConfigurationEntry.Remove(applicationConfigurationEntry);
            this.dbContext.SaveChanges();

            return new ResponseModel(StatusCode.Ok, "Application configuration entry deleted successfully.");
        }

        List<ApplicationConfigurationModel> IApplicationConfigurationService.GetAllApplicationConfiguration()
        {
            var applicationConfigurations = this.dbContext.ApplicationConfiguration;

            var applicationConfigurationModels = new ApplicationConfigurationModelMapping().Map(applicationConfigurations.AsQueryable()).ToList();

            return applicationConfigurationModels;
        }

        ApplicationConfigurationModel IApplicationConfigurationService.GetApplicationConfiguration(string applicationConfigurationType)
        {
            var applicationConfigurations = this.dbContext.ApplicationConfiguration.FirstOrDefault(a => a.ConfigurationType.Equals(applicationConfigurationType.Trim(), StringComparison.InvariantCultureIgnoreCase));

            var applicationConfigurationModel = new ApplicationConfigurationModelMapping().Map(applicationConfigurations);

            return applicationConfigurationModel;
        }

        ResponseModel IApplicationConfigurationService.UpdateApplicationConfigurationEntry(ApplicationConfigurationEntryModel applicationConfigurationEntryModel)
        {
            var applicationConfigurationEntry = this.dbContext.ApplicationConfigurationEntry.FirstOrDefault(a => a.Id == applicationConfigurationEntryModel.Id);

            if (applicationConfigurationEntry == null)
            {
                return new ResponseModel(StatusCode.Error, "Application configuration entry does not exists.");
            }

            applicationConfigurationEntry.ConfigurationKey = string.IsNullOrWhiteSpace(applicationConfigurationEntryModel.ConfigurationKey) ? applicationConfigurationEntry.ConfigurationKey : applicationConfigurationEntryModel.ConfigurationKey;
            applicationConfigurationEntry.ConfigurationValue = string.IsNullOrWhiteSpace(applicationConfigurationEntryModel.ConfigurationValue) ? applicationConfigurationEntry.ConfigurationValue : applicationConfigurationEntryModel.ConfigurationValue;
            applicationConfigurationEntry.ModifiedBy = this.context.Current.UserId;
            applicationConfigurationEntry.ModifiedOn = DateTime.UtcNow;

            this.dbContext.SaveChanges();

            return new ResponseModel(StatusCode.Ok, "Application configuration entry updated successfully");
        }

        void IDisposable.Dispose()
        {
            if (this.dbContext != null)
            {
                this.dbContext.Dispose();
            }
        }
    }
}