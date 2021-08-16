using Microsoft.AspNetCore.Authorization;

namespace AniBand.Web.Core.Filters.Permission
{
    public class PermissionRequirement 
        : IAuthorizationRequirement
    {
        public string[] Permissions { get; set; }

        public PermissionRequirement(string[] permissions)
        {
            Permissions = permissions;
        }
    }
}