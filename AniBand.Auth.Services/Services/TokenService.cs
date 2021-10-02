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
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using AniBand.Auth.Services.Abstractions.Models;
using AniBand.Auth.Services.Extensions;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.DataAccess.Abstractions.Repositories;
using Microsoft.AspNetCore.Identity;

namespace AniBand.Auth.Services.Services
{
    internal class TokenService : ITokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserSetter _currentUserSetter;
        private readonly IConfigurationHelper _configurationHelper;
        private readonly IUnitOfWork _unitOfWork;

        public TokenService(
            UserManager<User> userManager,
            IUserSetter currentUserSetter, 
            IConfigurationHelper configurationHelper, 
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager
                ?? throw new NullReferenceException(nameof(userManager));
            _currentUserSetter = currentUserSetter
                ?? throw new NullReferenceException(nameof(currentUserSetter));
            _configurationHelper = configurationHelper
                ?? throw new NullReferenceException(nameof(configurationHelper));
            _unitOfWork = unitOfWork
                ?? throw new NullReferenceException(nameof(unitOfWork));
        }

        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(
                _configurationHelper
                    .SecretKey);

            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public async Task<List<Claim>> GetClaimsAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(CustomClaimTypes.Id, user.Id.ToString())
            };

            var roles = (await _userManager.GetRolesAsync(user))
                .Select(role => 
                    new Claim(CustomClaimTypes.Role, role));
            claims.AddRange(roles);

            var permissions = (await _userManager.GetClaimsAsync(user))
                .Where(c => 
                    c.Type == CustomClaimTypes.Permission)
                .ToList();
            claims.AddRange(permissions);
            return claims;
        }

        public async Task<string> GenerateTokenAsync(User user)
        {
            var claims = await GetClaimsAsync(user);

            var jwt = new JwtSecurityToken(
                issuer:_configurationHelper.Issuer,
                audience: _configurationHelper.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(
                    _configurationHelper.TokenExpireSeconds),
                signingCredentials: GetSigningCredentials()
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public string GenerateRefreshToken(User user)
        {
            var token = new RefreshTokenDto
            {
                Created = DateTime.Now, 
                Expires = DateTime.Now.AddDays(7), 
                UserId = user.Id
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
            var historyToken = new RefreshToken
            {
                Token = EncodeRefreshToken(tokenDto),
                OwnerId = user.Id
            };
            
            _unitOfWork
                .GetReadWriteRepository<RefreshToken>()
                .Save(historyToken);
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var userId = (await _unitOfWork
                    .GetReadonlyRepository<UserToken>()
                    .GetNoTrackingAsync(t => 
                        t.Value == token))
                    .FirstOrDefault()?
                    .UserId;
            if (!userId.HasValue)
            {
                return false;
            }

            var user = await _userManager.GetByIdAsync(userId.Value);
            _currentUserSetter.User = user;
            
            var refreshToken = DecodeRefreshToken(
                await _userManager.GetAuthenticationTokenAsync(
                    user,
                    "AniBand",
                    "RefreshToken"));
            if (!refreshToken.IsActive)
            {
                return false;
            }

            refreshToken.Revoked = DateTime.UtcNow;
                await _userManager.SetAuthenticationTokenAsync(
                    user,
                    "AniBand",
                    "RefreshToken",
                    EncodeRefreshToken(refreshToken));
                
            await _userManager.UpdateAsync(user);

            return true;
        }
    }
}