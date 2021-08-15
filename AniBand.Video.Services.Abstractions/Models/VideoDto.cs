using Microsoft.AspNetCore.Http;

namespace AniBand.Video.Services.Abstractions.Models
{
    public class VideoDto
    {
        public string Title { get; set; }

        public IFormFile VideoFile { get; set; }

        public string Description { get; set; }

        public long SeasonId { get; set; }
    }
}