using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AniBand.Web.Core.Filters.Permission
{
    public class PermissionHandler 
        : AuthorizationHandler<PermissionRequirement>
    {
        private const string ALL_PERMISSIONS = "*";
        
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            PermissionRequirement requirement)
        {
            var hasAccess = false;
            foreach (var requiredPermission in requirement.Permissions)
            {
                hasAccess = context
                    .User
                    .Claims
                    .Any(c 
                        => c.Value == requiredPermission
                        || HasAllPermissions(c));
            }

            if (hasAccess)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private bool HasAllPermissions(Claim claim)
            => claim.Value == ALL_PERMISSIONS;
    }
}