using AniBand.Auth.Services.Abstractions.Helpers;
using AniBand.Auth.Services.Abstractions.Services;
using AniBand.Auth.Services.Helpers;
using AniBand.Auth.Services.Services;
using AniBand.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AniBand.Auth.Services.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddUser(this IServiceCollection services)
            => services
                .AddScoped<UserManager<User>>()
                .AddScoped<CurrentUserAccessor>()
                .AddScoped<IUserAccessor>(di 
                    => di.GetRequiredService<CurrentUserAccessor>())
                .AddScoped<IUserSetter>(di 
                    => di.GetRequiredService<CurrentUserAccessor>());

        public static IServiceCollection AddAuth(this IServiceCollection services)
            => services
                .AddScoped<IAuthService, AuthService>();

        public static IServiceCollection AddHelpers(this IServiceCollection services)
            => services.AddScoped<IConfigurationHelper, ConfigurationHelper>();
    }
}