using System;

namespace AniBand.Domain.Abstractions.Interfaces
{
    public interface ICreatableEntity
    {
        public DateTime CreateDate { get; set; }

        public long CreatedById { get; set; }
    }
}