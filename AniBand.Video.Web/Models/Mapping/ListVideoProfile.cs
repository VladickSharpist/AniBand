using System.Collections.Generic;
using System.Linq;
using AniBand.Video.Services.Abstractions.Models;
using AutoMapper;

namespace AniBand.Video.Web.Models.Mapping
{
    public class ListVideoProfile : Profile
    {
        public ListVideoProfile()
        {
            CreateMap<ListVideoPostVm, ListVideoDto>()
                .ForMember(lvd => 
                    lvd.VideosDto, opt =>
                        opt.AllowNull())
                .ForMember(lvd => 
                    lvd.VideosDto, opt =>
                        opt.MapFrom(listVideoVm => 
                            MapVideoDto(listVideoVm)));
        }

        private IEnumerable<VideoDto> MapVideoDto(ListVideoPostVm listVideoPostVm)
        {
            if (listVideoPostVm.Files.Count == listVideoPostVm.Videos.Count)
            {
                var videosDtoList = listVideoPostVm
                    .Videos
                    .Select((element, index) => 
                    new VideoDto
                    {
                    Title = element.Title,
                    Description = element.Description, 
                    SeasonId = element.SeasonId,
                    VideoFile = listVideoPostVm.Files[index].OpenReadStream()
                    });
                
                return videosDtoList;
            }

            return null;
        }
    }
}