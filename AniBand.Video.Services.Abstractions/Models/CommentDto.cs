namespace AniBand.Video.Services.Abstractions.Models
{
    public class CommentDto
    {
        public string Text { get; set; }

        public long UserId { get; set; }

        public long VideoId { get; set; }

        public long? ParentCommentId { get; set; }
    }
}