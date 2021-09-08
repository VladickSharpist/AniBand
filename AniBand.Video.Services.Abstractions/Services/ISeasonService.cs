using System.Collections.Generic;
using System.Threading.Tasks;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Core.Abstractions.Infrastructure.Helpers.Generic;
using AniBand.Video.Services.Abstractions.Models;

namespace AniBand.Video.Services.Abstractions.Services
{
    public interface ISeasonService
    {
        Task<IHttpResult> AddSeasonAsync(SeasonDto season);
        
        Task<IHttpResult<IEnumerable<SeasonDto>>> GetAllSeasonsAsync();
        
        Task<IHttpResult<SeasonDto>> GetSeasonByIdAsync(long id);
        
        Task<IHttpResult> DeleteSeasonByIdAsync(long id);
    }
}