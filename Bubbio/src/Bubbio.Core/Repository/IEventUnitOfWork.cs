using System;

namespace Bubbio.Core.Repository
{
    public interface IEventUnitOfWork<TEntity, in TKey> : ISaveEventUnitOfWork<TKey>, IUnitOfWork<TEntity, TKey>
        where TKey : IEquatable<TKey>
    {
    }
}