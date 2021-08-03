using System;
using System.Collections.Generic;
using System.Linq;
using AniBand.DataAccess.Abstractions.Repositories;
using AniBand.Domain.Interfaces;
using AniBand.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AniBand.DataAccess.Repositories
{
    public class BaseReadWriteRepository<TEntity>
        : IBaseReadWriteRepository<TEntity> 
        where TEntity : class, IEntity
    {
        protected AniBandDbContext aniBandDbContext;
        protected DbSet<TEntity> _dbSet;

        public BaseReadWriteRepository(AniBandDbContext aniBandDbContext)
        {
            this.aniBandDbContext = aniBandDbContext 
                                    ?? throw new NullReferenceException(nameof(aniBandDbContext));
            _dbSet = aniBandDbContext.Set<TEntity>();
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
            
            aniBandDbContext.SaveChanges();
        }

        public virtual async void SaveAsync(TEntity model)
        {
            if (model.Id > 0)
            {
                _dbSet.Update(model);
            }
            else
            {
                await _dbSet.AddAsync(model);
            }

            await aniBandDbContext.SaveChangesAsync();
        }

        public virtual void Remove(TEntity model)
        {
            aniBandDbContext.Remove(model);
            aniBandDbContext.SaveChanges();
        }

        public virtual async void RemoveAsync(TEntity model)
        {
            aniBandDbContext.Remove(model);
            await aniBandDbContext.SaveChangesAsync();
        }
        
        public virtual void Remove(long id)
        {
            var model = _dbSet.SingleOrDefault(x => x.Id == id);
            Remove(model);
        }
        
        public virtual async void RemoveAsync(long id)
        {
            var model = await _dbSet.SingleOrDefaultAsync(x => x.Id == id);
            RemoveAsync(model);
        }
    }
}