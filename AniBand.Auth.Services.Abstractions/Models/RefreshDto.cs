namespace AniBand.Auth.Services.Abstractions.Models
{
    public class RefreshDto
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}