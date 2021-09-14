using AniBand.Core.Infrastructure.Helpers.Generic;
using AniBand.Video.Services.Abstractions.Models;
using AniBand.Web.Core.Models.Generic;
using AutoMapper;

namespace AniBand.Video.Web.Models.Mapping
{
    public class CommentProfile
        : Profile
    {
        public CommentProfile()
        {
            CreateMap<CommentVm, CommentDto>();
            CreateMap<CommentDto, CommentVm>();
            CreateMap<PagedList<CommentDto>, PagedVm<CommentVm>>()
                .ForMember(vm => vm.Data, expression => 
                    expression.MapFrom(dtos => dtos));
        }
    }
}