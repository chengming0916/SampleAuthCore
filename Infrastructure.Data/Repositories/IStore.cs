using Infrastructure.Data.Entity;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public interface IStore<TEntity> where TEntity : IAggregateRoot
    {
        Task CreateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);
    }

    public interface IStore<TEntity, TKey> : IStore<TEntity>
        where TEntity : IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>
    {
        Task<TEntity> FindByIdAsync(TKey id);
    }
}