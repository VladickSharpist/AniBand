using AniBand.Domain.Abstractions;

namespace AniBand.Domain.Models
{
    public class RefreshToken
        : BaseEntity
    {
        public string Token { get; set; }

        public User Owner { get; set; }

        public long OwnerId { get; set; }
    }
}
