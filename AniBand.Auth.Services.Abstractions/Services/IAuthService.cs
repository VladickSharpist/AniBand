using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Models;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Core.Abstractions.Infrastructure.Helpers.Generic;

namespace AniBand.Auth.Services.Abstractions.Services
{
    public interface IAuthService
    {
        Task<IHttpResult> RegisterAsync(RegisterUserDto model);
        
        Task<IHttpResult<AuthDto>> AuthenticateAsync(LoginUserDto model);
        
        Task<IHttpResult<RefreshDto>> RefreshAsync(string refreshToken);
        
        Task<IHttpResult> RevokeAsync(string token);
    }
}