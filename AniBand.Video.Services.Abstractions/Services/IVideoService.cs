using System.Collections.Generic;
using System.Threading.Tasks;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Core.Abstractions.Infrastructure.Helpers.Generic;
using AniBand.Video.Services.Abstractions.Models;

namespace AniBand.Video.Services.Abstractions.Services
{
    public interface IVideoService
    {
        Task<IHttpResult> AddVideosAsync(IEnumerable<VideoDto> videos);
        
        Task<IHttpResult> AddSeasonAsync(SeasonDto season);

        Task<IHttpResult> SaveVideoAsync(VideoDto videoDto);
        
        Task<IHttpResult<IEnumerable<SeasonDto>>> GetAllSeasonsAsync();
        
        Task<IHttpResult<SeasonDto>> GetSeasonByIdAsync(long id);
        
        Task<IHttpResult<IEnumerable<VideoDto>>> GetVideosBySeasonIdAsync(long id);
        
        Task<IHttpResult<VideoDto>> GetVideoByIdAsync(long id);

        Task<IHttpResult> DeleteSeasonByIdAsync(long id);
        
        Task<IHttpResult> DeleteVideoByIdAsync(long id);

        Task<IHttpResult> AddCommentAsync(CommentDto model);

        Task<IHttpResult> ApproveCommentAsync(long id);

        Task<IHttpResult> DeclineCommentAsync(long id, string declineMessage);

        Task<IHttpResult<IEnumerable<WaitingCommentDto>>> GetAllWaitingCommentsAsync();
    }
}