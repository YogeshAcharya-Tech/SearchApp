namespace SearchApp.Domain
{
    public class ApiError
    {
        public string ExceptionMessage { get; set; }
        public string Details { get; set; }
        public IEnumerable<ValidationError> ValidationErrors { get; set; }
        public ApiError(string message)
        {
            this.ExceptionMessage = message;
        }
        public ApiError(string message, IEnumerable<ValidationError> validationErrors)
        {
            this.ExceptionMessage = message;
            this.ValidationErrors = validationErrors;
        }
    }
}
