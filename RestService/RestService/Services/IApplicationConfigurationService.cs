namespace RestService.Services
{
    using System.Collections.Generic;
    using RestService.Enums;
    using RestService.Models;

    /// <summary>
    /// Provides operations to perform CRUD on application configuration.
    /// </summary>
    public interface IApplicationConfigurationService
    {
        /// <summary>
        /// Gets all application configuration.
        /// </summary>
        /// <returns>The application configuration list.</returns>
        List<ApplicationConfigurationModel> GetAllApplicationConfiguration();

        /// <summary>
        /// Gets the application configuration for given configuration type..
        /// </summary>
        /// <param name="applicationConfigurationType">Type of the application configuration.</param>
        /// <returns>The application configuration.</returns>
        ApplicationConfigurationModel GetApplicationConfiguration(string applicationConfigurationType);

        /// <summary>
        /// Adds the application configuration.
        /// </summary>
        /// <param name="applicationConfigurationModel">The application configuration model.</param>
        /// <returns>Application configuration added confirmation.</returns>
        ResponseModel AddApplicationConfiguration(ApplicationConfigurationModel applicationConfigurationModel);

        /// <summary>
        /// Deletes the application configuration.
        /// </summary>
        /// <param name="applicationConfigurationType">Type of the application configuration.</param>
        /// <returns>Application configuration deleted confirmation.</returns>
        ResponseModel DeleteApplicationConfiguration(string applicationConfigurationType);

        /// <summary>
        /// Updates the application configuration entry.
        /// </summary>
        /// <param name="applicationConfigurationEntryModel">The application configuration entry model.</param>
        /// <returns>Application configuration entry update confirmation.</returns>
        ResponseModel UpdateApplicationConfigurationEntry(ApplicationConfigurationEntryModel applicationConfigurationEntryModel);

        /// <summary>
        /// Deletes the application configuration entry.
        /// </summary>
        /// <param name="applicationConfigurationEntryId">The application configuration entry identifier.</param>
        /// <returns>Application configuration entry deleted confirmation.</returns>
        ResponseModel DeleteApplicationConfigurationEntry(int applicationConfigurationEntryId);
    }
}
