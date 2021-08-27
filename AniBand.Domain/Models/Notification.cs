using AniBand.Domain.Abstractions.Abstractions;

namespace AniBand.Domain.Models
{
    public class Notification
        : BaseEntity
    {
        public long UserId { get; set; }

        public virtual User User { get; set; }

        public string Message { get; set; }
    }
}