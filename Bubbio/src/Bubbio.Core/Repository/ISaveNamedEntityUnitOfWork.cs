using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bubbio.Core.Repository
{
    /// <summary>
    /// Expose save operation for entities with names,
    /// ie. Parent, Child.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface ISaveNamedEntityUnitOfWork<in TKey>
        where TKey : IEquatable<TKey>
    {
        #region Create

        /// <summary>
        /// Async save one named entity.
        /// </summary>
        /// <param name="first">First name.</param>
        /// <param name="last">Last name.</param>
        /// <param name="middle">Optional middle name.</param>
        /// <param name="foreignId">Optional foreign key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <returns></returns>
        Task SaveAsync(string first, string last, string middle = null, TKey foreignId = default,
            CancellationToken token = default);

        #endregion
    }
}