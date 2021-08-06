using AniBand.Core.Abstractions.Infrastructure.Helpers;
using AniBand.Core.Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
    }
}