using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Helpers;
using AniBand.Auth.Services.Abstractions.Models;

namespace AniBand.Auth.Services.Abstractions.Services
{
    public interface IAuthService
    {
        Task<IHttpResult> Register(RegisterUserDto model);
        
        Task<IHttpResult<AuthDto>> Authenticate(LoginUserDto model);
        
        Task<IHttpResult<RefreshDto>> Refresh(string refreshToken);
        
        Task<IHttpResult> Revoke(string token);
        
    }
}