using AniBand.SignalR.Services.Abstractions.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace AniBand.SignalR.Services.Abstractions.Hubs
{
    public class NotificationHub 
        : Hub<IClient>
    {
    }
}