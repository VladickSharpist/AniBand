using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Models;
using AniBand.Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace AniBand.Auth.Services.Abstractions.Services
{
    public interface ITokenService
    {
        SigningCredentials GetSigningCredentials();
        
        Task<List<Claim>> GetClaimsAsync(User user);
        
        Task<string> GenerateTokenAsync(User user);
        
        string GenerateRefreshToken(User user);

        RefreshTokenDto DecodeRefreshToken(string token);

        string EncodeRefreshToken(RefreshTokenDto tokenDto);

        void MoveToHistory(
            User user, 
            RefreshTokenDto tokenDto);

        Task<bool> RevokeTokenAsync(string token);
    }
}