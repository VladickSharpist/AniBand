using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Models;
using AniBand.Auth.Services.Abstractions.Services;
using AniBand.Auth.Web.Models;
using AniBand.Auth.Web.Permissions;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Core.Abstractions.Infrastructure.Helpers.Generic;
using AniBand.Core.Infrastructure.Helpers;
using AniBand.Core.Infrastructure.Helpers.Generic;
using AniBand.Domain.Models;
using AniBand.SignalR.Services.Abstractions.Services;
using AniBand.Web.Core.Controllers;
using AniBand.Web.Core.Filters.Permission;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace AniBand.Auth.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController 
        : BaseController
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private readonly INotificationService _notificationService;

        public AccountController(
            IMapper mapper,
            IAuthService authService, 
            ILogger<AccountController> logger, 
            IUserService userService, 
            INotificationService notificationService)
            : base(mapper)
        {
            _authService = authService
                ?? throw new NullReferenceException(nameof(authService));
            _logger = logger
                ?? throw new NullReferenceException(nameof(logger));
            _userService = userService
                ?? throw new NullReferenceException(nameof(userService));
            _notificationService = notificationService
                ?? throw new NullReferenceException(nameof(notificationService));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<IHttpResult>> Register(UserRegistrationVm userVm)
            => Ok(await _authService.RegisterAsync(
                _mapper.Map<RegisterUserDto>(userVm)));

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<RefreshTokenVm>> Login([FromBody] UserLoginVm userVm)
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
            return Ok(result.Errors);
        }
        
        [HttpPost]
        public async Task<ActionResult<IHttpResult>> Logout()
        {
            await HttpContext.SignOutAsync();
            return new HttpResult();
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<IHttpResult<RefreshDto>>> Refresh(string refreshToken)
            => Ok(await _authService.RefreshAsync(refreshToken));
        
        [HttpPost]
        public async Task<ActionResult<IHttpResult>> RevokeToken(string token)
            => Ok(await _authService.RevokeAsync(token));

        [Permission(Permission.AdminPermission.GetUsers)]
        [HttpPost]
        public ActionResult<IHttpResult<List<ApproveUserVm>>> GetUnApprovedUsers()
            => Ok(new HttpResult<List<ApproveUserVm>>
            {
                Data = _mapper.Map<List<ApproveUserVm>>(
                    _userService.GetUnApprovedUsers())
            });

        [Permission(Permission.AdminPermission.ApproveUser)]
        [HttpPost]
        public async Task<ActionResult<IHttpResult>> ApproveUser(long id)
            => Ok(await _userService.ApproveUserAsync(id));

        [Permission(Permission.AdminPermission.DeclineUser)]
        [HttpPost]
        public async Task<ActionResult<IHttpResult>> DeclineUser(
            long id, 
            string message)
            => Ok(await _userService.DeclineUserAsync(id, message));
        
        [HttpPost]
        public async Task<ActionResult<IHttpResult<IEnumerable<NotificationVm>>>> GetUnViewedNotifications(long userId)
            => Ok(new HttpResult<IEnumerable<NotificationVm>>(
                _mapper.Map<IEnumerable<NotificationVm>>(
                    await _notificationService.GetUnViewedNotificationsAsync(userId))));
    }
}