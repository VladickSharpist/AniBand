using AniBand.Video.Services.Abstractions.Models;
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
            CreateMap<WaitingCommentDto, WaitingCommentVm>();
        }
    }
}