using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Core.Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AniBand.Core.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddHelpers(
            this IServiceCollection services, 
            IConfiguration configuration)
            => services
                .AddScoped<IConfigurationHelper>(di 
                    => new ConfigurationHelper(configuration));
        
        public static IServiceCollection AddLoggers(this IServiceCollection services)
            => services
                .AddLogging(opt =>
                {
                    opt
                     .AddConsole()
                     .AddFileLogger(
                         services
                          .BuildServiceProvider()
                          .GetRequiredService<IConfigurationHelper>());
                });
    }
}