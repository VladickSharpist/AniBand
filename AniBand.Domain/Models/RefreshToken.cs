using System;
using AniBand.Domain.Interfaces;

namespace AniBand.Domain.Models
{
    public class RefreshToken:IEntity,ICreatableEntity,IUpdatableEntity
    {
        public long Id { get; set; }
        
        public string Token { get; set; }

        public User Owner { get; set; }

        public long OwnerId { get; set; }
        
        public DateTime CreateDate { get; set; }
        
        public long CreatedById { get; set; }
        
        public DateTime? UpdateDate { get; set; }
        
        public long? UpdatedById { get; set; }
    }
}
