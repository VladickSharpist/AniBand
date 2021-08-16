using System.Linq;
using System.Threading.Tasks;
using AniBand.DataAccess.Abstractions.Repositories;
using AniBand.Domain.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AniBand.DataAccess.Repositories
{
    internal class BaseReadWriteRepository<TEntity>
        : BaseReadonlyRepository<TEntity>, 
          IBaseReadWriteRepository<TEntity> 
        where TEntity : class, IEntity
    {
        public BaseReadWriteRepository(AniBandDbContext aniBandDbContext) 
            : base(aniBandDbContext)
        {
        }

        public virtual void Save(TEntity model)
        {
            if (model.Id > 0)
            {
                _dbSet.Update(model);
            }
            else
            {
                _dbSet.Add(model);
            }
            
            _aniBandDbContext.SaveChanges();
        }

        public virtual async Task SaveAsync(TEntity model)
        {
            if (model.Id > 0)
            {
                _dbSet.Update(model);
            }
            else
            {
                await _dbSet.AddAsync(model);
            }

            await _aniBandDbContext.SaveChangesAsync();
        }

        public void SaveChanges()
        {
            _aniBandDbContext.SaveChanges();
        }
        
        public async Task SaveChangesAsync()
        {
            await _aniBandDbContext.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
            => _aniBandDbContext
                .Database
                .BeginTransaction();
        
        public async Task<IDbContextTransaction> BeginTransactionAsync()
            => await _aniBandDbContext
                .Database
                .BeginTransactionAsync();

        public virtual void Remove(TEntity model)
        {
            _aniBandDbContext.Remove(model);
            _aniBandDbContext.SaveChanges();
        }

        public virtual async Task RemoveAsync(TEntity model)
        {
            _aniBandDbContext.Remove(model);
            await _aniBandDbContext.SaveChangesAsync();
        }
        
        public virtual void Remove(long id)
        {
            var model = _dbSet.SingleOrDefault(x => x.Id == id);
            Remove(model);
        }
        
        public virtual async Task RemoveAsync(long id)
        {
            var model = await _dbSet.SingleOrDefaultAsync(x => x.Id == id);
            await RemoveAsync(model);
        }
    }
}