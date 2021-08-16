using System.Collections.Generic;
using AniBand.Domain.Abstractions.Abstractions;

namespace AniBand.Domain.Models
{
    public class Video
        : BaseEntity
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public double DurationInSeconds { get; set; }

        public string Description { get; set; }

        public long SeasonId { get; set; }

        public string VideoFileHash { get; set; }

        public virtual Season Season { get; set; }

        public virtual ICollection<View> Views { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Rate> Rates { get; set; }
    }
}