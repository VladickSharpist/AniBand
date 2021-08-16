using System;
using System.IO;

namespace AniBand.Core.Abstractions.Infrastructure.Storages
{
    public interface IFileStorage
    {
        public string FilePath { get; }
        
        void SaveFileAsync(Stream file, string fileName);
        
        IDisposable BeginScope<TState>(TState state);
    }
}