using AniBand.Domain.Abstractions.Abstractions;
using AniBand.Domain.Enums;

namespace AniBand.Domain.Models
{
    public class Comment
        : BaseEntity
    {
        public string Text { get; set; }

        public long UserId { get; set; }
        
        public virtual User User { get; set; }

        public long VideoId { get; set; }

        public virtual Video Video { get; set; }

        public long? ParentCommentId { get; set; }

        public Status Status  { get; set; }

        public string DeclineMessage { get; set; }
    }
}