using System;
using System.IO;
using System.Threading.Tasks;
using AniBand.Core.Abstractions.Infrastructure.Storages;

namespace AniBand.Core.Infrastructure.Storages
{
    internal class LocalFileStorage 
        : IFileStorage
    {
        private string _filePath;

        public LocalFileStorage(string filePath)
        {
            _filePath = filePath;
        }

        public string FilePath => _filePath;

        public async Task SaveFileAsync(Stream file, string fileName)
        {
            using (var fileStream = new FileStream(
                _filePath + $"\\{fileName}", 
                FileMode.OpenOrCreate))
            {
                await file.CopyToAsync(fileStream);
            }
        }

        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}