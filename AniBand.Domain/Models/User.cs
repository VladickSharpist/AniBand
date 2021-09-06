using System;
using System.Collections.Generic;
using AniBand.Domain.Abstractions.Interfaces;
using AniBand.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace AniBand.Domain.Models
{
    public class User
        : IdentityUser<long>,
          IEntity,
          IUpdatableEntity
    {
        public DateTime RegistrationDate { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokensHistory { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public long CreatedById { get; set; }
        
        public DateTime? UpdateDate { get; set; }
        
        public long? UpdatedById { get; set; }

        public Status Status { get; set; }

        public string DeclineMessage { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }

        public virtual ICollection<Rate> Rates { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<View> Views { get; set; }
    }
}
