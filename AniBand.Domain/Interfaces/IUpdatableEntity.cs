using System;

namespace AniBand.Domain.Interfaces
{
    public interface IUpdatableEntity
    {
        public DateTime? UpdateDate { get; set; }

        public long? UpdatedById { get; set; }
    }
}