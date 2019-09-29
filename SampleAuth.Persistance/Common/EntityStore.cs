using Infrastructure.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAuth.Persistance
{
    public class EntityStore<TEntity> where TEntity : class, IAggregateRoot
    {
        public EntityStore(DataContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            DbEntitySet = context.Set<TEntity>();
        }

        public DataContext Context { get; }

        public DbSet<TEntity> DbEntitySet { get; }

        public IQueryable<TEntity> EntitySet { get { return DbEntitySet; } }

        public virtual void Create(TEntity entity)
        {
            DbEntitySet.Add(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            DbEntitySet.Remove(entity);
        }

        public virtual void Update(TEntity entity)
        {
            Context.Update<TEntity>(entity);
            //if (entity != null)
            //    Context.Entry(entity).State = EntityState.Modified;
        }
    }

    public class EntityStore<TEntity, TKey> : EntityStore<TEntity>
        where TEntity : class, IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>
    {
        public EntityStore(DataContext context) : base(context) { }

        public virtual Task<TEntity> GetByIdAsync(TKey id)
        {
            return base.DbEntitySet.FindAsync(id);
        }
    }
}