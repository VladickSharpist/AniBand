using System.Threading.Tasks;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Video.Services.Abstractions.Models;
using AniBand.Video.Services.Abstractions.Services;
using AniBand.Video.Web.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AniBand.Video.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VideoController 
        : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly IMapper _mapper;
        
        public VideoController(
            IVideoService videoService, 
            IMapper mapper)
        {
            _videoService = videoService;
            _mapper = mapper;
        }
        
        [HttpPost]
        public async Task<ActionResult<IHttpResult>> AddVideo([FromForm] ListVideoVM model)
            =>Ok(await _videoService
                .AddVideoAsync(
                    _mapper.Map<ListVideoDto>(model)
                        .VideosDto));
        

        [HttpPost]
        public async Task<ActionResult<IHttpResult>> AddSeason([FromForm] SeasonVm model)
            => Ok(await _videoService
                .AddSeasonAsync(
                    _mapper.Map<SeasonDto>(model)));
    }
}
