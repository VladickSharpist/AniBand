using AniBand.Core.Abstractions.Infrastructure.Storages;
using Microsoft.Extensions.DependencyInjection;

namespace AniBand.Core.Infrastructure.Storages
{
    internal class FileStorageBuilder 
        : IFileStorageBuilder
    {
        public FileStorageBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }

        public IServiceCollection UseLocal()
            => Services
                .AddScoped<IFileStorageProvider, LocalFileStorageProvider>();
    }
}