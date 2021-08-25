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

using IDbTransaction = AniBand.DataAccess.Abstractions.IDbTransaction;

namespace AniBand.DataAccess.Repositories
{
    internal class UnitOfWork<TContext> 
        : IUnitOfWork
        where TContext : DbContext
    {
        private bool _disposed;
        private IDictionary<Type, object> _repositories;
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

        public IDbTransaction BeginTransaction(
            IsolationLevel level = IsolationLevel.ReadUncommitted)
        {
            var transaction = new DbTransaction(DbContext);
            transaction.BeginTransaction(level);
            return transaction;
        }

        public async Task<IDbTransaction> BeginTransactionAsync(
            IsolationLevel level = IsolationLevel.ReadUncommitted)
        { 
            var transaction = new DbTransaction(DbContext);
            await transaction.BeginTransactionAsync(level);
            return transaction;
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