namespace SearchApp.Core.Entities
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        public bool IsModelValidatonError { get; set; }
        public IEnumerable<ValidationError>? Errors { get; set; } = new List<ValidationError>();

        public ApiException(string message, int statusCode = 500) : base(message)
        {
            this.IsModelValidatonError = false;
            this.StatusCode = statusCode;
        }

        public ApiException(IEnumerable<ValidationError> errors, int statusCode = 400)
        {
            this.IsModelValidatonError = true;
            this.StatusCode = statusCode;
            this.Errors = errors;
        }

        public ApiException(Exception ex, int statusCode = 500) : base(ex.Message)
        {
            this.IsModelValidatonError = false;
            StatusCode = statusCode;
        }
    }
}
