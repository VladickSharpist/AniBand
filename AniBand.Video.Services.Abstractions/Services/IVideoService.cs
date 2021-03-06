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

        Task<IHttpResult> SaveVideoAsync(VideoDto videoDto);

        Task<IHttpResult> DeleteVideoByIdAsync(long id);
    }
}