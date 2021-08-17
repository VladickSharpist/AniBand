using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AniBand.Auth.Services.Abstractions.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddAuthServiceMapper(this IServiceCollection services)
            => services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}