using System;
using Microsoft.AspNetCore.Http;

namespace AniBand.Core.Abstractions.Infrastructure.Storages
{
    public interface IFileStorage
    {
        public string FilePath { get; }
        
        void SaveFile(IFormFile file, string fileName);
        
        IDisposable BeginScope<TState>(TState state);
    }
}