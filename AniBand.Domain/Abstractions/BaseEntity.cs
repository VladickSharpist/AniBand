using System;
using AniBand.Domain.Interfaces;

namespace AniBand.Domain.Abstractions
{
    public class BaseEntity 
        : IUpdatableEntity , 
        IEntity
    {
        public DateTime CreateDate { get; set; }
        
        public long CreatedById { get; set; }
        
        public DateTime? UpdateDate { get; set; }
        
        public long? UpdatedById { get; set; }
        
        public long Id { get; set; }
    }
}