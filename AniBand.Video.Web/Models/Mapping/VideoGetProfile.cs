using AniBand.Core.Infrastructure.Helpers.Generic;
using AniBand.Video.Services.Abstractions.Models;
using AniBand.Web.Core.Models.Generic;
using AutoMapper;

namespace AniBand.Video.Web.Models.Mapping
{
    public class VideoGetProfile : Profile
    {
        public VideoGetProfile()
        {
            CreateMap<VideoDto, VideoGetVm>();
            CreateMap<PagedList<VideoDto>, PagedVm<VideoGetVm>>()
                .ForMember(vm => vm.Data, expression => 
                    expression.MapFrom(dtos => dtos));
        }
    }
}