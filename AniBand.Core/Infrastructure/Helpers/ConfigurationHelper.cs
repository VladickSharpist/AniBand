using System;
using System.Collections.Generic;
using System.Linq;
using AniBand.Core.Abstractions.Infrastructure.Helpers;
using Microsoft.Extensions.Configuration;

namespace AniBand.Core.Infrastructure.Helpers
{
    public class ConfigurationHelper 
        : IConfigurationHelper
    {
        private IConfiguration _configuration;

        public string SecretKey => _configuration
            .GetSection("JWTSettings:securityKey")
            .Value;
        
        public string ConnectionString => _configuration
            .GetConnectionString("AniBand");

        public string Issuer => _configuration
            .GetSection("JWTSettings:validIssuer")
            .Value;

        public string Audience => _configuration
            .GetSection("JWTSettings:validAudience")
            .Value;

        public string LogsFilePath => _configuration
            .GetSection("PathToLogFiles:TextLogPath")
            .Value;

        public double TokenExpireSeconds => Convert
            .ToDouble(_configuration
            .GetSection("JWTSettings:expiryInSeconds")
            .Value);

        public string LocalPathFileStorage => _configuration
            .GetSection("PathToStoreFiles:LocalPath")
            .Value;

        public IEnumerable<string> AllowedOrigins => _configuration
            .GetSection("Origins")
            .GetChildren()
            .Select(ch => ch.Value);

        public ConfigurationHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
    }
}