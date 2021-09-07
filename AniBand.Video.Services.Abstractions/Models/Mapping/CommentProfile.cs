using AniBand.Domain.Models;
using AutoMapper;

namespace AniBand.Video.Services.Abstractions.Models.Mapping
{
    public class CommentProfile
        : Profile
    {
        public CommentProfile()
        {
            CreateMap<CommentDto, Comment>();
            CreateMap<Comment, CommentDto>();
            CreateMap<Comment, WaitingCommentDto>()
                .ForMember(dto => dto.Comment, opt =>
                    opt.MapFrom(comment => comment))
                .ForMember(dto => dto.Video, opt =>
                    opt.MapFrom(comment => comment.Video))
                .ForMember(dto => dto.UserEmail, opt =>
                    opt.MapFrom(comment => comment.User.Email));
        }
    }
}