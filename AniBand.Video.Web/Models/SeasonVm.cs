using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace AniBand.Video.Web.Models
{
    public class SeasonVm
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public IFormFile Image { get; set; }

        public long StudioId { get; set; }

        public List<VideoVm> Videos { get; set; }
        
        public List<IFormFile> Files { get; set; }
    }
}