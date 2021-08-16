using AutoMapper;

using Episode = AniBand.Domain.Models.Video;

namespace AniBand.Video.Services.Abstractions.Models.Mapping
{
    public class VideoProfile : Profile
    {
        public VideoProfile()
        {
            CreateMap<VideoDto, Episode>();
        }
    }
}