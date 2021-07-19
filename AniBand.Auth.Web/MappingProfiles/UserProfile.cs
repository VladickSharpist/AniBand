using AniBand.Auth.Services.Abstractions.Models;
using AniBand.Auth.Web.Models;
using AniBand.Domain.Models;
using AutoMapper;

namespace AniBand.Auth.Web.MappingProfiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegistrationViewModel, RegisterUserDto>();
            CreateMap<RegisterUserDto, User>();
        }
    }
}