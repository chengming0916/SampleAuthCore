using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Entity
{
    public interface IAggregateRoot : IEntity { }

    public interface IAggregateRoot<TKey> : IAggregateRoot, IEntity<TKey>
        where TKey : IEquatable<TKey>
    { }

    public abstract class AggregateRoot<TKey> : IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
    }
}
