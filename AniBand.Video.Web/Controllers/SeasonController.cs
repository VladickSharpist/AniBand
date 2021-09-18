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
    public class SeasonController
        : BaseController
    {
        private readonly ISeasonService _seasonService;
        private readonly IQueryService<SeasonDto> _queryService;
        
        public SeasonController(
            IMapper mapper, 
            ISeasonService seasonService, 
            IQueryService<SeasonDto> queryService) 
            : base(mapper)
        {
            _seasonService = seasonService;
            _queryService = queryService;
        }
        
        [Permission(Permissions.Permission.AdminPermission.AddVideo)]
        [HttpPost]
        public async Task<ActionResult<IHttpResult>> AddSeason([FromForm] SeasonPostVm model)
            => Ok(await _seasonService
                .AddSeasonAsync(
                    _mapper.Map<SeasonDto>(model)));
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<IHttpResult<PagedVm<SeasonGetVm>>>> GetAllSeasons()
        {
            var result = await _queryService
                .GetAllAsync();
            return Ok(CheckResult<PagedList<SeasonDto>, 
                PagedVm<SeasonGetVm>>(result));
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<IHttpResult<SeasonGetVm>>> GetSeason(DataRequestVm model)
        {
            var resultSeason = await _queryService
                .GetAsync(_mapper.Map<QueryDto>(model));
            return Ok(CheckResult<SeasonDto, 
                SeasonGetVm>(resultSeason));
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<IHttpResult<SeasonGetVm>>> GetListSeason(DataRequestVm model)
        {
            var resultSeason = await _queryService
                .GetListAsync(_mapper.Map<QueryDto>(model));
            return Ok(CheckResult<PagedList<SeasonDto>, 
                PagedVm<SeasonGetVm>>(resultSeason));
        }
        
        [Permission(Permissions.Permission.AdminPermission.RemoveVideo)]
        [HttpPost]
        public async Task<ActionResult<IHttpResult>> DeleteSeasonById(long id)
            => Ok(await _seasonService
                .DeleteSeasonByIdAsync(id)); 
    }
}