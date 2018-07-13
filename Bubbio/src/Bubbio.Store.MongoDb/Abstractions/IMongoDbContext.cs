using System;
using System.Threading;
using System.Threading.Tasks;
using Bubbio.Store.MongoDb.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bubbio.Store.MongoDb.Abstractions
{
    /// <summary>
    /// Expose database context functionality.
    /// </summary>
    public interface IMongoDbContext
    {
        /// <summary>
        /// Return collection for TDocument with a partition key.
        /// </summary>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <returns></returns>
        IMongoCollection<TDocument> GetCollection<TDocument, TKey>(string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Drop a collection with a partition key.
        /// </summary>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <returns></returns>
        Task DropCollectionAsync<TDocument, TKey>(string partitionKey = null, CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Set Guid representation.
        /// </summary>
        /// <param name="representation">How to represent Guid.</param>
        void SetGuidRepresentation(GuidRepresentation representation);
    }
}