using AniBand.Domain;
using Microsoft.AspNetCore.SignalR;

namespace AniBand.Core.SignalR
{
    public class ActorBasedIdProvider
        : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(CustomClaimTypes.Id)?.Value;
        }
    }
}