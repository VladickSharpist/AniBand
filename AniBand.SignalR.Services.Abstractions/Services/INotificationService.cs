using System.Threading.Tasks;

namespace AniBand.SignalR.Services.Abstractions.Services
{
    public interface INotificationService
    {
        Task NotifyAsync(string actor, string message);
    }
}