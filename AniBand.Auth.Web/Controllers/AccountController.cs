using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Models;
using AniBand.Auth.Services.Abstractions.Services;
using AniBand.Auth.Web.Models;
using AniBand.Auth.Web.Permissions;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Core.Abstractions.Infrastructure.Helpers.Generic;
using AniBand.Core.Infrastructure.Helpers;
using AniBand.Web.Core;
using AniBand.Web.Core.Controllers;
using AniBand.Web.Core.Filters.Permission;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace AniBand.Auth.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController 
        : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ILogger _logger;

        public AccountController(
            IMapper mapper,
            IAuthService authService, 
            ILogger<AccountController> logger)
            : base(mapper)
        {
            _authService = authService
                ?? throw new NullReferenceException(nameof(authService));
            _logger = logger
                ?? throw new NullReferenceException(nameof(logger));
        }

        [HttpPost]
        public async Task<ActionResult<IHttpResult>> Register(UserRegistrationVm userVm)
            => Ok(await _authService.RegisterAsync(
                _mapper.Map<RegisterUserDto>(userVm)));


        [HttpPost]
        public async Task<ActionResult<RefreshTokenVm>> Login(UserLoginVm userVm)
        {
            var result = await _authService.AuthenticateAsync(
                _mapper.Map<LoginUserDto>(userVm));
            if (!result.IsEmpty)
            {
                await HttpContext.SignInAsync(result.Data.ClaimsPrincipal);
                _logger.Log(LogLevel.Information, "User login in");
                return _mapper.Map<RefreshTokenVm>(result.Data);
            }
            _logger.Log(LogLevel.Information, "User failed login");
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<IHttpResult>> Logout()
        {
            await HttpContext.SignOutAsync();
            return new HttpResult();
        }
        
        [HttpPost]
        public async Task<ActionResult<IHttpResult<RefreshDto>>> Refresh(string refreshToken)
            => Ok(await _authService.RefreshAsync(refreshToken));

        [HttpPost]
        public async Task<ActionResult<IHttpResult>> RevokeToken(string token)
            => Ok(await _authService.RevokeAsync(token));
    }
}