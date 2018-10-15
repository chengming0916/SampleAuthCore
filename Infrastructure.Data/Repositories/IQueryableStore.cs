using Infrastructure.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Repositories
{
    public interface IQueryableStore<TEntity> where TEntity : class, IAggregateRoot
    {
        IQueryable<TEntity> QueryContext { get; }
    }
}
