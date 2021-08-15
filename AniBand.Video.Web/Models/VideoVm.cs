using System.ComponentModel.DataAnnotations;

namespace AniBand.Video.Web.Models
{
    public class VideoVm
    {
        public string Title { get; set; }

        public string Description { get; set; }
        
        [Required]
        public long SeasonId { get; set; }
    }
}