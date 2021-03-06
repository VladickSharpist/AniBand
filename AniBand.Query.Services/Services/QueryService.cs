using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
            var condition = GetWhereCondition(queryDto.Filter);

            var model = (await _unitOfWork
                .GetReadonlyRepository<TEntity>()
                .GetNoTrackingAsync(condition))
                .SingleOrDefault();
            
            var dto = _mapper.Map<TDto>(model);
            return new HttpResult<TDto>(dto);
        }
        
        public async Task<IHttpResult<TDto>> GetAsync(
            IDictionary<string, string> filter)
        {
            var condition = GetWhereCondition(filter);

            var model = (await _unitOfWork
                    .GetReadonlyRepository<TEntity>()
                    .GetNoTrackingAsync(condition))
                .SingleOrDefault();
            
            var dto = _mapper.Map<TDto>(model);
            return new HttpResult<TDto>(dto);
        }
        
        public async Task<IHttpResult<PagedList<TDto>>> GetListAsync(QueryDto queryDto)
        {
            var condition = GetWhereCondition(queryDto.Filter);
            var order = GetOrderByExpr(queryDto.OrderProp);

            var model = order != null 
                ? await _unitOfWork
                    .GetReadonlyRepository<TEntity>()
                    .GetNoTrackingAsync(condition, o => o.OrderBy(order))
                : await _unitOfWork
                    .GetReadonlyRepository<TEntity>()
                    .GetNoTrackingAsync(condition);

            var dto = PagedList<TDto>.ToPagedList(
                _mapper.Map<IEnumerable<TDto>>(model),
                queryDto.PageNumber,
                queryDto.PageSize
            );
            return new HttpResult<PagedList<TDto>>(dto);
        }
        
        public async Task<IHttpResult<PagedList<TDto>>> GetListAsync(
            IDictionary<string, string> filter,
            string orderBy = null,
            int pageNumber = default,
            int pageSize = default)
        {
            var condition = GetWhereCondition(filter);
            var order = GetOrderByExpr(orderBy);

            var model = order != null 
                ? await _unitOfWork
                    .GetReadonlyRepository<TEntity>()
                    .GetNoTrackingAsync(condition, o => o.OrderBy(order))
                : await _unitOfWork
                    .GetReadonlyRepository<TEntity>()
                    .GetNoTrackingAsync(condition);

            var dto = PagedList<TDto>.ToPagedList(
                _mapper.Map<IEnumerable<TDto>>(model),
                pageNumber,
                pageSize);
            return new HttpResult<PagedList<TDto>>(dto);
        }

        public async Task<IHttpResult<PagedList<TDto>>> GetAllAsync()
        {
            var model = await _unitOfWork
                .GetReadonlyRepository<TEntity>()
                .GetNoTrackingAsync();
            
            var dto = PagedList<TDto>.ToPagedList(
                _mapper.Map<IEnumerable<TDto>>(model)
            );
            
            return new HttpResult<PagedList<TDto>>(dto);
        }

        private Expression<Func<TEntity, bool>> GetWhereCondition(
            IDictionary<string, string> filter)
        {
            var param = Expression.Parameter(typeof(TEntity));

            var props = filter
                .Select(keyValuePair => 
                    Expression.Property(param, keyValuePair.Key))
                .ToList();
            
            var constants = props.Select(prop =>
                Expression
                    .Constant(
                        ChangeType(
                            filter[prop.Member.Name],
                            prop.Type)))
                .ToList();

            var equalsExprList = props.Select((prop, index) =>
                Expression.Equal(prop, constants[index]))
                .ToList();

            var filterExpr = equalsExprList[0];
            for (var i = 1; i < equalsExprList.Count; i++)
            {
                filterExpr = Expression.And(filterExpr, equalsExprList[i]);
            }

            return Expression
                .Lambda<Func<TEntity, bool>>(filterExpr, param);
        }

        private Expression<Func<TEntity, object>> GetOrderByExpr(
            string stringOrderProp)
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

        private object ChangeType(string value, Type type)
        {
            if (type.GetTypeInfo().IsEnum)
                return Enum.Parse(type, value);

            return Convert.ChangeType(value, type);
        }
    }
}