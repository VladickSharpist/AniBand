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
using Microsoft.AspNetCore.Mvc;

namespace AniBand.Video.Web.Controllers
{
    public class CommentController
        : BaseController
    {
        private readonly ICommentService _commentService;
        
        public CommentController(
            IMapper mapper, 
            ICommentService commentService)
            : base(mapper)
        {
            _commentService = commentService;
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
        [HttpPost]
        public async Task<ActionResult<IHttpResult<IEnumerable<WaitingCommentVm>>>> GetAllWaitingComments()
        {
            var result = await _commentService
                .GetAllWaitingCommentsAsync();
            return Ok(CheckResult<IEnumerable<WaitingCommentDto>,
                IEnumerable<WaitingCommentVm>>(result));
        }
    }
}