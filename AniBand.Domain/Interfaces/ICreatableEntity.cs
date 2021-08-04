using System;

namespace AniBand.Domain.Interfaces
{
    public interface ICreatableEntity
    {
        public DateTime CreateDate { get; set; }

        public long CreatedById { get; set; }
    }
}