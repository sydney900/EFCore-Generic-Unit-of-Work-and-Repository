using BussinessCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading;

namespace GenericUnitOfWork.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private IAppContext _context;
        private Dictionary<Type, dynamic> _dictRepositories;        

        public UnitOfWork(IAppContext context, params dynamic[] repositories)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (repositories == null || repositories.Length == 0)
                throw new ArgumentNullException("repository");

            _context = context;
            _dictRepositories = new Dictionary<Type, dynamic>();
            foreach (var item in repositories)
            {
                if (item == null)
                    throw new ArgumentNullException("repository");

                if (!_dictRepositories.ContainsKey(item.EntityType))
                    //_dictRepositories.Add(item.EntityType, item);
                    this[item.EntityType] = item;
            }
        }

        private dynamic this[Type type]
        {
            get
            {
                dynamic iRepository;
                _dictRepositories.TryGetValue(type, out iRepository);
                return iRepository;
            }
            set
            {
                _dictRepositories.Add(type, value);
            }
        }

        public IRepository<T> Repository<T>() where T : BaseEntity
        {
            IRepository<T> rep = this[typeof(T)];
            return rep;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async System.Threading.Tasks.Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync(default(CancellationToken));
        }

        public void BeginTransaction()
        {
            _context.BeginTransaction();
        }

        public bool Commit()
        {
            _context.Commit();
            return true;
        }

        public void Rollback()
        {
            _context.Rollback();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                try
                {
                    _dictRepositories.Clear();
                    _context.CloseConnection();
                }
                catch (ObjectDisposedException)
                {
                }

                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }

            _disposed = true;
        }
    }
}