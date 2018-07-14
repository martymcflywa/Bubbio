using System;
using Bubbio.Repository.MongoDb.Models;
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
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <returns></returns>
        IMongoCollection<TDocument> HandlePartition<TDocument, TKey>(string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Get a collection by a document. If TDocument is IPartitionDocument, use the partition key,
        /// otherwise just return the collection.
        /// </summary>
        /// <param name="document">The document, which may or may not be partitioned.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of partition key</typeparam>
        /// <returns></returns>
        IMongoCollection<TDocument> HandlePartition<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Get a collection. Call this from within HandlePartition overloads after partition key is resolved.
        /// </summary>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <returns></returns>
        IMongoCollection<TDocument> GetCollection<TDocument, TKey>(string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;
    }
}