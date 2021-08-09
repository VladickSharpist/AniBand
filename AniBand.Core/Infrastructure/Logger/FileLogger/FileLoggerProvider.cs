using System;
using Microsoft.Extensions.Logging;

namespace AniBand.Core.Infrastructure.Logger.FileLogger
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private string _path;

        public FileLoggerProvider(string path)
        {
            _path = path;
        }

        private void Dispose(bool disposing)
        {
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_path);
        }
    }
}