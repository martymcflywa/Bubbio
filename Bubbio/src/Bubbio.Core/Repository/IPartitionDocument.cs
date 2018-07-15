using System;

namespace Bubbio.Core.Repository
{
    /// <inheritdoc />
    /// <summary>
    /// A generic partitioned document.
    /// </summary>
    /// <typeparam name="TKey">The primary key type.</typeparam>
    public interface IPartitionDocument<TKey> : IDocument<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// The partition key used to for partitioning a collection.
        /// It won't be inserted into the collection, but will prefix the
        /// collection name.
        /// </summary>
        string PartitionKey { get; set; }
    }
}