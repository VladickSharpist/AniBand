using AniBand.DataAccess.Abstractions.Repositories;
using AniBand.DataAccess.Abstractions.Repositories.Generic;
using AniBand.DataAccess.Repositories;
using AniBand.Domain.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AniBand.DataAccess.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
            where TContext : DbContext
            => services
                .AddScoped<IUnitOfWork, UnitOfWork<TContext>>();

        public static IServiceCollection AddCustomRepository<TEntity, TIRepository, TRepository>(
            this IServiceCollection services)
            where TEntity : class, IEntity
            where TIRepository : class, IBaseReadonlyRepository<TEntity>
            where TRepository : class, TIRepository
            => services
                .AddScoped<TIRepository, TRepository>();
    }
}