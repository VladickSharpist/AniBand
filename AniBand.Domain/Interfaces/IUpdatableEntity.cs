using System;

namespace AniBand.Domain.Interfaces
{
    public interface IUpdatableEntity 
        : ICreatableEntity
    {
        public DateTime? UpdateDate { get; set; }

        public long? UpdatedById { get; set; }
    }
}