using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SearchApp.Domain;
using System.Diagnostics;
using System.Net;

namespace SearchApp.Api
{
    public class ApiResponseMiddleware
    {
        private readonly RequestDelegate _next; //Used to execute and pass control in order of sequence
        private readonly ILogger _logger; // Using NLog

        public ApiResponseMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            var stopWatch = Stopwatch.StartNew();

            var CorelationId = GenerateCorrelationId(); // CorelationId mapping with each unique request and response

            // Use indexer or Append to avoid exception if "CorelationId" already exists
            httpContext.Request.Headers.Add("CorelationId", CorelationId);

            var bodyAsText = await RequestHelper.FormatRequest(httpContext.Request);

            // Insert request log - HttpContext htcontext, string bodyText, string uid         
            _logger.RequestLog(httpContext, bodyAsText, "");

            // Validate input
            if (string.IsNullOrWhiteSpace(bodyAsText))
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await HandleNotSuccessRequestAsync(httpContext, httpContext.Response.StatusCode, "", stopWatch.ElapsedMilliseconds);
                return;
            }

            await HandleRequestAsync(httpContext, "", stopWatch);
        }

        /// <summary> Handle Http Request
        /// </summary>
        private async Task HandleRequestAsync(HttpContext context, string uid, Stopwatch stopWatch)
        {
            var originalBodyStream = context.Response.Body;
            using (var bodyStream = new MemoryStream())
            {
                var bodyAsText = string.Empty;
                try
                {
                    context.Response.Body = bodyStream;

                    await _next.Invoke(context);

                    context.Response.Body = originalBodyStream;

                    bodyAsText = await RequestHelper.FormatResponse(bodyStream);
                    if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                    {
                        await HandleSuccessRequestAsync(context, bodyAsText, context.Response.StatusCode);
                    }
                    else
                    {
                        if ((context.Response.StatusCode == (int)HttpStatusCode.NotFound || context.Response.StatusCode == (int)HttpStatusCode.BadRequest) && bodyAsText != null)
                            await HandleNotSuccessRequestAsync(context, bodyAsText, context.Response.StatusCode, uid, stopWatch.ElapsedMilliseconds);
                        else
                            await HandleNotSuccessRequestAsync(context, context.Response.StatusCode, uid, stopWatch.ElapsedMilliseconds);
                    }
                }
                catch (Exception ex)
                {
                    await HandleExceptionAsync(context, ex, uid, stopWatch.ElapsedMilliseconds);
                    bodyStream.Seek(0, SeekOrigin.Begin);
                    await bodyStream.CopyToAsync(originalBodyStream);
                }
                finally
                {
                    stopWatch.Stop();
                    if (context.Response.StatusCode == (int)HttpStatusCode.OK)
                        _logger.ResponseLog(context, bodyAsText, uid, stopWatch.ElapsedMilliseconds);
                }
            }
        }

        /// <summary> Handling Global Exception
        /// </summary>
        private Task HandleExceptionAsync(HttpContext context, Exception exception, string uid, long apiTime)
        {
            ApiError apiError;
            int code = 0;
            string strMessage;

            if (exception is ApiException ex)
            {
                if (ex.IsModelValidatonError)
                {
                    apiError = new ApiError(ResponseMessageEnum.ValidationError.GetDescription(), ex.Errors ?? new List<ValidationError>());
                    strMessage = $"{ResponseMessageEnum.ValidationError.GetDescription()} {exception}";
                }
                else
                {
                    apiError = new ApiError(ex.Message);
                    strMessage = $"{ResponseMessageEnum.Exception.GetDescription()} {exception}";
                }

                code = ex.StatusCode;
                context.Response.StatusCode = code;
            }
            else if (exception is UnauthorizedAccessException)
            {
                apiError = new ApiError(ResponseMessageEnum.UnAuthorized.GetDescription());
                code = (int)HttpStatusCode.Unauthorized;
                context.Response.StatusCode = code;
                strMessage = $"{ResponseMessageEnum.UnAuthorized.GetDescription()} {exception}";
            }
            else
            {
                var exceptionMessage = ResponseMessageEnum.Unhandled.GetDescription();
                var message = $"{exceptionMessage} {exception.GetBaseException().Message}";

                apiError = new ApiError(message);
                code = (int)HttpStatusCode.InternalServerError;
                context.Response.StatusCode = code;
                strMessage = $"{message} {exception}";
            }

            // Insert ErrorLog
            _logger.ErrorLog(context, strMessage, uid, apiTime);

            var jsonString = ConvertToJSONString(GetErrorResponse(code, apiError));
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(jsonString);
        }

        /// <summary> Handling Not Success Request
        /// </summary>
        private Task HandleNotSuccessRequestAsync(HttpContext context, int code, string uid, long apiTime)
        {
            ApiError apiError;

            if (code == (int)HttpStatusCode.NotFound)
                apiError = new ApiError(ResponseMessageEnum.NotFound.GetDescription());
            else if (code == (int)HttpStatusCode.NoContent)
                apiError = new ApiError(ResponseMessageEnum.NotContent.GetDescription());
            else if (code == (int)HttpStatusCode.MethodNotAllowed)
                apiError = new ApiError(ResponseMessageEnum.MethodNotAllowed.GetDescription());
            else if (code == (int)HttpStatusCode.Unauthorized)
                apiError = new ApiError(ResponseMessageEnum.UnAuthorized.GetDescription());
            else if (code == (int)HttpStatusCode.Conflict)
                apiError = new ApiError(ResponseMessageEnum.Conflict.GetDescription());
            else if (code == (int)HttpStatusCode.UnprocessableEntity)
                apiError = new ApiError(ResponseMessageEnum.UserBlocked.GetDescription());
            else if (code == (int)HttpStatusCode.Forbidden)
                apiError = new ApiError(ResponseMessageEnum.KbExpired.GetDescription());            
            else
                apiError = new ApiError(ResponseMessageEnum.Unknown.GetDescription());

            context.Response.StatusCode = code;

            var jsonString = ConvertToJSONString(GetErrorResponse(code, apiError));

            // Insert ErrorLog
            _logger.ErrorLog(context, jsonString, uid, apiTime);

            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(jsonString);
        }

        /// <summary> Handling Not Success Request
        /// </summary>
        private Task HandleNotSuccessRequestAsync(HttpContext context, object body, int code, string uid, long apiTime)
        {
            var bodyText = body.ToString();
            if (!bodyText.IsValidJson()) bodyText = ConvertToJSONString(body);

            if (string.IsNullOrEmpty(bodyText))
            {
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync(ConvertToJSONString(code, new { message = "Empty response body" }));
            }

            dynamic? bodyContent = JsonConvert.DeserializeObject<dynamic>(bodyText);
            Type? type = bodyContent?.GetType();

            string jsonString;
            if (type == typeof(JObject))
            {
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(bodyText);
                if (apiResponse != null && apiResponse.StatusCode == 0)
                {
                    //When Want to return other response format 
                    jsonString = ConvertToJSONString(bodyContent);
                }
                else
                {
                    if (apiResponse != null && ((apiResponse.StatusCode != code || apiResponse.Result != null) ||
                        (apiResponse.StatusCode == code && apiResponse.Result == null)))
                        jsonString = ConvertToJSONString(apiResponse);
                    else
                        jsonString = ConvertToJSONString(code, bodyContent);
                }
            }
            else
            {
                jsonString = ConvertToJSONString(code, bodyContent);
            }

            // Insert ErrorLog
            _logger.ErrorLog(context, jsonString, uid, apiTime);

            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(jsonString);
        }

        /// <summary> Unique Id To Map A Request And Reponse
        /// </summary>
        private string GenerateCorrelationId()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary> Handling Success Request
        /// </summary>
        private Task HandleSuccessRequestAsync(HttpContext context, object body, int code)
        {
            string jsonString = string.Empty;

            var bodyText = !body.ToString().IsValidJson() ? ConvertToJSONString(body) : body.ToString();

            dynamic bodyContent = JsonConvert.DeserializeObject<dynamic>(bodyText);
            Type type = bodyContent?.GetType();

            if (type.Equals(typeof(Newtonsoft.Json.Linq.JObject)))
            {
                ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(bodyText);
                if ((apiResponse.StatusCode != code || apiResponse.Result != null) ||
                    (apiResponse.StatusCode == code && apiResponse.Result == null))
                    jsonString = ConvertToJSONString(apiResponse);
                else
                    jsonString = ConvertToJSONString(code, bodyContent);
            }
            else
            {
                jsonString = ConvertToJSONString(code, bodyContent);
            }

            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(jsonString);
        }
        private string ConvertToJSONString(int code, object content)
        {
            return JsonConvert.SerializeObject(new ApiResponse(ResponseMessageEnum.Success.GetDescription(), content, code), JSONSettings());
        }
        private string ConvertToJSONString(ApiResponse apiResponse)
        {
            return JsonConvert.SerializeObject(apiResponse, JSONSettings());
        }
        private string ConvertToJSONString(object rawJSON)
        {
            return JsonConvert.SerializeObject(rawJSON, JSONSettings());
        }
        private JsonSerializerSettings JSONSettings()
        {
            return new JsonSerializerSettings
            {
                //For CamelCase Changes
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                //For PascalCase Changes
                ContractResolver = new DefaultContractResolver(),
                //NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };
        }

        /// <summary> Common method for custom error response
        /// </summary>
        private ApiResponse GetErrorResponse(int code, ApiError apiError)
        {
            return new ApiResponse(code, apiError);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ApiResponseMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiResponseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiResponseMiddleware>();
        }
    }
}
