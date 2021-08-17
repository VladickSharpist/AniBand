using System.Collections.Generic;
using System.Threading.Tasks;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Video.Services.Abstractions.Models;

namespace AniBand.Video.Services.Abstractions.Services
{
    public interface IVideoService
    {
        Task<IHttpResult> AddVideoAsync(IEnumerable<VideoDto> videos);
        
        Task<IHttpResult> AddSeasonAsync(SeasonDto season);
    }
}