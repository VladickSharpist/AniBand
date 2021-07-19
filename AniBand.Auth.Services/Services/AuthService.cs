using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Helpers;
using AniBand.Auth.Services.Abstractions.Models;
using AniBand.Auth.Services.Abstractions.Permissions;
using AniBand.Auth.Services.Abstractions.Services;
using AniBand.Auth.Services.Helpers;
using AniBand.Domain.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AniBand.Auth.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IPermissionHelper _permissionHelper;
        private readonly ITokenService _tokenService;

        public AuthService(
            IMapper mapper,
            UserManager<User> userManager,
            IPermissionHelper permissionHelper,
            ITokenService tokenService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _permissionHelper = permissionHelper;
            _tokenService = tokenService;
        }

        public async Task<IHttpResult> Register(RegisterUserDto model)
        {
            var user = _mapper.Map<User>(model);
            if (_userManager.Users.All(u => u.Email != model.Email))
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return new HttpResult(result.Errors.Select(e => e.Description).ToList(), 422);
                }

                await _userManager.AddToRoleAsync(user, "User");
                await _permissionHelper.GivePermissionsAsync(user,
                    UserPermissions.CommentVideo, UserPermissions.WatchVideo);
                return new HttpResult();
            }

            return new HttpResult("User with this Email already exists", 422);
        }

        public async Task<IHttpResult> Authenticate(LoginUserDto model, HttpContext context)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new HttpResult("User doesnt exist", 422);
            }

            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var refreshToken = _tokenService.GenerateRefreshToken(user);
                await _userManager.SetAuthenticationTokenAsync(
                    user, "AniBand", "RefreshTokenDTO", refreshToken);

                var claims = await _userManager.GetClaimsAsync(user);
                var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                await context.SignInAsync(new ClaimsPrincipal(identity));

                return new HttpResult<string>(refreshToken);
            }

            return new HttpResult("Wrong password", 422);
        }

        public async Task<IHttpResult> Refresh(string refreshToken)
        {
            var refreshTokenObject = _tokenService.DecodeRefreshToken(refreshToken);
            var user = await _userManager.FindByIdAsync(refreshTokenObject.UserId);
            if (user == null)
            {
                return new HttpResult("User wasn`t found", 422);
            }

            var activeRefreshToken = await _userManager.GetAuthenticationTokenAsync(
                user, "AniBand", "RefreshToken");
            if (activeRefreshToken != refreshToken)
            {
                return new HttpResult("Invalid refresh token", 422);
            }

            refreshTokenObject.Revoked = DateTime.Now;
            _tokenService.MoveToHistory(user, refreshTokenObject);

            var newJwtToken = await _tokenService.GenerateTokenAsync(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken(user);
            await _userManager.SetAuthenticationTokenAsync(
                user, "AniBand", "RefreshToken", newRefreshToken);
            return new HttpResult<ObjectResult>(new ObjectResult(new
            {
                token = newJwtToken, refreshToken = newRefreshToken
            }));
        }

        public async Task<IHttpResult> Revoke(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return new HttpResult("Token is required",(int)HttpStatusCode.BadRequest);
            }

            var response = await _tokenService.RevokeToken(token);

            if (!response)
            {
                return new HttpResult("Token not found",(int)HttpStatusCode.BadRequest);
            }

            return new HttpResult();
        }
    }
}