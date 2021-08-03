using AniBand.Auth.Services.Abstractions.Services;
using AniBand.Domain.Models;

namespace AniBand.Auth.Services.Services
{
    public class CurrentUserAccessor
        : IUserAccessor,
            IUserSetter
    {
        private User _user;

        public User GetUser()
        {
            return _user;
        }

        public void SetUser(User user)
        {
            _user = user;
        }
    }
}