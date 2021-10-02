using AniBand.Domain.Abstractions.Abstractions;

namespace AniBand.Domain.Models
{
    public class View
        : BaseEntity     
    {
        public long UserId { get; set; }

        public virtual User User { get; set; }

        public long VideoId { get; set; }

        public virtual Episode Episode { get; set; }
    }
}