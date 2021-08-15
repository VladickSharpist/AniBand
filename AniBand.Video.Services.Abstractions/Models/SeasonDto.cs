using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace AniBand.Video.Services.Abstractions.Models
{
    public class SeasonDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public IFormFile Image { get; set; }

        public long StudioId { get; set; }

        public List<VideoDto> VideosDto { get; set; }
    }
}