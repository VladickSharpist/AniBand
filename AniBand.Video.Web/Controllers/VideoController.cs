using System.Collections.Generic;
using System.Threading.Tasks;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Core.Abstractions.Infrastructure.Helpers.Generic;
using AniBand.Core.Infrastructure.Helpers.Generic;
using AniBand.Query.Services.Abstractions.Models;
using AniBand.Query.Services.Abstractions.Services;
using AniBand.Video.Services.Abstractions.Models;
using AniBand.Video.Services.Abstractions.Services;
using AniBand.Video.Web.Models;
using AniBand.Web.Core.Controllers;
using AniBand.Web.Core.Filters.Permission;
using AniBand.Web.Core.Models.Generic;
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
        private readonly IQueryService<VideoDto> _queryService;

        public VideoController(
            IVideoService videoService,
            IMapper mapper, 
            IQueryService<VideoDto> queryService)
            : base(mapper)
        {
            _videoService = videoService;
            _queryService = queryService;
        }
        
        [Permission(Permissions.Permission.AdminPermission.AddVideo)]
        [HttpPost]
        public async Task<ActionResult<IHttpResult>> AddVideo([FromForm] ListVideoPostVm model)
            => Ok(await _videoService
                .AddVideosAsync(
                    _mapper.Map<ListVideoDto>(model)
                        .VideosDto));

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<IHttpResult<IEnumerable<VideoGetVm>>>> GetVideos(DataRequestVm model)
        {
            var result = await _queryService
                .GetListAsync(_mapper.Map<QueryDto>(model));
            return Ok(CheckResult<PagedList<VideoDto>, 
                PagedVm<VideoGetVm>>(result));
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<IHttpResult<VideoGetVm>>> GetVideo(DataRequestVm model)
        {
            var result = await _queryService
                .GetAsync(_mapper.Map<QueryDto>(model));
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
