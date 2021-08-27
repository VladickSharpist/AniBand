using System.Collections.Generic;
using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Models;
using AniBand.Core.Abstractions.Infrastructure.Helpers;

namespace AniBand.Auth.Services.Abstractions.Services
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetUnApprovedUsersAsync();

        Task<IHttpResult> ApproveUser(long id);

        Task<IHttpResult> DeclineUser(long id, string message);
    }
}