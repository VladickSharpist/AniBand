using System;

namespace AniBand.Domain.Abstractions.Interfaces
{
    public interface IUpdatableEntity 
        : ICreatableEntity
    {
        public DateTime? UpdateDate { get; set; }

        public long? UpdatedById { get; set; }
    }
}