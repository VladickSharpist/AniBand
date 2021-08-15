using AniBand.Core.Abstractions.Infrastructure.Storages;
using AniBand.Video.Services.Abstractions.Services;
using MediaToolkit;
using MediaToolkit.Model;
using Microsoft.AspNetCore.Http;

namespace AniBand.Video.Services.Services
{
    public class FileService
        : IFileService
    {
        private readonly IFileStorage _fileStorage;

        public FileService(IFileStorageProvider storageProvider)
        {
            _fileStorage = storageProvider.CreateStorage();
        }

        public string StoreFileGetUrl(IFormFile file, string fileName)
        {
            _fileStorage.SaveFile(file, fileName);
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
    }
}