using AniBand.Domain.Models;
using AutoMapper;

namespace AniBand.Video.Services.Abstractions.Models.Mapping
{
    public class SeasonProfile : Profile
    {
        public SeasonProfile()
        {
            CreateMap<SeasonDto, Season>();
            CreateMap<Season, SeasonDto>()
                .ForMember(dto => 
                    dto.VideosDto, opt =>
                        opt.MapFrom(s => s.Videos));
        }
    }
}