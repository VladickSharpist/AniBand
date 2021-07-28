using System;

namespace AniBand.Auth.Services.Abstractions.Models
{
    public class RefreshTokenDto
    {
        public long UserId { get; set; }
        
        public DateTime Expires { get; set; }
        
        public bool IsExpired => DateTime.UtcNow >= Expires;
        
        public DateTime Created { get; set; }
        
        public DateTime? Revoked { get; set; }
        
        public bool IsActive => Revoked == null && !IsExpired;
    }
}