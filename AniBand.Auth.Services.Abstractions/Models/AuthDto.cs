using System.Security.Claims;

namespace AniBand.Auth.Services.Abstractions.Models
{
    public class AuthDto
    {
        public string RefreshToken { get; set; }

        public ClaimsPrincipal ClaimsPrincipal { get; set; }
    }
}