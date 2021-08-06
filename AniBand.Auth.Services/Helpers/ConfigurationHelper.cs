using AniBand.Auth.Services.Abstractions.Helpers;
using Microsoft.Extensions.Configuration;

namespace AniBand.Auth.Services.Helpers
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        private IConfiguration _configuration;

        public ConfigurationHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string SecretKey()
            => _configuration
                .GetSection("JWTSettings:securityKey")
                .Value;

        public string ConnectionString()
            => _configuration
                .GetValue<string>("connectionString");

        public string Issuer()
            => _configuration
                .GetSection("JWTSettings:validIssuer")
                .Value;

        public string Audience()
            => _configuration
                .GetSection("JWTSettings:validAudience")
                .Value;
    }
}