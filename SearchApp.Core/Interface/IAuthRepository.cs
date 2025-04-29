namespace SearchApp.Domain
{
    public interface IAuthRepository
    {
        Task<CommonResponse> GetAccessToken(AuthRequest AuthRequest);
    }
}
