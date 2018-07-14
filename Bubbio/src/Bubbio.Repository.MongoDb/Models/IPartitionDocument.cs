using System;

namespace Bubbio.Repository.MongoDb.Models
{
    /// <inheritdoc />
    ///  <summary>
    ///  Represents a document that can be inserted in a partitioned collection.
    ///  The partition key allows for the creation of different collections in the same document schema.
    ///  This can be useful for building Software as a Service (SaaS) platform,
    ///  or for reducing indexing.
    ///  Example usage: Insert logs into different partitions based on week/year,
    ///  or categorised by source or type.
    ///  </summary>
    public interface IPartitionDocument<TKey> : IDocument<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// The name of the property used for partitioning the collection.
        /// This won't be inserted into the collection, but will be prepended
        /// to the collection name to create a new collection.
        /// </summary>
        string PartitionKey { get; set; }
    }
}