using Microsoft.EntityFrameworkCore;
using SearchApp.Core.Interface;
using SearchApp.Infrastructure.Data;
using SearchApp.Core.Entities;

namespace SearchApp.Infrastructure.Repositories
{
    public class AuthRepository(AppDbContext dbContext) : IAuthRepository
    {
        public async Task<CommonResponse> GetAccessToken(AuthRequest AuthRequest)
        {
            var data = await dbContext.Employees.FirstOrDefaultAsync(x => x.Email == AuthRequest.Username && x.Password == AuthRequest.Password);

            if (data != null) 
            {
                return new CommonResponse { UserId = data.Id.ToString(), ResponseStatus = true, ResponseMessage = "Logged in successfully!" };
            }
            else
            {
                return new CommonResponse { ResponseStatus = false, ResponseMessage = "Failed: Invalid username or password" };
            }
        }
    }
}
