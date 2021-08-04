using AniBand.Domain.Models;

namespace AniBand.Auth.Services.Abstractions.Services
{
    public interface IUserAccessor
    {
        User User { get; }
    }
}