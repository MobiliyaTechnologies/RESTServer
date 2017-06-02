namespace RestService.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ApplicationConfigurationEntryModel
    {
        public int Id { get; set; }

        [Required]
        public string ConfigurationKey { get; set; }

        [Required]
        public string ConfigurationValue { get; set; }
    }
}