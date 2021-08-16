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
using AniBand.Web.Core.Filters.Permission;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace AniBand.Auth.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        private readonly ILogger _logger;

        public AccountController(
            IMapper mapper,
            IAuthService authService, 
            ILogger<AccountController> logger)
        {
            _mapper = mapper
                ?? throw new NullReferenceException(nameof(mapper));
            _authService = authService
                ?? throw new NullReferenceException(nameof(authService));
            _logger = logger
                ?? throw new NullReferenceException(nameof(logger));
        }

        [Permission(Permission.AdminPermission.AddVideo)]
        [HttpPost]
        public string MyName()
        {
            return "MyName";
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