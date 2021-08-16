using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Models;
using AniBand.Auth.Services.Abstractions.Services;
using AniBand.Auth.Services.Extensions;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Core.Abstractions.Infrastructure.Helpers.Generic;
using AniBand.Core.Infrastructure.Helpers;
using AniBand.Core.Infrastructure.Helpers.Generic;
using AniBand.Domain;
using AniBand.Domain.Enums;
using AniBand.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace AniBand.Auth.Services.Services
{
    internal class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole<long>> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserSetter _currentUserSetter;

        public AuthService(
            IMapper mapper,
            ITokenService tokenService,
            RoleManager<IdentityRole<long>> roleManager,
            UserManager<User> userManager, 
            IUserSetter currentUserSetter)
        {
            _mapper = mapper 
                ?? throw new NullReferenceException(nameof(mapper));
            _tokenService = tokenService 
                ?? throw new NullReferenceException(nameof(tokenService));
            _roleManager = roleManager
                ?? throw new NullReferenceException(nameof(roleManager));
            _userManager = userManager
                ?? throw new NullReferenceException(nameof(userManager));
            _currentUserSetter = currentUserSetter
                ?? throw new NullReferenceException(nameof(currentUserSetter));
        }

        public async Task<IHttpResult> RegisterAsync(RegisterUserDto model)
        {
            var user = _mapper.Map<User>(model);
            if (_userManager
                .Users
                .All(u => 
                    u.Email != model.Email))
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return new HttpResult(result.Errors
                            .Select(e => e.Description)
                            .ToList(),
                        HttpStatusCode.UnprocessableEntity);
                }

                await _userManager.AddToRoleAsync(
                    user, 
                    Roles.User.ToString());
                
                var userRole = await _roleManager
                    .FindByNameAsync(Roles.User.ToString());
                
                var userPermissions = (await _roleManager.GetClaimsAsync(userRole))
                    .Where(c => 
                        c.Type == CustomClaimTypes.Permission)
                    .ToList();
                await _userManager.AddClaimsAsync(user, userPermissions);

                return new HttpResult();
            }

            return new HttpResult(
                "User with this Email already exists",
                HttpStatusCode.UnprocessableEntity);
        }

        public async Task<IHttpResult<AuthDto>> AuthenticateAsync(LoginUserDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new HttpResult<AuthDto>(
                    null,
                    "User doesnt exist",
                    HttpStatusCode.UnprocessableEntity);
            }

            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                _currentUserSetter.User = user;
                
                var refreshToken = _tokenService.GenerateRefreshToken(user);
                await _userManager.SetAuthenticationTokenAsync(
                    user, 
                    "AniBand", 
                    "RefreshToken", 
                    refreshToken);

                var claims = await _userManager.GetClaimsAsync(user);
                claims.Add(new Claim (
                    CustomClaimTypes.Actor, 
                    user.Email));
                
                var identity = new ClaimsIdentity(
                    claims,
                    JwtBearerDefaults.AuthenticationScheme);

                return new HttpResult<AuthDto>
                {
                    Data = new AuthDto
                    {
                        RefreshToken = refreshToken, 
                        ClaimsPrincipal = new ClaimsPrincipal(identity)
                    }
                };
            }

            return new HttpResult<AuthDto>(
                null,
                "Wrong password",
                HttpStatusCode.UnprocessableEntity);
        }

        public async Task<IHttpResult<RefreshDto>> RefreshAsync(string refreshToken)
        {
            var refreshTokenObject = _tokenService.DecodeRefreshToken(refreshToken);
            var user = await _userManager.GetByIdAsync(refreshTokenObject.UserId);
            if (user == null)
            {
                return new HttpResult<RefreshDto>(
                    null,
                    "User wasn`t found",
                    HttpStatusCode.UnprocessableEntity);
            }

            _currentUserSetter.User = user;
            
            var activeRefreshToken = await _userManager.GetAuthenticationTokenAsync(
                user, 
                "AniBand", 
                "RefreshToken");
            if (activeRefreshToken != refreshToken)
            {
                return new HttpResult<RefreshDto>(
                    null,
                    "Invalid refresh token",
                    HttpStatusCode.UnprocessableEntity);
            }

            refreshTokenObject.Revoked = DateTime.Now;
            _tokenService.MoveToHistory(user, refreshTokenObject);

            var newJwtToken = await _tokenService.GenerateTokenAsync(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken(user);
            await _userManager.SetAuthenticationTokenAsync(
                user, 
                "AniBand", 
                "RefreshToken", 
                newRefreshToken);

            return new HttpResult<RefreshDto>(new RefreshDto
            {
                Token = newJwtToken, 
                RefreshToken = newRefreshToken
            });
        }

        public async Task<IHttpResult> RevokeAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new HttpResult(
                    "Token is required", 
                    HttpStatusCode.BadRequest);
            }

            var response = await _tokenService.RevokeTokenAsync(token);

            if (!response)
            {
                return new HttpResult(
                    "Token not found", 
                    HttpStatusCode.BadRequest);
            }

            return new HttpResult();
        }
    }
}