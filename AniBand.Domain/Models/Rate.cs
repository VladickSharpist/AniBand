using AniBand.Domain.Abstractions;

namespace AniBand.Domain.Models
{
    public class Rate
        :BaseEntity
    {
        public long UserId { get; set; }

        public virtual User User { get; set; }

        public long VideoId { get; set; }

        public virtual Video Video { get; set; }

        public double Value { get; set; }
    }
}