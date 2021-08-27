using AniBand.Domain.Models;
using AutoMapper;

namespace AniBand.Auth.Services.Abstractions.Models.Mapping
{
    public class UserProfile
        : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}