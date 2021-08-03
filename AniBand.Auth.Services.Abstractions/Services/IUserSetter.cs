using AniBand.Domain.Models;

namespace AniBand.Auth.Services.Abstractions.Services
{
    public interface IUserSetter
    {
        void SetUser(User user);
    }
}