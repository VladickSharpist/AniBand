using AniBand.Video.Services.Abstractions.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AniBand.Video.Web.Models.Mapping
{
    public class VideoGetProfile : Profile
    {
        public VideoGetProfile()
        {
            CreateMap<VideoDto, VideoGetVm>();
        }
    }
}