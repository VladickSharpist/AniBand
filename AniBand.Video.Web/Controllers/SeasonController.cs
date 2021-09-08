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
    public class SeasonController
        : BaseController
    {
        private readonly ISeasonService _seasonService;
        
        public SeasonController(
            IMapper mapper, 
            ISeasonService seasonService) 
            : base(mapper)
        {
            _seasonService = seasonService;
        }
        
        [Permission(Permissions.Permission.AdminPermission.AddVideo)]
        [HttpPost]
        public async Task<ActionResult<IHttpResult>> AddSeason([FromForm] SeasonVm model)
            => Ok(await _seasonService
                .AddSeasonAsync(
                    _mapper.Map<SeasonDto>(model)));
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<IHttpResult<IEnumerable<SeasonGetVm>>>> GetAllSeasons()
        {
            var result = await _seasonService
                .GetAllSeasonsAsync();
            return Ok(CheckResult<IEnumerable<SeasonDto>, 
                IEnumerable<SeasonGetVm>>(result));
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<IHttpResult<SeasonGetVm>>> GetSeasonById(long id)
        {
            var result = await _seasonService
                .GetSeasonByIdAsync(id);
            return Ok(CheckResult<SeasonDto, 
                SeasonGetVm>(result));
        }
        
        [Permission(Permissions.Permission.AdminPermission.RemoveVideo)]
        [HttpPost]
        public async Task<ActionResult<IHttpResult>> DeleteSeasonById(long id)
            => Ok(await _seasonService
                .DeleteSeasonByIdAsync(id)); 
    }
}