using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AniBand.Domain.Models;

namespace AniBand.Auth.Services.Abstractions.Helpers
{
    public interface IPermissionHelper
    {
        Task GivePermissionsAsync(User user,params string[] permissions);

        Task<List<Claim>> GetPermissionsAsync(User user);
    }
}