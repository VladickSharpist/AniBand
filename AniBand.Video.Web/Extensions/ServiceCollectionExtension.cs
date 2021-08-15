using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AniBand.Video.Web.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddWebVideoMapper(this IServiceCollection services)
            => services
                .AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}