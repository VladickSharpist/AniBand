using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Core.Abstractions.Infrastructure.Storages;

namespace AniBand.Core.Infrastructure.Storages
{
    public class LocalFileStorageProvider : IFileStorageProvider
    {
        private readonly IConfigurationHelper _helper;

        public LocalFileStorageProvider(IConfigurationHelper helper)
        {
            _helper = helper;
        }
        
        public virtual void Dispose()
        {
        }

        public IFileStorage CreateStorage()
        {
            return new LocalFileStorage(_helper.LocalPathFileStorage);
        }
    }
}