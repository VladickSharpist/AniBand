using System;
using System.IO;
using System.Threading.Tasks;

namespace AniBand.Core.Abstractions.Infrastructure.Storages
{
    public interface IFileStorage
    {
        public string FilePath { get; }
        
        Task SaveFileAsync(Stream file, string fileName);

        void DeleteFile(string filePath);
        
        IDisposable BeginScope<TState>(TState state);
    }
}