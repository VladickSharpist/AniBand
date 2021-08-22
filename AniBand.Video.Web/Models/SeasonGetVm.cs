using System.Collections.Generic;

namespace AniBand.Video.Web.Models
{
    public class SeasonGetVm
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public long StudioId { get; set; }

        public List<VideoGetVm> Videos { get; set; }
    }
}