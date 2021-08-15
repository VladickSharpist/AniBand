using AutoMapper;

namespace AniBand.Video.Services.Abstractions.Models.Mapping
{
    public class VideoProfile : Profile
    {
        public VideoProfile()
        {
            CreateMap<VideoDto, Domain.Models.Video>();
        }
    }
}