using AniBand.Auth.Services.Abstractions.Models;
using AutoMapper;

namespace AniBand.Auth.Web.Models.Mapping
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<AuthDto, RefreshTokenVm>();
        }
    }
}