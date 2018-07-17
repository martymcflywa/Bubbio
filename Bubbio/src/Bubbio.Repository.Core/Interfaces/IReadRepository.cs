using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Bubbio.Repository.Core.Interfaces
{
    public interface IReadRepository
    {
        /// <summary>
        /// Async find one document by its primary key.
        /// </summary>
        /// <param name="id">The primary key to find.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <returns></returns>
        Task<TDocument> FindAsync<TDocument, TKey>(
                TKey id,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async find one document by a linq predicate filter.
        /// </summary>
        /// <param name="filter">The linq predicate filter.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <returns></returns>
        Task<TDocument> FindAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async find many documents by a linq predicate filter.
        /// </summary>
        /// <param name="filter">The linq predicate filter.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <returns></returns>
        Task<IEnumerable<TDocument>> FindManyAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async find many paginated documents by a linq predicate filter.
        /// </summary>
        /// <param name="filter">The linq predicate filter.</param>
        /// <param name="skip">Number of documents to skip, default is 0.</param>
        /// <param name="take">Number of documents in page, default is 50.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <returns></returns>
        Task<IEnumerable<TDocument>> FindManyAsync<TDocument, TKey>(
            Expression<Func<TDocument, bool>> filter,
            int skip = 0,
            int take = 50,
            string partitionKey = null,
            CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async true if any documents found by a linq predicate filter.
        /// </summary>
        /// <param name="filter">The linq predicate filter.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <returns></returns>
        Task<bool> AnyAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async count of documents in the collection.
        /// Limited to partition, if partition key is provided.
        /// </summary>
        /// <param name="partitionKey"></param>
        /// <param name="token"></param>
        /// <typeparam name="TDocument"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        Task<long> CountAsync<TDocument, TKey>(
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async count of documents found by a linq predicate filter.
        /// </summary>
        /// <param name="filter">The linq predicate filter.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <returns></returns>
        Task<long> CountAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async project one document by a linq predicate filter,
        /// projecting it to another type.
        /// </summary>
        /// <param name="filter">The linq predicate filter.</param>
        /// <param name="projection">The linq projection expression.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <typeparam name="TProject">The projection type.</typeparam>
        /// <returns></returns>
        Task<TProject> ProjectOneAsync<TDocument, TKey, TProject>(
                Expression<Func<TDocument, bool>> filter,
                Expression<Func<TDocument, TProject>> projection,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProject : class;

        /// <summary>
        /// Async project many documents by a linq predicate filter,
        /// projecting them to another type.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="projection"></param>
        /// <param name="partitionKey"></param>
        /// <param name="token"></param>
        /// <typeparam name="TDocument"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TProject"></typeparam>
        /// <returns></returns>
        Task<IEnumerable<TProject>> ProjectManyAsync<TDocument, TKey, TProject>(
                Expression<Func<TDocument, bool>> filter,
                Expression<Func<TDocument, TProject>> projection,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProject : class;
    }
}