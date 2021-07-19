using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Helpers;
using AniBand.Domain;
using AniBand.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace AniBand.Auth.Services.Helpers
{
    public class PermissionHelper:IPermissionHelper
    {
        private readonly UserManager<User> _userManager;

        public PermissionHelper(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task GivePermissionsAsync(User user, params string[] permissions)
        {
            foreach (var permission in permissions)
            {
                await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, permission));
            }
        }
        
        public async Task<List<Claim>> GetPermissionsAsync(User user)
        {
            var permissions = (await _userManager.GetClaimsAsync(user))
                .Where(c => c.Type == CustomClaimTypes.Permission)
                .ToList();
            return permissions;
        }
        
    }
}