using AniBand.Domain.Models;
using AutoMapper;

namespace AniBand.Video.Services.Abstractions.Models.Mapping
{
    public class SeasonProfile : Profile
    {
        public SeasonProfile()
        {
            CreateMap<SeasonDto, Anime>();
            CreateMap<Anime, SeasonDto>()
                .ForMember(dto => 
                    dto.Videos, opt =>
                        opt.MapFrom(s => s.Videos));
        }
    }
}