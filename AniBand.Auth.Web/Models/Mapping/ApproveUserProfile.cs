using AniBand.Auth.Services.Abstractions.Models;
using AutoMapper;

namespace AniBand.Auth.Web.Models.Mapping
{
    public class ApproveUserProfile
        : Profile
    {
        public ApproveUserProfile()
        {
            CreateMap<UserDto, ApproveUserVm>();
        }
    }
}