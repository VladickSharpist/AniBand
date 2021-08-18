using System.Collections.Generic;
using System.Linq;
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
                            MapVideosDto(svm)))
                .ForMember(sdt => 
                    sdt.Image, opt => 
                        opt.MapFrom(svm => 
                            svm.Image.OpenReadStream()));
        }

        private IEnumerable<VideoDto> MapVideosDto(SeasonVm svm)
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
                var videosDtoList = svm
                    .Videos
                    .Select((element, index) => 
                    new VideoDto()
                    {
                    Title = element.Title,
                    Description = element.Description, 
                    SeasonId = element.SeasonId,
                    VideoFile = svm.Files[index].OpenReadStream()
                    });
                
                return videosDtoList;
            }

            return null;
        }
    }
}