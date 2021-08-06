namespace AniBand.Auth.Services.Abstractions.Helpers
{
    public interface IConfigurationHelper
    {
        string SecretKey();

        string ConnectionString();

        string Issuer();

        string Audience();
    }
}