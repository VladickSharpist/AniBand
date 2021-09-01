using AniBand.SignalR.Services.Abstractions.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace AniBand.Web.Core.Extensions
{
    public static class RouteBuilderExtension
    {
        public static IEndpointRouteBuilder MapHubs(this IEndpointRouteBuilder builder)
        {
            builder.MapHub<NotificationHub>("/hubs/notifications");
            return builder;
        }
    }
}