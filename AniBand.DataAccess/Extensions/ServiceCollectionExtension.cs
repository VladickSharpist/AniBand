using AniBand.DataAccess.Abstractions.Repositories;
using AniBand.DataAccess.Abstractions.Repositories.Generic;
using AniBand.DataAccess.Repositories;
using AniBand.Domain.Abstractions.Interfaces;
using AniBand.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AniBand.DataAccess.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
            => services
                .AddScoped<IBaseReadWriteRepository<RefreshToken>,
                    BaseReadWriteRepository<RefreshToken>>()
                .AddScoped<IBaseReadonlyRepository<UserToken>,
                    BaseReadonlyRepository<UserToken>>()
                .AddScoped<IBaseReadonlyRepository<Season>,
                BaseReadonlyRepository<Season>>()
                .AddScoped<IBaseReadWriteRepository<Video>,
                    BaseReadWriteRepository<Video>>()
                .AddScoped<IBaseReadonlyRepository<Video>,
                    BaseReadonlyRepository<Video>>()
                .AddScoped<IBaseReadWriteRepository<Season>,
                    BaseReadWriteRepository<Season>>()
                .AddScoped<IBaseReadonlyRepository<Studio>,
                    BaseReadonlyRepository<Studio>>();

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