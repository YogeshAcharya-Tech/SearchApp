using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace SearchApp.Domain
{
    public class ApiResponse
    {
        [DataMember]
        public int StatusCode { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public bool IsError { get; set; }

        [DataMember]
        public object ResponseException { get; set; }

        [DataMember]
        public object Result { get; set; }

        [JsonConstructor]
        public ApiResponse(string message, object result = null, int statusCode = 200)
        {
            this.StatusCode = statusCode;
            this.Message = message;
            this.Result = result;
            this.ResponseException = null;
            this.IsError = false;
        }
        public ApiResponse(int statusCode, ApiError apiError)
        {
            this.StatusCode = statusCode;
            this.Message = "Fail";
            this.Result = null;
            this.ResponseException = apiError;
            this.IsError = true;
        }
    }
}
