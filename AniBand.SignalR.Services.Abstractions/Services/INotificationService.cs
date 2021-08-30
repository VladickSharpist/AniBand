using System.Collections.Generic;
using System.Threading.Tasks;
using AniBand.Core.Abstractions.Infrastructure.Helpers.Generic;
using AniBand.SignalR.Services.Abstractions.Models;

namespace AniBand.SignalR.Services.Abstractions.Services
{
    public interface INotificationService
    {
        Task NotifyAsync(string actorId, string message);
        
        Task<IHttpResult<IEnumerable<NotificationDto>>> GetUnViewedNotificationsAsync(long userId);
    }
}