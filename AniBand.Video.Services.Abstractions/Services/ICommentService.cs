using System.Collections.Generic;
using System.Threading.Tasks;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Core.Abstractions.Infrastructure.Helpers.Generic;
using AniBand.Video.Services.Abstractions.Models;

namespace AniBand.Video.Services.Abstractions.Services
{
    public interface ICommentService
    {
        Task<IHttpResult> AddCommentAsync(CommentDto model);

        Task<IHttpResult> ApproveCommentAsync(long id);

        Task<IHttpResult> DeclineCommentAsync(long id, string declineMessage);
    }
}