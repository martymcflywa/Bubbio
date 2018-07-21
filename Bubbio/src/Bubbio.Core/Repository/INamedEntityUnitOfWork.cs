using System;

namespace Bubbio.Core.Repository
{
    public interface INamedEntityUnitOfWork<TEntity, in TKey> : ISaveNamedEntityUnitOfWork<TKey>, IUnitOfWork<TEntity, TKey>
        where TKey : IEquatable<TKey>
    {
    }
}