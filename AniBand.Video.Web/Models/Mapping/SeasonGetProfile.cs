using AniBand.Core.Infrastructure.Helpers.Generic;
using AniBand.Video.Services.Abstractions.Models;
using AniBand.Web.Core.Models.Generic;
using AutoMapper;

namespace AniBand.Video.Web.Models.Mapping
{
    public class SeasonGetProfile : Profile
    {
        public SeasonGetProfile()
        {
            CreateMap<SeasonDto, SeasonGetVm>();
            CreateMap<PagedList<SeasonDto>, PagedVm<SeasonGetVm>>()
                .ForMember(vm => vm.Data,
                    expression => expression.MapFrom(dtos => dtos));
        }
    }
}