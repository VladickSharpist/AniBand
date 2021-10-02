using System.Collections.Generic;
using System.Threading.Tasks;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Core.Abstractions.Infrastructure.Helpers.Generic;
using AniBand.Core.Infrastructure.Helpers.Generic;
using AniBand.Domain.Enums;
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
    public class CommentController
        : BaseController
    {
        private readonly ICommentService _commentService;
        private readonly IQueryService<CommentDto> _queryService;
        
        public CommentController(
            IMapper mapper, 
            ICommentService commentService, 
            IQueryService<CommentDto> queryService)
            : base(mapper)
        {
            _commentService = commentService;
            _queryService = queryService;
        }

        [Permission(Permissions.Permission.UserPermission.Approved)]
        [HttpPost]
        public async Task<ActionResult<IHttpResult>> AddComment(CommentVm model)
            => Ok(await _commentService
                .AddCommentAsync(_mapper.Map<CommentDto>(model)));

        [Permission(Permissions.Permission.AdminPermission.ApproveComment)]
        [HttpPost]
        public async Task<ActionResult<IHttpResult>> ApproveComment(long id)
            => Ok(await _commentService.ApproveCommentAsync(id));

        [Permission(Permissions.Permission.AdminPermission.ApproveComment)]
        [HttpPost]
        public async Task<ActionResult<IHttpResult>> DeclineComment(long id, string declineMessage)
            => Ok(await _commentService.DeclineCommentAsync(id, declineMessage));
        
        [Permission(Permissions.Permission.AdminPermission.ApproveComment)]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<IHttpResult<PagedVm<CommentVm>>>> GetAllWaitingComments()
        {
            var result = await _queryService
                .GetListAsync(new Dictionary<string, string>
                {
                    { "Status", Status.Waiting.ToString() }
                });
            return Ok(CheckResult<PagedList<CommentDto>,
                PagedVm<CommentVm>>(result));
        }
        
        [HttpPost]
        public async Task<ActionResult<IHttpResult<CommentVm>>> GetComment(DataRequestVm model)
        {
            var result = await _queryService
                .GetAsync(_mapper.Map<QueryDto>(model));
            return Ok(CheckResult<CommentDto,
                CommentVm>(result));
        }
        
        [HttpPost]
        public async Task<ActionResult<IHttpResult<PagedVm<CommentVm>>>> GetListComment(DataRequestVm model)
        {
            var result = await _queryService
                .GetListAsync(_mapper.Map<QueryDto>(model));
            return Ok(CheckResult<PagedList<CommentDto>,
                PagedVm<CommentVm>>(result));
        }
    }
}