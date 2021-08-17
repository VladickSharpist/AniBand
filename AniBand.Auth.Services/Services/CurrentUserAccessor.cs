using AniBand.Auth.Services.Abstractions.Services;
using AniBand.Domain.Models;

namespace AniBand.Auth.Services.Services
{
    internal class CurrentUserAccessor
        : IUserAccessor, 
          IUserSetter
    {
        private User _user = default;

        public User User
        {
            get => _user;
            
            set
            {
                if (value != default)
                {
                    _user = value;
                }
            }
        }
        
    }
}