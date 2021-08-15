using System.Collections.Generic;
using AniBand.Video.Services.Abstractions.Models;
using AutoMapper;

namespace AniBand.Video.Web.Models.Mapping
{
    public class SeasonProfile : Profile
    {
        public SeasonProfile()
        {
            CreateMap<SeasonVm, SeasonDto>()
                .ForMember(sdt =>
                    sdt.VideosDto, opt =>
                        opt.AllowNull())
                .ForMember(sdt =>
                    sdt.VideosDto, opt =>
                        opt.MapFrom(svm => 
                            MapVideosDto(svm)));
        }

        private List<VideoDto> MapVideosDto(SeasonVm svm)
        {
            if (svm.Videos == null 
                && svm.Files == null)
            {
                return new List<VideoDto>();
            }
            
            if(svm.Videos != null 
               && svm.Files == null
               || svm.Files != null 
               && svm.Videos == null)
            {
                return null;
            }

            if (svm.Files.Count == svm.Videos.Count)
            {
                var videosDtoList = new List<VideoDto>();
                for (var i = 0; i < svm.Videos.Count; i++)
                {
                    videosDtoList.Add(new VideoDto()
                    {
                        Title = svm.Videos[i].Title,
                        Description = svm.Videos[i].Description,
                        SeasonId = svm.Videos[i].SeasonId,
                        VideoFile = svm.Files[i]
                    });
                }
                return videosDtoList;
            }

            return null;
        }
    }
}