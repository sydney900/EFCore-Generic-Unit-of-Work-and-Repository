using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;

namespace GenericUnitOfWork.Base
{
    public interface IAppContext
    {
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void BeginTransaction();
        void Commit();
        void Rollback();
        void CloseConnection();
        void Dispose();
        DbSet<T> Set<T>() where T : class;
        EntityEntry<T> Entry<T>(T entity) where T : class;
        EntityEntry<T> Update<T>(T entity) where T : class;
    }
}
