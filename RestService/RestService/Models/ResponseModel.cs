namespace RestService.Models
{
    using RestService.Enums;

    /// <summary>
    /// Provides status of current operations.
    /// </summary>
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

        /// <summary>
        /// Gives status of current operation.
        /// 200 (Ok) -> Operation completed successfully.
        /// 0 (Error) -> operation failed.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public int Status_Code { get; set; }

        /// <summary>
        /// Gives process response, success or failure message.
        /// </summary>
        /// <value>
        /// The response message.
        /// </value>
        public string Message { get; set; }
    }
}