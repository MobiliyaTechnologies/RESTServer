namespace RestService.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using RestService.Enums;

    public class ApplicationConfigurationModel
    {
        public ApplicationConfigurationModel()
        {
           this.ApplicationConfigurationEntries = new List<ApplicationConfigurationEntryModel>();
        }

        [Required]
        public string ApplicationConfigurationType { get; set; }

        public List<ApplicationConfigurationEntryModel> ApplicationConfigurationEntries { get; set; }
    }
}