using System;
using Bubbio.Repository.Core.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bubbio.Repository.MongoDb.Interfaces
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
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <returns></returns>
        IMongoCollection<TDocument> GetCollection<TDocument, TKey>(string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Set Guid representation.
        /// </summary>
        /// <param name="representation">How to represent Guid.</param>
        void SetGuidRepresentation(GuidRepresentation representation);
    }
}