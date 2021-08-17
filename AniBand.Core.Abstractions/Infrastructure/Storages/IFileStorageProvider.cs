using System;

namespace AniBand.Core.Abstractions.Infrastructure.Storages
{
    public interface IFileStorageProvider : IDisposable
    {
        IFileStorage CreateStorage();
    }
}