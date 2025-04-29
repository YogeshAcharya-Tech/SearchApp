using System.Text;

namespace SearchApp.Api
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
            bodyStream.Seek(0, SeekOrigin.Begin);
            var plainBodyText = await new StreamReader(bodyStream).ReadToEndAsync();
            bodyStream.Seek(0, SeekOrigin.Begin);
            return plainBodyText;
        }
    }
}
