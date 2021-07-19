using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Helpers;
using AniBand.Auth.Services.Abstractions.Models;
using AniBand.Auth.Services.Abstractions.Services;
using AniBand.Auth.Services.Helpers;
using AniBand.Auth.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace AniBand.Auth.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public AccountController(
            IMapper mapper,
            IAuthService authService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(
                typeof(IMapper).ToString());
            _authService = authService ?? throw new ArgumentNullException(
                typeof(IAuthService).ToString());;
        }

        [Authorize]
        [HttpPost]
        public string MyName()
        {
            return "MyName";
        }

        [HttpPost]
        public async Task<IHttpResult> Register(UserRegistrationViewModel userViewModel)
        {
            var registerModel = _mapper.Map<RegisterUserDto>(userViewModel);
            var result = await _authService.Register(registerModel);
            return result;
        }

        [HttpPost]
        public async Task<IHttpResult> Login(UserLoginViewModel userViewModel)
        {
            var model = _mapper.Map<LoginUserDto>(userViewModel);
            var result = await _authService.Authenticate(model,HttpContext);
            return result;
        }

        [HttpPost]
        public async Task<IHttpResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return new HttpResult();
        }

        [HttpPost]
        public async Task<IHttpResult> Refresh(string refreshToken)
        {
            var result = await _authService.Refresh(refreshToken);
            return result;
        }

        [HttpPost]
        public async Task<IHttpResult> RevokeToken(string token)
        {
            var result = await _authService.Revoke(token);
            return result;
        }
    }
}