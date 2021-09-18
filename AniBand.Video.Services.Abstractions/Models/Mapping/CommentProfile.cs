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
        }
    }
}