using System.Collections.Generic;
using System.Threading.Tasks;
using AniBand.Core.Abstractions.Infrastructure.Helpers.Generic;
using AniBand.Core.Infrastructure.Helpers.Generic;
using AniBand.Query.Services.Abstractions.Models;

namespace AniBand.Query.Services.Abstractions.Services
{
    public interface IQueryService<TDto>
        where TDto : class, new()
    {
        Task<IHttpResult<TDto>> GetAsync(QueryDto queryDto);

        Task<IHttpResult<TDto>> GetAsync(
            IDictionary<string, string> filter);

        Task<IHttpResult<PagedList<TDto>>> GetListAsync(QueryDto queryDto);

        Task<IHttpResult<PagedList<TDto>>> GetListAsync(
            IDictionary<string, string> filter,
            string orderBy = null,
            int pageNumber = default,
            int pageSize = default);

        Task<IHttpResult<PagedList<TDto>>> GetAllAsync();
    }
}