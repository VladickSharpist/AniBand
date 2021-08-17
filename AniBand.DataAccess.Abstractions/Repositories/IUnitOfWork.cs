using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AniBand.DataAccess.Abstractions.Repositories.Generic;
using AniBand.Domain.Abstractions.Interfaces;

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

        void BeginTransaction(
            IsolationLevel level = IsolationLevel.ReadUncommitted);

        Task BeginTransactionAsync(
            IsolationLevel level = IsolationLevel.ReadUncommitted);

        void RollBack();

        Task RollBackAsync();

        void Commit();

        Task CommitAsync();
        
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