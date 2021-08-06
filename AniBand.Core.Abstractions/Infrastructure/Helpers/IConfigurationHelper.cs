namespace AniBand.Core.Abstractions.Infrastructure.Helpers
{
    public interface IConfigurationHelper
    {
        string SecretKey { get; }

        string ConnectionString { get; }

        string Issuer { get; }

        string Audience { get; }
    }
}