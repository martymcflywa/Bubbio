using System;
using Bubbio.Core.Repository;
using MongoDB.Driver;

namespace Bubbio.Repository.MongoDb.Interfaces
{
    /// <summary>
    /// Expose helper functionality.
    /// </summary>
    public interface IRepositoryHelper
    {
        /// <summary>
        /// Get a collection by an optional partition key.
        /// </summary>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <returns></returns>
        IMongoCollection<TDocument> GetCollection<TDocument, TKey>(string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Get a collection by a document. If TDocument is IPartitionDocument, use the partition key,
        /// otherwise just return the collection.
        /// </summary>
        /// <param name="document">The document, which may or may not be partitioned.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <returns></returns>
        IMongoCollection<TDocument> GetCollection<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Resolves optional partition key. If provided, returns collection based on partition key,
        /// else returns the collection without a partition key.
        /// </summary>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <returns></returns>
        IMongoCollection<TDocument> ResolvePartition<TDocument, TKey>(string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;
    }
}