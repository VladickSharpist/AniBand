using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AniBand.Auth.Web.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddWebAuthMapper(this IServiceCollection services)
            => services
                .AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}