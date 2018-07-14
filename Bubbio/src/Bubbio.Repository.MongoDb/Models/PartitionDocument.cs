using System;

namespace Bubbio.Repository.MongoDb.Models
{
    /// <inheritdoc cref="IPartitionDocument{TKey}" />
    /// <summary>
    /// </summary>
    public class PartitionDocument<TKey> : IPartitionDocument<TKey>
        where TKey : IEquatable<TKey>
    {
        #region IPartitionDocument

        public TKey Id { get; set; }
        public int Version { get; set; }

        #endregion

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public string PartitionKey { get; set; }

        /// <summary>
        /// The constructor for a partitioned document, requires a partition key.
        /// </summary>
        /// <param name="partitionKey"></param>
        public PartitionDocument(string partitionKey)
        {
            PartitionKey = partitionKey;
        }
    }
}