using System;

namespace Bubbio.Core.Store
{
    public interface IEntityCommon<TKey> : IEntity<TKey>
    {
        bool IsActive { get; set; }
        DateTimeOffset Created { get; set; }
        DateTimeOffset Modified { get; set; }
        string ToString();
    }
}