using AniBand.Domain.Models;
using AutoMapper;

namespace AniBand.Auth.Services.Abstractions.Models.Mapping
{
    public class RegisterUserProfile : Profile
    {
        public RegisterUserProfile()
        {
            CreateMap<RegisterUserDto, User>();
        }
    }
}