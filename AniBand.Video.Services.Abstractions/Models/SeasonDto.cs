using System.Collections.Generic;
using System.IO;

namespace AniBand.Video.Services.Abstractions.Models
{
    public class SeasonDto
    {
        public long Id { get; set; }
        
        public string Title { get; set; }

        public string Description { get; set; }

        public Stream Image { get; set; }
        
        public string ImageUrl { get; set; }

        public long StudioId { get; set; }

        public IEnumerable<VideoDto> Videos { get; set; }
    }
}