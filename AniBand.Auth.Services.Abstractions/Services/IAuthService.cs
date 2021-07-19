using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Helpers;
using AniBand.Auth.Services.Abstractions.Models;
using Microsoft.AspNetCore.Http;

namespace AniBand.Auth.Services.Abstractions.Services
{
    public interface IAuthService
    {
        Task<IHttpResult> Register(RegisterUserDto model);

        Task<IHttpResult> Authenticate(LoginUserDto model,HttpContext context);

        Task<IHttpResult> Refresh(string refreshToken);

        Task<IHttpResult> Revoke(string token);
    }
}