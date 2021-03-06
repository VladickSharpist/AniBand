using AniBand.Domain.Abstractions;
using AniBand.Domain.Abstractions.Abstractions;

namespace AniBand.Domain.Models
{
    public class RefreshToken
        : BaseEntity
    {
        public string Token { get; set; }

        public virtual User Owner { get; set; }

        public long OwnerId { get; set; }
    }
}
