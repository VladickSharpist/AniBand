using System.Collections.Generic;

namespace AniBand.DataAccess.Abstractions.Repositories
{
    public interface IBaseReadWriteRepository<TEntity>
    {
        void Save(TEntity model);

        void SaveAsync(TEntity model);
        
        void Remove(TEntity model);
        
        void RemoveAsync(TEntity model);

        void Remove(long id);
        
        void RemoveAsync(long id);
    }
}