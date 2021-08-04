using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AniBand.Auth.Services.Abstractions.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServiceMapper(this IServiceCollection services)
            => services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}