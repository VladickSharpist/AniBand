using AniBand.Domain.Models;
using AutoMapper;

namespace AniBand.SignalR.Services.Abstractions.Models.Mapping
{
    public class NotificationProfile
        : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto>();
        }
    }
}