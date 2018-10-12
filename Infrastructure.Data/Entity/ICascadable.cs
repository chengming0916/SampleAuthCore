using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Entity
{
    public interface ICascadable<TKey> where TKey : IEquatable<TKey>
    {
        TKey ParentId { get; set; }
    }

    public interface ICascadable<TEntity, TKey> : ICascadable<TKey>
        where TEntity : IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>
    {
        TEntity Parent { get; set; }

        ICollection<TEntity> Children { get; set; }
    }
}
