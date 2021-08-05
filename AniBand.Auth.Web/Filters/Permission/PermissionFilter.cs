using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AniBand.Auth.Web.Filters.Permission
{
    public class PermissionFilter : Attribute, IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly string[] _permissions;

        public PermissionFilter(
            IAuthorizationService authorizationService, 
            string[] permissions)
        {
            _authorizationService = authorizationService
                ?? throw new NullReferenceException(nameof(authorizationService));
            _permissions = permissions
                ?? throw new NullReferenceException(nameof(permissions));
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var ok = await _authorizationService.AuthorizeAsync(
                context
                    .HttpContext
                    .User, 
                null,
                new PermissionRequirement(_permissions));

            if (!ok.Succeeded)
            {
                context.Result = new ChallengeResult();
            } 
        }
    }
}