using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace AniBand.Core.Infrastructure.Logger.FileLogger
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private string _path;
        private List<FileLogger> _loggers;

        public FileLoggerProvider(string path)
        {
            _path = path;
            _loggers = new List<FileLogger>();
        }

        private void Dispose(bool disposing)
        {
            _loggers.Clear();
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public ILogger CreateLogger(string categoryName)
        {
            var fileLogger = new FileLogger(_path);
            _loggers.Add(fileLogger);
            return new FileLogger(_path);
        }
    }
}