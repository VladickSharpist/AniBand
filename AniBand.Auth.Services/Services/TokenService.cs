using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Services;
using AniBand.Domain;
using AniBand.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using AniBand.Auth.Services.Abstractions.Models;
using AniBand.DataAccess.Abstractions.Repositories;

namespace AniBand.Auth.Services.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _jwtSettings;
        private readonly IBaseReadWriteRepository<RefreshToken> _refreshTokenReadWriteRepository;

        public TokenService(
            IConfiguration jwtSettings,
            UserManager<User> userManager,
            IBaseReadWriteRepository<RefreshToken> refreshTokenReadWriteRepository)
        {
            _jwtSettings = jwtSettings ?? throw new ArgumentNullException(
                typeof(IConfiguration).ToString());
            _userManager = userManager ?? throw new ArgumentNullException(
                typeof(UserManager<User>).ToString());
            _refreshTokenReadWriteRepository = refreshTokenReadWriteRepository ?? throw new ArgumentNullException(
                typeof(IBaseReadWriteRepository<RefreshToken>).ToString());
        }

        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(
                _jwtSettings.GetSection("JWTSettings:securityKey")
                    .Value);

            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task<List<Claim>> GetClaimsAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(CustomClaimTypes.Actor, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(CustomClaimTypes.Role, role));
            }

            var permissions = (await _userManager.GetClaimsAsync(user))
                .Where(c => c.Type == CustomClaimTypes.Permission)
                .ToList();
            claims.AddRange(permissions);
            return claims;
        }

        public async Task<string> GenerateTokenAsync(User user)
        {
            var claims = await GetClaimsAsync(user);

            var jwt = new JwtSecurityToken(
                issuer: _jwtSettings.GetSection("JWTSettings:validIssuer").Value,
                audience: _jwtSettings.GetSection("JWTSettings:validAudience").Value,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: GetSigningCredentials()
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public string GenerateRefreshToken(User user)
        {
            var token = new RefreshTokenDto
            {
                Created = DateTime.Now, Expires = DateTime.Now.AddDays(7), UserId = user.Id
            };

            return EncodeRefreshToken(token);
        }

        public string EncodeRefreshToken(RefreshTokenDto tokenDto)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(tokenDto));
            return Convert.ToBase64String(plainTextBytes);
        }

        public RefreshTokenDto DecodeRefreshToken(string token)
        {
            var base64EncodedBytes = Convert.FromBase64String(token);
            var decodedToken = JsonSerializer.Deserialize<RefreshTokenDto>(
                Encoding.UTF8.GetString(base64EncodedBytes));
            return decodedToken;
        }

        public void MoveToHistory(User user, RefreshTokenDto tokenDto)
        {
            var historyToken = new RefreshToken()
            {
                Token = EncodeRefreshToken(tokenDto), Owner = user
            };
            _refreshTokenReadWriteRepository.Save(historyToken);
        }

        public async Task<bool> RevokeToken(string token)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.RefreshToken == token);
            if (user == null)
            {
                return false;
            }

            var refreshToken = DecodeRefreshToken(user.RefreshToken);
            if (!refreshToken.IsActive)
            {
                return false;
            }

            refreshToken.Revoked = DateTime.UtcNow;
            user.RefreshToken = EncodeRefreshToken(refreshToken);
            await _userManager.UpdateAsync(user);

            return true;
        }
    }
}