namespace RestService.Models
{
    using RestService.Enums;

    public class ResponseModel
    {
        public ResponseModel()
        {
        }

        public ResponseModel(StatusCode statusCode, string message)
        {
            this.Status_Code = (int)statusCode;
            this.Message = message;
        }

        public int Status_Code { get; set; }

        public string Message { get; set; }
    }
}