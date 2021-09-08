using AniBand.Video.Services.Abstractions.Services;
using AniBand.Video.Services.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AniBand.Video.Services.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddVideo(this IServiceCollection services)
            => services
                .AddScoped<IVideoService, VideoService>()
                .AddScoped<IFileService, FileService>()
                .AddScoped<ICommentService, CommentService>()
                .AddScoped<ISeasonService, SeasonService>();
    }
}