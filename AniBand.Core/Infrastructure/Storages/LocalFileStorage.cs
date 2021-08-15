using System;
using System.IO;
using AniBand.Core.Abstractions.Infrastructure.Storages;
using Microsoft.AspNetCore.Http;

namespace AniBand.Core.Infrastructure.Storages
{
    public class LocalFileStorage : IFileStorage
    {
        private string _filePath;

        public LocalFileStorage(string filePath)
        {
            _filePath = filePath;
        }

        public string FilePath => _filePath;

        public async void SaveFile(IFormFile file, string fileName)
        {
            using (var fileStream = new FileStream(_filePath + $"\\{fileName}", FileMode.OpenOrCreate))
            {
                await file.CopyToAsync(fileStream);
            }
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}