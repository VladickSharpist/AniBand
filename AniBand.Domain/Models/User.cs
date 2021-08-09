using System;
using System.Collections.Generic;
using AniBand.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AniBand.Domain.Models
{
    public class User
        : IdentityUser<long>,
          IEntity,
          IUpdatableEntity
    {
        public DateTime RegistrationDate { get; set; }

        public virtual List<RefreshToken> RefreshTokensHistory { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public long CreatedById { get; set; }
        
        public DateTime? UpdateDate { get; set; }
        
        public long? UpdatedById { get; set; }
    }
}
