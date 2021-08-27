using System.Threading.Tasks;

namespace AniBand.SignalR.Services.Abstractions.Interfaces
{
    public interface IClient
    {
        Task SendNotificationAsync(string message);
    }
}