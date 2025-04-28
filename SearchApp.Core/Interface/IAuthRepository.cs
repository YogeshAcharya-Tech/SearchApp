using SearchApp.Core;

namespace SearchApp.Core
{
    public interface IAuthRepository
    {
        Task<CommonResponse> GetAccessToken(AuthRequest AuthRequest);
    }
}
