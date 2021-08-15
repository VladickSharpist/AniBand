using System.Collections.Generic;
using AniBand.Video.Services.Abstractions.Models;
using AutoMapper;

namespace AniBand.Video.Web.Models.Mapping
{
    public class ListVideoProfile : Profile
    {
        public ListVideoProfile()
        {
            CreateMap<ListVideoVM, ListVideoDto>()
                .ForMember(lvd => 
                    lvd.VideosDto, opt =>
                        opt.AllowNull())
                .ForMember(lvd => 
                    lvd.VideosDto, opt =>
                        opt.MapFrom(listVideoVm => 
                            MapVideoDto(listVideoVm)));
        }

        private List<VideoDto> MapVideoDto(ListVideoVM listVideoVm)
        {
            if (listVideoVm.Files.Count == listVideoVm.Videos.Count)
            {
                var list = new List<VideoDto>();
                for (var  i = 0; i < listVideoVm.Videos.Count; i++)
                {
                    list.Add(new VideoDto()
                    {
                        Description = listVideoVm.Videos[i].Description,
                        Title = listVideoVm.Videos[i].Title,
                        SeasonId = listVideoVm.Videos[i].SeasonId,
                        VideoFile = listVideoVm.Files[i]
                    });
                }

                return list;
            }

            return null;
        }
    }
}