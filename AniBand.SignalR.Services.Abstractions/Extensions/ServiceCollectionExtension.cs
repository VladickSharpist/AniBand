using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace AniBand.SignalR.Services.Abstractions.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSignalRServiceMapper(this IServiceCollection services)
            => services
                .AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}