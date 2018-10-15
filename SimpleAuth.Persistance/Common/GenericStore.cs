using Infrastructure.Data.Entity;
using Infrastructure.Data.Repositories;
using System;
using System.Threading.Tasks;

namespace SimpleAuth.Persistance
{
    public abstract class GenericStore<TEntity, TKey> : IStore<TEntity, TKey>
        where TEntity : class, IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>
    {
        private bool _disposed;
        protected EntityStore<TEntity, TKey> _entityStore;

        public GenericStore(DataContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public DataContext Context { get; private set; }

        public bool DisposeContext { get; set; }

        public async Task CreateAsync(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _entityStore.Create(entity);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _entityStore.Delete(entity);
            await Context.SaveChangesAsync();
        }

        public Task<TEntity> FindByIdAsync(TKey id)
        {
            ThrowIfDisposed();
            return _entityStore.GetByIdAsync(id);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            ThrowIfDisposed();
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _entityStore.Update(entity);
            await Context.SaveChangesAsync();
        }

        protected void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (DisposeContext && disposing && Context != null)
                Context.Dispose();
            _disposed = true;
            Context = null;
        }
    }
}
