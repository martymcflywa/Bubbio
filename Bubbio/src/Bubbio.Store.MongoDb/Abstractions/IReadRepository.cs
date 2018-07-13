using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Bubbio.Store.MongoDb.Models;
using MongoDB.Driver;

namespace Bubbio.Store.MongoDb.Abstractions
{
    /// <summary>
    /// Define readonly functionality.
    /// </summary>
    public interface IReadRepository
    {
        /// <summary>
        /// The connection string.
        /// </summary>
        string ConnectionString { get; set; }
        /// <summary>
        /// The database name.
        /// </summary>
        string DatabaseName { get; set; }

        #region Read

        /// <summary>
        /// Async find one document by its primary key.
        /// </summary>
        /// <param name="id">Primary key of document to get.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <returns></returns>
        Task<TDocument> FindAsync<TDocument, TKey>(
                TKey id,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async find one document matching a given linq predicate filter.
        /// </summary>
        /// <param name="filter">Linq predicate filter.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <returns></returns>
        Task<TDocument> FindAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async find a collection of documents matching a given linq predicate filter.
        /// </summary>
        /// <param name="filter">Linq predicate filter.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <returns></returns>
        Task<IEnumerable<TDocument>> FindManyAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Get a collection cursor where documents match a given linq predicate filter.
        /// </summary>
        /// <param name="filter">Linq predicate filter.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <returns></returns>
        IFindFluent<TDocument, TDocument> GetCursor<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async true if any documents match a given linq predicate filter.
        /// </summary>
        /// <param name="filter">Linq predicate filter.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <returns></returns>
        Task<bool> AnyAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async count how many documents match a given linq predicate filter.
        /// </summary>
        /// <param name="filter">Linq predicate filter.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <returns></returns>
        Task<long> CountAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async count how many documents are in the collection.
        /// Limit count to partition if partition key is provided.
        /// </summary>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of key.</typeparam>
        /// <returns></returns>
        Task<long> CountAsync<TDocument, TKey>(
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        #endregion
    }
}