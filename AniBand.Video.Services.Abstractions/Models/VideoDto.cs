using System.Collections.Generic;
using System.IO;

namespace AniBand.Video.Services.Abstractions.Models
{
    public class VideoDto
    {
        public long Id { get; set; }
        
        public string Title { get; set; }

        public Stream VideoFile { get; set; }
        
        public string VideoFileUrl { get; set; }

        public string Description { get; set; }

        public long SeasonId { get; set; }
        
        public virtual ICollection<CommentDto> Comments { get; set; }
    }
}