namespace RestService.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ApplicationConfigurationEntryModel
    {
        public int Id { get; set; }

        public string ConfigurationKey { get; set; }

        public string ConfigurationValue { get; set; }
    }
}