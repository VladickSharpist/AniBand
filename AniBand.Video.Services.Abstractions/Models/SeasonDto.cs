using System.Collections.Generic;
using System.IO;

namespace AniBand.Video.Services.Abstractions.Models
{
    public class SeasonDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public Stream Image { get; set; }
        
        public string ImageUrl { get; set; }

        public long StudioId { get; set; }

        public List<VideoDto> VideosDto { get; set; }
    }
}