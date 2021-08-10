using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace AniBand.Core.Infrastructure.Logger.FileLogger
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private bool _disposed = false;
        private string _path;
        private List<FileLogger> _loggers;

        public FileLoggerProvider(string path)
        {
            _path = path;
            _loggers = new List<FileLogger>();
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