using NLog;

namespace SearchApp.Api
{
    public interface ILogger
    {
        void RequestLog(HttpContext htcontext, string bodyText, string uid);
        void ResponseLog(HttpContext htcontext, string bodyText, string uid, long apiTime);
        void ErrorLog(HttpContext htcontext, string bodyText, string uid, long apiTime);
    }

    public class Logger : ILogger
    {
        private readonly NLog.ILogger logger;

        // Constructor to initialize the logger
        public Logger()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        public void RequestLog(HttpContext htcontext, string bodyText, string uid)
        {
            AddlogInfo(NLog.LogLevel.Trace, logger, htcontext, uid, "Request", bodyText, 0);
        }

        public void ResponseLog(HttpContext htcontext, string bodyText, string uid, long apiTime)
        {
            AddlogInfo(NLog.LogLevel.Trace, logger, htcontext, uid, "Response", bodyText, apiTime);
        }

        public void ErrorLog(HttpContext htcontext, string bodyText, string uid, long apiTime)
        {
            AddlogInfo(NLog.LogLevel.Error, logger, htcontext, uid, "Response", bodyText, apiTime);
        }

        private void AddlogInfo(NLog.LogLevel logLevel, NLog.ILogger loggerObj, HttpContext htcontext, string uid, string requesttype, string bodyText, long apiTime)
        {
            var logEventInfo = new LogEventInfo(logLevel, loggerObj.Name, requesttype);

            // Get IP Address
            var remoteIp = htcontext.Connection.RemoteIpAddress;
            if (remoteIp != null && remoteIp.IsIPv4MappedToIPv6)
                remoteIp = remoteIp.MapToIPv4();

            string functionName = htcontext.Request.Path.Value?.Split("/")?.LastOrDefault() ?? "Unknown";

            logEventInfo.Properties["CorelationId"] = htcontext.Request?.Headers["CorelationId"].ToString();
            logEventInfo.Properties["UserId"] = uid;
            logEventInfo.Properties["RequestURL"] = GetRequestUrl(htcontext);
            logEventInfo.Message = bodyText;
            logEventInfo.Properties["RequestType"] = requesttype;
            logEventInfo.Properties["IPAddress"] = remoteIp?.ToString();
            logEventInfo.Properties["FunctionName"] = functionName;
            logEventInfo.Properties["StatusCode"] = requesttype == "Request" ? string.Empty : htcontext.Response.StatusCode;
            logEventInfo.Properties["APITime"] = apiTime;
            loggerObj.Log(logEventInfo);
        }

        private string GetRequestUrl(HttpContext httpContext)
        {
            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}{httpContext.Request.QueryString}";
        }
    }
}
