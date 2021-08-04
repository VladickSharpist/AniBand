using AniBand.Auth.Services.Abstractions.Models;
using AutoMapper;

namespace AniBand.Auth.Web.Models.Mapping
{
    public class UserLoginProfile : Profile
    {
        public UserLoginProfile()
        {
            CreateMap<UserLoginVm, LoginUserDto>();
        }
    }
}