using AniBand.Domain.Models;
using AniBand.Query.Services.Abstractions.Services;
using AniBand.Query.Services.Services;
using AniBand.Video.Services.Abstractions.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AniBand.Query.Services.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddQueryService(this IServiceCollection services)
            => services
                .AddScoped<IQueryService<CommentDto>, QueryService<CommentDto, Comment>>()
                .AddScoped<IQueryService<VideoDto>, QueryService<VideoDto, Episode>>();
    }
}