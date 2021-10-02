using System.Collections.Generic;

namespace AniBand.Core.Abstractions.Infrastructure.Helpers
{
    public interface IConfigurationHelper
    {
        string SecretKey { get; }

        string ConnectionString { get; }

        string Issuer { get; }

        string Audience { get; }
        
        string LogsFilePath { get; }
        
        double TokenExpireSeconds { get; }
        
        string LocalPathFileStorage { get; }
        
        IEnumerable<string> AllowedOrigins { get; }
    }
}