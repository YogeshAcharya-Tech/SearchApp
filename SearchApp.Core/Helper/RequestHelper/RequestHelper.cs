using Microsoft.AspNetCore.Http;
using System.Text;

namespace SearchApp.Core
{
    public class RequestHelper
    {
        public static async Task<string> FormatRequest(HttpRequest request)
        {
            if (request == null || request.ContentLength == null || request.ContentLength == 0)
                return "No Request Data";

            request.EnableBuffering(); // Allow multiple reads

            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            string bodyAsText = await reader.ReadToEndAsync();

            request.Body.Seek(0, SeekOrigin.Begin); // Reset stream position

            return bodyAsText;
        }

        public static async Task<string> FormatResponse(Stream bodyStream)
        {
            //if (bodyStream == null || !bodyStream.CanSeek)
            //    return "No Response Data";

            //bodyStream.Seek(0, SeekOrigin.Begin);

            //using var reader = new StreamReader(bodyStream, Encoding.UTF8, leaveOpen: true);
            //string responseText = await reader.ReadToEndAsync();

            //bodyStream.Seek(0, SeekOrigin.Begin); // Reset for reuse

            //return responseText;

            bodyStream.Seek(0, SeekOrigin.Begin);
            var plainBodyText = await new StreamReader(bodyStream).ReadToEndAsync();
            bodyStream.Seek(0, SeekOrigin.Begin);
            return plainBodyText;
        }
    }
}
