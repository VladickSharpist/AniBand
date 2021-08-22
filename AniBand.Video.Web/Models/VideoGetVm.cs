using Microsoft.AspNetCore.Mvc;

namespace AniBand.Video.Web.Models
{
    public class VideoGetVm
    {
        public string Title { get; set; }

        public string VideoFileUrl { get; set; }

        public string Description { get; set; }

        public long SeasonId { get; set; }
    }
}