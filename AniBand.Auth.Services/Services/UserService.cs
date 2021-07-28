using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AniBand.Auth.Services.Abstractions.Services;
using AniBand.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AniBand.Auth.Services.Services
{
    public class UserService
        : UserManager<User>
            , IUserService
    {
        public UserService(
            IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services, ILogger<UserManager<User>> logger)
            : base(store, optionsAccessor, passwordHasher,
                userValidators, passwordValidators, keyNormalizer,
                errors, services, logger)
        {
            
        }

        public User FindById(long id)
        {
            return Users.FirstOrDefault(u => u.Id == id);
        }

        public async Task<User> FindByIdAsync(long id)
        {
            return await Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}