using System.Collections.Generic;
using AniBand.Domain.Abstractions;

namespace AniBand.Domain.Models
{
    public class Studio
        :BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual List<Season> Seasons { get; set; }
    }
}