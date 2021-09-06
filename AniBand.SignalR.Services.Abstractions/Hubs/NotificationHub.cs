using System;
using System.Linq;
using System.Threading.Tasks;
using AniBand.Domain;
using AniBand.SignalR.Services.Abstractions.Interfaces;
using AniBand.SignalR.Services.Abstractions.Services;
using Microsoft.AspNetCore.SignalR;

namespace AniBand.SignalR.Services.Abstractions.Hubs
{
    public class NotificationHub 
        : Hub<IClient>
    {
        private readonly INotificationService _notificationService;

        public NotificationHub(
            INotificationService notificationService)
        {
            _notificationService = notificationService
                ?? throw new NullReferenceException(nameof(notificationService));
        }

        public override async Task OnConnectedAsync()
        {
            var newNotifications = (await _notificationService
                .GetUnViewedNotificationsAsync(
                    Convert.ToInt64(Context.User?.FindFirst(CustomClaimTypes.Actor)?.Value)))
                .Data
                .ToList()
                .Select(n => n.Message);
        
            await Clients.Caller.GetNewNotificationsAsync(newNotifications);
            
            await base.OnConnectedAsync();
        }
    }
}