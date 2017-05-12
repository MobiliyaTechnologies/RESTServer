namespace RestService.Models
{
    using System.ComponentModel.DataAnnotations;

    public class EmailModel
    {
        [Required(ErrorMessage ="Email subject required.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Email body required.")]
        public string Body { get; set; }

        public bool IsBodyHtml { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Recipient { get; set; }
    }
}