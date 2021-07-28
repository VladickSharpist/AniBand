using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace AniBand.Domain.Models
{
    public class User:IdentityUser<long>
    {
        public DateTime RegistrationDate { get; set; }
        
        public string RefreshToken { get; set; }

        public virtual List<RefreshToken> RefreshTokensHistory { get; set; }
    }
}
