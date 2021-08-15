using Microsoft.AspNetCore.Authorization;

namespace AniBand.Auth.Web.Filters.Permission
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