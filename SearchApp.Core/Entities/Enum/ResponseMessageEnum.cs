using System.ComponentModel;

namespace SearchApp.Core
{
    public enum ResponseMessageEnum
    {
        [Description("Request successful.")]
        Success,
        [Description("Request not found. The specified uri does not exist.")]
        NotFound,
        [Description("Request responded with 'Method Not Allowed'.")]
        MethodNotAllowed,
        [Description("Request no content. The specified uri does not contain any content.")]
        NotContent,
        [Description("Request responded with exceptions.")]
        Exception,
        [Description("Request Denied. Unauthorized Access.")]
        UnAuthorized,
        [Description("Request responded with validation error(s). Please correct the specified validation errors and try again.")]
        ValidationError,
        [Description("Request cannot be processed. Please contact a support.")]
        Unknown,
        [Description("Unhandled Exception occured. Unable to process the request.")]
        Unhandled,
        [Description("Unable to process the request.")]
        Failure,
        [Description("Refresh Token Expired.")]
        TokenExpired,
        [Description("You tried multiple wrong attempts, your account got blocked. Please login again to unblock.")]
        UserBlocked,
        [Description("Session Expired :  Invalid Session Key")]
        KbExpired,
        [Description("Someone logged into your account from another device.")]
        Conflict,
        [Description("Request timed out.")]
        RequestTimeout,
        [Description("An unexpected error occurred. Please try again later.")]
        InternalServerError
    }
}
