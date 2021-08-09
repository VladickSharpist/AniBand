using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Core.Infrastructure.Logger.FileLogger;
using Microsoft.Extensions.Logging;

namespace AniBand.Core.Extensions
{
    public static class LogginBuilderExtension
    {
        public static ILoggingBuilder AddFileLogger(
            this ILoggingBuilder builder,
            IConfigurationHelper helper)
        {
            var filePath = helper.LogsFilePath;
            builder.AddProvider(new FileLoggerProvider(filePath));
            return builder;
        } 
    }
}