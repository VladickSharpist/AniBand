using AniBand.Domain.Models;

namespace AniBand.Auth.Services.Abstractions.Services
{
    public interface IUserSetter
    {
        User User { set; }
    }
}