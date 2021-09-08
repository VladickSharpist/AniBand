using System.Collections.Generic;
using System.Threading.Tasks;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Core.Abstractions.Infrastructure.Helpers.Generic;
using AniBand.Video.Services.Abstractions.Models;
using AniBand.Video.Services.Abstractions.Services;
using AniBand.Video.Web.Models;
using AniBand.Web.Core.Controllers;
using AniBand.Web.Core.Filters.Permission;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AniBand.Video.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VideoController 
        : BaseController
    {
        private readonly IVideoService _videoService;

        public VideoController(
            IVideoService videoService,
            IMapper mapper)
            : base(mapper)
        {
            _videoService = videoService;
        }
        
        [Permission(Permissions.Permission.AdminPermission.AddVideo)]
        [HttpPost]
        public async Task<ActionResult<IHttpResult>> AddVideo([FromForm] ListVideoVM model)
            => Ok(await _videoService
                .AddVideosAsync(
                    _mapper.Map<ListVideoDto>(model)
                        .VideosDto));

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<IHttpResult<IEnumerable<VideoGetVm>>>> GetVideosBySeasonId(long id)
        {
            var result = await _videoService
                .GetVideosBySeasonIdAsync(id);
            return Ok(CheckResult<IEnumerable<VideoDto>, 
                IEnumerable<VideoGetVm>>(result));
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<IHttpResult<VideoGetVm>>> GetVideoById(long id)
        {
            var result = await _videoService
                .GetVideoByIdAsync(id);
            return Ok(CheckResult<VideoDto, 
                VideoGetVm>(result));
        }

        [Permission(Permissions.Permission.AdminPermission.RemoveVideo)]
        [HttpPost]
        public async Task<ActionResult<IHttpResult>> DeleteVideoById(long id)
            => Ok(await _videoService
                .DeleteVideoByIdAsync(id));
    }
}
