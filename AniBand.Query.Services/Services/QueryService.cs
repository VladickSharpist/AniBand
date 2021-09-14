using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AniBand.Core.Abstractions.Infrastructure.Helpers.Generic;
using AniBand.Core.Infrastructure.Helpers.Generic;
using AniBand.DataAccess.Abstractions.Repositories;
using AniBand.Domain.Abstractions.Abstractions;
using AniBand.Query.Services.Abstractions.Models;
using AniBand.Query.Services.Abstractions.Services;
using AutoMapper;

namespace AniBand.Query.Services.Services
{
    internal class QueryService<TDto, TEntity> 
        : IQueryService<TDto>
        where TEntity : BaseEntity
        where TDto : class, new()
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public QueryService(
            IMapper mapper, 
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IHttpResult<TDto>> GetAsync(QueryDto queryDto)
        {
            var condition = GetWhereCondition(
                queryDto.Props, 
                queryDto.Consts);
            var includes = GetIncludesList(queryDto.Includes).ToArray();

            var model = (await _unitOfWork
                .GetReadonlyRepository<TEntity>()
                .GetNoTrackingAsync(
                    condition, 
                    null, 
                    includes))
                .SingleOrDefault();
            
            var dto = _mapper.Map<TDto>(model);
            return new HttpResult<TDto>(dto);
        }
        
        public async Task<IHttpResult<TDto>> GetAsync(
            string propName, 
            string propValue,
            params string[] stringIncludes)
        {
            var condition = GetWhereCondition(
                new [] { propName },
                new [] { propValue });
            var includes = GetIncludesList(stringIncludes).ToArray();

            var model = (await _unitOfWork
                    .GetReadonlyRepository<TEntity>()
                    .GetNoTrackingAsync(
                        condition, 
                        null,
                        includes))
                .SingleOrDefault();
            
            var dto = _mapper.Map<TDto>(model);
            return new HttpResult<TDto>(dto);
        }
        
        public async Task<IHttpResult<PagedList<TDto>>> GetListAsync(QueryDto queryDto)
        {
            var condition = GetWhereCondition(
                queryDto.Props, 
                queryDto.Consts);
            var order = GetOrderByExpr(queryDto.OrderProp);
            var includes = GetIncludesList(queryDto.Includes).ToArray();

            var model = order != null 
                ? await _unitOfWork
                    .GetReadonlyRepository<TEntity>()
                    .GetNoTrackingAsync(condition, o => o.OrderBy(order), includes)
                : await _unitOfWork
                    .GetReadonlyRepository<TEntity>()
                    .GetNoTrackingAsync(condition, includes: includes);

            var dto = PagedList<TDto>.ToPagedList(
                _mapper.Map<IEnumerable<TDto>>(model),
                queryDto.PageNumber,
                queryDto.PageSize
            );
            return new HttpResult<PagedList<TDto>>(dto);
        }
        
        public async Task<IHttpResult<PagedList<TDto>>> GetListAsync(
            string propName, 
            string propValue,
            string orderBy = null,
            int pageNumber = default,
            int pageSize = default,
            params string[] stringIncludes)
        {
            var condition = GetWhereCondition(
                new [] { propName },
                new [] { propValue });
            var order = GetOrderByExpr(orderBy);
            var includes = GetIncludesList(stringIncludes).ToArray();
            
            var model = order != null 
                ? await _unitOfWork
                    .GetReadonlyRepository<TEntity>()
                    .GetNoTrackingAsync(condition, o => o.OrderBy(order), includes)
                : await _unitOfWork
                    .GetReadonlyRepository<TEntity>()
                    .GetNoTrackingAsync(condition, includes: includes);

            var dto = PagedList<TDto>.ToPagedList(
                _mapper.Map<IEnumerable<TDto>>(model),
                pageNumber,
                pageSize
                );
            return new HttpResult<PagedList<TDto>>(dto);
        }

        public async Task<IHttpResult<IEnumerable<TDto>>> GetAllAsync()
        {
            var model = await _unitOfWork
                .GetReadonlyRepository<TEntity>()
                .GetNoTrackingAsync();

            return new HttpResult<IEnumerable<TDto>>(_mapper.Map<IEnumerable<TDto>>(model));
        }

        private Expression<Func<TEntity, bool>> GetWhereCondition(
            IEnumerable<string> stringProps,
            IEnumerable<string> stringConsts)
        {
            var param = Expression.Parameter(typeof(TEntity));

            var props = stringProps.Select(stringProp =>
            {
                var prop = Expression.Property(param, stringProp);
                return prop;
            }).ToList();
            
            var constants = props.Select((prop, index) =>
            {
                var constant = Expression
                    .Constant(
                        Convert.ChangeType(
                            stringConsts.ToList()[index], 
                            prop.Type));
                return constant;
            }).ToList();

            var equalsExprList = props.Select((prop, index) =>
            {
                var equalExpr = Expression.Equal(prop, constants[index]);
                return equalExpr;
            }).ToList();

            var filterExpr = equalsExprList[0];
            for (var i = 1; i < equalsExprList.Count; i++)
            {
                filterExpr = Expression.And(filterExpr, equalsExprList[i]);
            }

            return Expression
                .Lambda<Func<TEntity, bool>>(filterExpr, param);
        }

        private Expression<Func<TEntity, object>> GetOrderByExpr(string stringOrderProp)
        {
            if (string.IsNullOrEmpty(stringOrderProp))
            {
                return null;
            }
            
            var param = Expression.Parameter(typeof(TEntity));
            var orderProperty = Expression.Property(param, stringOrderProp);
            
            var order = Expression
                .Lambda<Func<TEntity, object>>(
                    orderProperty,
                    param
                );

            return order;
        }

        private IEnumerable<Expression<Func<TEntity, object>>> GetIncludesList(
            IEnumerable<string> includes)
        {
            if (includes == null
                || !includes.Any())
            {
                return null;
            }
            
            var param = Expression.Parameter(typeof(TEntity));

            var props = includes
                .Select(include => 
                    Expression.Property(param, include));

            var includesExpr = props
                .Select(prop =>
                    Expression.Lambda<Func<TEntity, object>>(
                        prop, 
                        param));

            return includesExpr;
        }
    }
}