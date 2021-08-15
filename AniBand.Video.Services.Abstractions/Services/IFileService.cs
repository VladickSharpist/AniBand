using Microsoft.AspNetCore.Http;

namespace AniBand.Video.Services.Abstractions.Services
{
    public interface IFileService
    {
        string StoreFileGetUrl(IFormFile file, string name);

        double GetVideoDuration(string url);
    }
}