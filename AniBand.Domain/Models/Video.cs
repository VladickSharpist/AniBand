using System.Collections.Generic;
using AniBand.Domain.Abstractions;

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

        public virtual Season Season { get; set; }

        public virtual List<View> Views { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Rate> Rates { get; set; }
    }
}