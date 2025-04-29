using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SearchApp.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SearchApp.Application
{
    public record AuthCommand(AuthRequest AuthRequest) : IRequest<AuthResponse>;
    public class AuthCommandHandler(IAuthRepository authRepository, IConfiguration _config) : IRequestHandler<AuthCommand, AuthResponse>
    {
        public async Task<AuthResponse> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            var data = await authRepository.GetAccessToken(request.AuthRequest);

            AuthResponse response = new AuthResponse();

            if (data == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }
            else if(!data.ResponseStatus)
            {
                throw new UnauthorizedAccessException(data.ResponseMessage);
            }
            else if (data.ResponseStatus)
            {
                response.AccessToken = GenerateAccessToken(data);
                response.UserId = data.UserId;
                return response;
            }
            else
            {
                throw new UnauthorizedAccessException("Some error occurred!");
            }
        }

        private string GenerateAccessToken(CommonResponse request)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSetting:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, request.UserId),
                new Claim("UserId", request.UserId),
                new Claim(ClaimTypes.Role, "Employee")
            };
            DateTime dateTimeOffset = DateTime.Now.AddMinutes(Convert.ToInt32(_config["JwtSetting:AccessExpireMinutes"]));
            var token = new JwtSecurityToken(
              _config["JwtSetting:Issuer"],
              _config["JwtSetting:Audience"],
              claims: claims,
              expires: dateTimeOffset,
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
