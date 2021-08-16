using System.IO;

namespace AniBand.Video.Services.Abstractions.Services
{
    public interface IFileService
    {
        string StoreFileGetUrl(Stream file, string name);

        double GetVideoDuration(string url);

        string GetFileHash(string url);
    }
}