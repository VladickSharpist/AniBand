using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AniBand.Video.Web.Models
{
    public class ListVideoVM
    {
        [Required]
        public List<VideoVm> Videos { get; set; }
        
        [Required]
        public List<IFormFile> Files { get; set; }
    }
}