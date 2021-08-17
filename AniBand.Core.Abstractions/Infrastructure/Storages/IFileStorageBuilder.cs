using Microsoft.Extensions.DependencyInjection;

namespace AniBand.Core.Abstractions.Infrastructure.Storages
{
    public interface IFileStorageBuilder
    {
        IServiceCollection Services { get; }

        IServiceCollection UseLocal();
    }
}