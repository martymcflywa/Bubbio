using System;
using System.Threading.Tasks;
using Bubbio.Core.Contracts.Enums;

namespace Bubbio.Core.Repository
{
    public interface ISaveEventUnitOfWork<in TKey>
        where TKey : IEquatable<TKey>
    {
        #region Create

        Task SaveAsync(TKey foreignId, EventType eventType);

        #endregion
    }
}