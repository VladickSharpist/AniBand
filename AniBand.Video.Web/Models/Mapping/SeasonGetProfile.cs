using AniBand.Video.Services.Abstractions.Models;
using AutoMapper;

namespace AniBand.Video.Web.Models.Mapping
{
    public class SeasonGetProfile : Profile
    {
        public SeasonGetProfile()
        {
            CreateMap<SeasonDto, SeasonGetVm>()
                .ForMember(vm => 
                    vm.Videos, opt => 
                        opt.MapFrom(dto => 
                            dto.VideosDto));
        }
    }
}