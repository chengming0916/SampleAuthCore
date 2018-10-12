using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Entity
{
    public interface IEntity { }

    public interface IEntity<TKey> where TKey : IEquatable<TKey>
    {
        TKey Id { get; set; }
    }

    public abstract class EntityBase<TKey> : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public TKey Id { get; set; }
    }
}
