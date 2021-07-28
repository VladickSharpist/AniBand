using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Helpers;
using AniBand.Auth.Services.Abstractions.Models;
using AniBand.Auth.Services.Abstractions.Services;
using AniBand.Auth.Services.Helpers;
using AniBand.Domain;
using AniBand.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace AniBand.Auth.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole<long>> _roleManager;
        private readonly IUserService _userService;

        public AuthService(
            IMapper mapper,
            ITokenService tokenService,
            RoleManager<IdentityRole<long>> roleManager,
            IUserService userService)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _userService = userService;
        }

        public async Task<IHttpResult> Register(RegisterUserDto model)
        {
            var user = _mapper.Map<User>(model);
            if (_userService.Users.All(u => u.Email != model.Email))
            {
                var result = await _userService.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return new HttpResult(result.Errors
                            .Select(e => e.Description)
                            .ToList(),
                        HttpStatusCode.UnprocessableEntity);
                }

                await _userService.AddToRoleAsync(user, Roles.User.ToString());
                var userRole = await _roleManager
                    .FindByNameAsync(Roles.User.ToString());
                var userPermissions = (await _roleManager.GetClaimsAsync(userRole))
                    .Where(c => c.Type == CustomClaimTypes.Permission)
                    .ToList();
                await _userService.AddClaimsAsync(user, userPermissions);

                return new HttpResult();
            }

            return new HttpResult(
                "User with this Email already exists",
                HttpStatusCode.UnprocessableEntity);
        }

        public async Task<IHttpResult<AuthDto>> Authenticate(LoginUserDto model)
        {
            var user = await _userService.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new HttpResult<AuthDto>(
                    null,
                    "User doesnt exist",
                    HttpStatusCode.UnprocessableEntity);
            }

            if (await _userService.CheckPasswordAsync(user, model.Password))
            {
                var refreshToken = _tokenService.GenerateRefreshToken(user);
                await _userService.SetAuthenticationTokenAsync(
                    user, "AniBand", "RefreshToken", refreshToken);

                var claims = await _userService.GetClaimsAsync(user);
                var identity = new ClaimsIdentity(
                    claims,
                    JwtBearerDefaults.AuthenticationScheme);

                return new HttpResult<AuthDto>
                {
                    Data = new AuthDto
                    {
                        RefreshToken = refreshToken, ClaimsPrincipal = new ClaimsPrincipal(identity)
                    }
                };
            }

            return new HttpResult<AuthDto>(
                null,
                "Wrong password",
                HttpStatusCode.UnprocessableEntity);
        }

        public async Task<IHttpResult<RefreshDto>> Refresh(string refreshToken)
        {
            var refreshTokenObject = _tokenService.DecodeRefreshToken(refreshToken);
            var user = await _userService.FindByIdAsync(refreshTokenObject.UserId);
            if (user == null)
            {
                return new HttpResult<RefreshDto>(
                    null,
                    "User wasn`t found",
                    HttpStatusCode.UnprocessableEntity);
            }

            var activeRefreshToken = await _userService.GetAuthenticationTokenAsync(
                user, "AniBand", "RefreshToken");
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
            await _userService.SetAuthenticationTokenAsync(
                user, "AniBand", "RefreshToken", newRefreshToken);
            return new HttpResult<RefreshDto>(new RefreshDto
            {
                Token = newJwtToken, RefreshToken = newRefreshToken
            });
        }

        public async Task<IHttpResult> Revoke(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new HttpResult("Token is required", HttpStatusCode.BadRequest);
            }

            var response = await _tokenService.RevokeToken(token);

            if (!response)
            {
                return new HttpResult("Token not found", HttpStatusCode.BadRequest);
            }

            return new HttpResult();
        }
    }
}