using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace AniBand.Core.Infrastructure.Logger.FileLogger
{
    internal class FileLogger : ILogger
    {
        private string _filePath;
        private static object _lock = new object();

        public FileLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Log<TState>(
            LogLevel logLevel, 
            EventId eventId, 
            TState state,
            Exception exception, 
            Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                lock (_lock)
                {
                    File.AppendAllText(
                        _filePath, 
                        formatter(state, exception) + Environment.NewLine);
                }
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}