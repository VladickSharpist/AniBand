using System.Collections.Generic;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Core.Abstractions.Infrastructure.Storages;

namespace AniBand.Core.Infrastructure.Storages
{
    public class LocalFileStorageProvider : IFileStorageProvider
    {
        private readonly IConfigurationHelper _helper;
        private List<LocalFileStorage> _localStorages;
        private bool _disposed = false;
        public LocalFileStorageProvider(IConfigurationHelper helper)
        {
            _helper = helper;
            _localStorages = new List<LocalFileStorage>();
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _localStorages.Clear();
                _localStorages = null;
            }
            
            _disposed = true;
        }

        public void Dispose() => Dispose(true);

        public IFileStorage CreateStorage()
        {
            var localStorage = new LocalFileStorage(_helper.LocalPathFileStorage);
            _localStorages.Add(localStorage);
            return localStorage;
        }
    }
}