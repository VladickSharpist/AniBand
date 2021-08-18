using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AniBand.DataAccess.Abstractions.Models
{
    public class DbTransaction 
        : IDbTransaction
    {
        private DbContext _context;
        private IDbContextTransaction _transaction;
        private bool _disposed = false;

        public DbTransaction(DbContext dbContext)
        {
            _context = dbContext;
        }

        public void BeginTransaction()
        {
            _transaction = _context
                .Database
                .BeginTransaction();
        }
        
        public async Task BeginTransactionAsync()
        {
            _transaction = await _context
                .Database
                .BeginTransactionAsync();
        }
        
        public void RollBack()
        { 
            _transaction.Rollback();
        }
        
        public async Task RollBackAsync()
        { 
            await _transaction.RollbackAsync();
        }
        
        public void Commit()
        {
            _transaction.Commit();
        }
        
        public async Task CommitAsync()
        { 
            await _transaction.CommitAsync();
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose() => Dispose(true);
    }
}