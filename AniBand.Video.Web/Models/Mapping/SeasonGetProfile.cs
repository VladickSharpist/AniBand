using AniBand.Video.Services.Abstractions.Models;
using AutoMapper;

namespace AniBand.Video.Web.Models.Mapping
{
    public class SeasonGetProfile : Profile
    {
        public SeasonGetProfile()
        {
            CreateMap<SeasonDto, SeasonGetVm>();
            CreateMap<SeasonDto, SeasonGetNoVideoVm>();
        }
    }
}