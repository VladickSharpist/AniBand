using AniBand.Core.Abstractions.Infrastructure.Helpers.Generic;
using AniBand.Core.Infrastructure.Helpers.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AniBand.Web.Core.Controllers
{
    [Authorize]
    public abstract class BaseController 
        : ControllerBase
    {
        protected readonly IMapper _mapper;

        protected BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }

        protected IHttpResult<TOut> CheckResult<TIn, TOut>(IHttpResult<TIn> result)
            where TIn : class
            where TOut : class
        {
            return result.IsSuccessful
                ? new HttpResult<TOut>(
                    _mapper.Map<TOut>(result.Data))
                : new HttpResult<TOut>(
                    null,
                    result.Errors,
                    result.StatusCode);
        }
    }
}