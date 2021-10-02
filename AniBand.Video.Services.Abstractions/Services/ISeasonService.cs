using System.Threading.Tasks;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Video.Services.Abstractions.Models;

namespace AniBand.Video.Services.Abstractions.Services
{
    public interface ISeasonService
    {
        Task<IHttpResult> AddSeasonAsync(SeasonDto season);

        Task<IHttpResult> DeleteSeasonByIdAsync(long id);
    }
}