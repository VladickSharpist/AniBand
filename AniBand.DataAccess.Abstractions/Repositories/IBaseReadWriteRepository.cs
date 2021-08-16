using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace AniBand.DataAccess.Abstractions.Repositories
{
    public interface IBaseReadWriteRepository<TEntity> 
        : IBaseReadonlyRepository<TEntity>
    {
        void Save(TEntity model);

        Task SaveAsync(TEntity model);

        void SaveChanges();

        Task SaveChangesAsync();

        IDbContextTransaction BeginTransaction();

        Task<IDbContextTransaction> BeginTransactionAsync();
        
        void Remove(TEntity model);
        
        Task RemoveAsync(TEntity model);

        void Remove(long id);
        
        Task RemoveAsync(long id);
    }
}