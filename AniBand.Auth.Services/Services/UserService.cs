using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Models;
using AniBand.Auth.Services.Abstractions.Services;
using AniBand.Auth.Services.Extensions;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Core.Infrastructure.Helpers;
using AniBand.Domain.Enums;
using AniBand.Domain.Models;
using AniBand.SignalR.Services.Abstractions.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AniBand.Auth.Services.Services
{
    public class UserService
        : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<long>> _roleManager;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;
        private readonly ITokenService _tokenService;

        public UserService(
            UserManager<User> userManager,
            IMapper mapper, 
            INotificationService notificationService, 
            ITokenService tokenService, 
            RoleManager<IdentityRole<long>> roleManager)
        {
            _userManager = userManager
                ?? throw new NullReferenceException(nameof(userManager));
            _mapper = mapper 
                ?? throw new NullReferenceException(nameof(mapper));
            _notificationService = notificationService
                ?? throw new NullReferenceException(nameof(notificationService));
            _tokenService = tokenService
                ?? throw new NullReferenceException(nameof(tokenService));
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<UserDto>> GetUnApprovedUsers()
        {
            var unApprovedUsers = await _userManager
                .GetUsersByField(u => 
                    u.Status == Status.Waiting);

            return _mapper.Map<IEnumerable<UserDto>>(unApprovedUsers);
        }

        public async Task<IHttpResult> ApproveUserAsync(long id)
        {
            var user = await _userManager.GetByIdAsync(id);
            
            try
            {
                await _userManager.ApproveUserAsync(id);
                var approvedRole = await _roleManager
                    .FindByNameAsync(Roles.ApprovedUser.ToString());
                await _userManager
                    .AddClaimsAsync(
                        user,
                        await _roleManager.GetClaimsAsync(approvedRole));
            }
            catch (Exception e)
            {
                return new HttpResult(
                    e.Message, 
                    HttpStatusCode.UnprocessableEntity);
            }
            
            await _notificationService.NotifyAsync(
                user.Id.ToString(), 
                "Your account approved");
            return new HttpResult();
        }
        

        public async Task<IHttpResult> DeclineUserAsync(long id, string message)
        {
            try
            {
                await _userManager.DeclineUserAsync(id, message);
            }
            catch (Exception e)
            {
                return new HttpResult(
                    e.Message, 
                    HttpStatusCode.UnprocessableEntity);
            }

            var user = await _userManager.GetByIdAsync(id);
            var userToken = await _userManager.GetAuthenticationTokenAsync(
                user,
                "AniBand",
                "RefreshToken");

            await _tokenService.RevokeTokenAsync(userToken);
            return new HttpResult();
        }
    }
}