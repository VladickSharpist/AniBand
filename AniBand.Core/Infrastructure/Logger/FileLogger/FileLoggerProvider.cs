using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace AniBand.Core.Infrastructure.Logger.FileLogger
{
    internal class FileLoggerProvider : ILoggerProvider
    {
        private string _path;
        private IList<ILogger> _loggers;
        private bool _disposed = false;

        public FileLoggerProvider(string path)
        {
            _path = path;
            _loggers = new List<ILogger>();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _loggers.Clear();
                _loggers = null;
            }
            
            _disposed = true;
        }
        
        public void Dispose() => Dispose(true);
        
        public ILogger CreateLogger(string categoryName)
        {
            var fileLogger = new FileLogger(_path);
            _loggers.Add(fileLogger);
            return new FileLogger(_path);
        }
    }
}