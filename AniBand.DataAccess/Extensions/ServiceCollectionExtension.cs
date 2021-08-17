using AniBand.DataAccess.Abstractions.Repositories;
using AniBand.DataAccess.Repositories;
using AniBand.Domain.Models;
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
    }
}