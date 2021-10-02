using System.Collections.Generic;
using AniBand.Domain.Abstractions.Abstractions;

namespace AniBand.Domain.Models
{
    public class Anime
        : BaseEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public long? StudioId { get; set; }

        public virtual Studio Studio { get; set; }

        public virtual ICollection<Episode> Videos { get; set; }
    }
}