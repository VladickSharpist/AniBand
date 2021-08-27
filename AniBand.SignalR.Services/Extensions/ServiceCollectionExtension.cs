using AniBand.Core.SignalR;
using AniBand.SignalR.Services.Abstractions.Services;
using AniBand.SignalR.Services.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace AniBand.SignalR.Services.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSignalRServices(this IServiceCollection services)
            => services
                .AddScoped<INotificationService, NotificationService>();
        
        public static IServiceCollection AddUserIdProviders(this IServiceCollection services)
            => services
                .AddSingleton<IUserIdProvider, ActorBasedIdProvider>();
    }
}