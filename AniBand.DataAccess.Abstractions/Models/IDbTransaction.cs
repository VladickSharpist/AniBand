using System;
using System.Threading.Tasks;

namespace AniBand.DataAccess.Abstractions.Models
{
    public interface IDbTransaction
        : IDisposable
    {
        void BeginTransaction();
        Task BeginTransactionAsync();
        void RollBack();
        Task RollBackAsync();
        void Commit();
        Task CommitAsync();
    }
}