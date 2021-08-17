using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using AniBand.DataAccess.Abstractions.Repositories;
using AniBand.DataAccess.Abstractions.Repositories.Generic;
using AniBand.Domain.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace AniBand.DataAccess.Repositories
{
    internal class UnitOfWork<TContext> 
        : IUnitOfWork
        where TContext : DbContext
    {
        private bool _disposed;
        private Dictionary<Type, object> _repositories;
        private IDbContextTransaction _transaction;
        
        public TContext DbContext { get; }
        
        public UnitOfWork(TContext context)
        {
            DbContext = context 
               ?? throw new ArgumentNullException(nameof(context));
        }

        public IBaseReadonlyRepository<TEntity> GetReadonlyRepository<TEntity>()
            where TEntity : class, IEntity
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new BaseReadonlyRepository<TEntity>(DbContext);
            }

            return (IBaseReadonlyRepository<TEntity>) _repositories[type];
        }

        public IBaseReadWriteRepository<TEntity> GetReadWriteRepository<TEntity>()
            where TEntity : class, IEntity
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new BaseReadWriteRepository<TEntity>(DbContext);
            }

            return (IBaseReadWriteRepository<TEntity>) _repositories[type];
        }

        public TIRepository GetCustomRepository<TEntity, TIRepository>()
            where TEntity : class, IEntity
            where TIRepository : class, IBaseReadonlyRepository<TEntity>
        {
            var customRepo = DbContext.GetService<TIRepository>();
            if (customRepo != null)
            {
                return customRepo;
            }

            return null;
        }

        public int ExecuteSqlCommand(
            string sql, 
            params object[] parameters) 
            => DbContext.Database.ExecuteSqlRaw(sql, parameters);

        public IQueryable<TEntity> FromSql<TEntity>(
            string sql, 
            params object[] parameters) 
            where TEntity : class 
            => DbContext.Set<TEntity>().FromSqlRaw(sql, parameters);

        public void BeginTransaction(
            IsolationLevel level = IsolationLevel.ReadUncommitted)
        {
            _transaction = DbContext
                .Database
                .BeginTransaction(level);
        }

        public async Task BeginTransactionAsync(
            IsolationLevel level = IsolationLevel.ReadUncommitted)
        { 
            _transaction = await DbContext
                    .Database
                    .BeginTransactionAsync(level);
        }

        public void RollBack()
        { 
            _transaction.Rollback();
            _transaction.Dispose();
        }
        
        public async Task RollBackAsync()
        { 
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }
        
        public void Commit()
        { 
            _transaction.Commit();
            _transaction.Dispose();
        }
        
        public async Task CommitAsync()
        { 
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
        }

        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }
        
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _repositories?.Clear();
                    
                    DbContext.Dispose();
                }
            }

            _disposed = true;
        }
    }
}