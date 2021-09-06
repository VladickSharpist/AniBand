namespace AniBand.Video.Web.Models
{
    public class WaitingCommentVm
    {
        public VideoGetVm Video { get; set; }

        public CommentVm Comment { get; set; }

        public string UserEmail { get; set; }
    }
}