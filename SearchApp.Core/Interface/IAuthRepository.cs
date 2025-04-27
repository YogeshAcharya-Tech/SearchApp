using SearchApp.Core.Entities;

namespace SearchApp.Core.Interface
{
    public interface IAuthRepository
    {
        Task<CommonResponse> GetAccessToken(AuthRequest AuthRequest);
    }
}
