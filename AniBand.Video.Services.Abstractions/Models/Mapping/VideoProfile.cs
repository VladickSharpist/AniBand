using AniBand.Domain.Models;
using AutoMapper;

namespace AniBand.Video.Services.Abstractions.Models.Mapping
{
    public class VideoProfile : Profile
    {
        public VideoProfile()
        {
            CreateMap<VideoDto, Episode>();
            CreateMap<Episode, VideoDto>()
                .ForMember(e => 
                        e.VideoFileUrl,
                    opt => 
                        opt.MapFrom(dto => 
                            dto.Url));
        }
    }
}