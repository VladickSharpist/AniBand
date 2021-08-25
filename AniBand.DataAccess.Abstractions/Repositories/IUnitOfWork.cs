using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AniBand.DataAccess.Abstractions.Repositories.Generic;
using AniBand.Domain.Abstractions.Interfaces;

using IDbTransaction = AniBand.DataAccess.Abstractions.IDbTransaction;

namespace AniBand.DataAccess.Abstractions.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseReadonlyRepository<TEntity> GetReadonlyRepository<TEntity>() 
            where TEntity : class, IEntity;
        
        IBaseReadWriteRepository<TEntity> GetReadWriteRepository<TEntity>() 
            where TEntity : class, IEntity;

        TIRepository GetCustomRepository<TEntity, TIRepository>()
            where TEntity : class, IEntity
            where TIRepository : class, IBaseReadonlyRepository<TEntity>;

        IDbTransaction BeginTransaction(
            IsolationLevel level = IsolationLevel.ReadUncommitted);

        Task<IDbTransaction> BeginTransactionAsync(
            IsolationLevel level = IsolationLevel.ReadUncommitted);
        
        int SaveChanges();
        
        Task<int> SaveChangesAsync();

        int ExecuteSqlCommand(
            string sql, 
            params object[] parameters);

        IQueryable<TEntity> FromSql<TEntity>(
            string sql, 
            params object[] parameters) 
            where TEntity : class;
    }
}