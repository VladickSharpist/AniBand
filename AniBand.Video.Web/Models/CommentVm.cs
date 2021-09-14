namespace AniBand.Video.Web.Models
{
    public class CommentVm
    {
        public long Id { get; set; }
        
        public string Text { get; set; }

        public long UserId { get; set; }

        public long VideoId { get; set; }

        public long? ParentCommentId { get; set; }
    }
}