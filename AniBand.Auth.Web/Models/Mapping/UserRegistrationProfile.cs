using AniBand.Auth.Services.Abstractions.Models;
using AutoMapper;

namespace AniBand.Auth.Web.Models.Mapping
{
    public class UserRegistrationProfile : Profile
    {
        public UserRegistrationProfile()
        {
            CreateMap<UserRegistrationVm, RegisterUserDto>();
        }
    }
}