using System;
using System.Data;
using System.Threading.Tasks;

namespace AniBand.DataAccess.Abstractions
{
    public interface IDbTransaction
        : IDisposable
    {
        void BeginTransaction(IsolationLevel level);
        
        Task BeginTransactionAsync(IsolationLevel level);
        
        void RollBack();
        
        Task RollBackAsync();
        
        void Commit();
        
        Task CommitAsync();
    }
}