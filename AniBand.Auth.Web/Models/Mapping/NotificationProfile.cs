using AniBand.SignalR.Services.Abstractions.Models;
using AutoMapper;

namespace AniBand.Auth.Web.Models.Mapping
{
    public class NotificationProfile
        : Profile
    {
        public NotificationProfile()
        {
            CreateMap<NotificationDto, NotificationVm>();
        }
    }
}