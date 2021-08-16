using System;
using System.IO;
using System.Security.Cryptography;
using AniBand.Core.Abstractions.Infrastructure.Storages;
using AniBand.Video.Services.Abstractions.Services;
using MediaToolkit;
using MediaToolkit.Model;

namespace AniBand.Video.Services.Services
{
    internal class FileService
        : IFileService
    {
        private readonly IFileStorage _fileStorage;

        public FileService(IFileStorageProvider storageProvider)
        {
            _fileStorage = storageProvider.CreateStorage();
        }

        public string StoreFileGetUrl(Stream file, string fileName)
        {
            _fileStorage.SaveFileAsync(file, fileName);
            return _fileStorage.FilePath + $"\\{fileName}";
        }

        public double GetVideoDuration(string url)
        {
            var video = new MediaFile(url);
            using (var engine = new Engine())
            {
                engine.GetMetadata(video);
            }

            return video.Metadata.Duration.TotalSeconds;
        }

        public string GetFileHash(string url)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(url))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter
                        .ToString(hash)
                        .Replace("-", string.Empty)
                        .ToLowerInvariant();
                }
            }
        }
    }
}