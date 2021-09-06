namespace AniBand.Video.Services.Abstractions.Models
{
    public class WaitingCommentDto
    {
        public VideoDto Video { get; set; }

        public CommentDto Comment { get; set; }

        public string UserEmail { get; set; }
    }
}