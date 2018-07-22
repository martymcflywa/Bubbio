using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Bubbio.Core.Repository;

namespace Bubbio.MongoDb.Interfaces
{
    public interface IMutateMongoDbRepository
    {
        #region Create

        /// <summary>
        /// Async add one document to the collection.
        /// </summary>
        /// <param name="document">The document to add.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <returns></returns>
        Task AddAsync<TDocument, TKey>(
                TDocument document,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async add many documents to the collection.
        /// </summary>
        /// <param name="documents">The documents to add.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <returns></returns>
        Task AddAsync<TDocument, TKey>(
                IEnumerable<TDocument> documents,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        #endregion

        #region Update

        /// <summary>
        /// Async update one document in the collection.
        /// </summary>
        /// <param name="updated">The updated document.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <returns></returns>
        Task<bool> UpdateAsync<TDocument, TKey>(
                TDocument updated,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async update one document by updating the selected field with a given value.
        /// </summary>
        /// <param name="toUpdate">The document to update.</param>
        /// <param name="selector">The linq expression field selector.</param>
        /// <param name="value">The new value of selected field.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <typeparam name="TField">The field type.</typeparam>
        /// <returns></returns>
        Task<bool> UpdateAsync<TDocument, TKey, TField>(
                TDocument toUpdate,
                Expression<Func<TDocument, TField>> selector,
                TField value,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async update one document by a linq predicate filter, updating the selected
        /// field with a given value.
        /// </summary>
        /// <param name="filter">The linq predicate filter.</param>
        /// <param name="selector">The linq expression field selector.</param>
        /// <param name="value">The new value of selected field.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <typeparam name="TField">The field type.</typeparam>
        /// <returns></returns>
        Task<bool> UpdateAsync<TDocument, TKey, TField>(
                Expression<Func<TDocument, bool>> filter,
                Expression<Func<TDocument, TField>> selector,
                TField value,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        #endregion

        #region Delete

        /// <summary>
        /// Async delete one document.
        /// </summary>
        /// <param name="document">The document to delete.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <returns></returns>
        Task<long> DeleteAsync<TDocument, TKey>(
                TDocument document,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async delete one document by primary key.
        /// </summary>
        /// <param name="id">The primary key of the document to delete.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <returns></returns>
        Task<long> DeleteAsync<TDocument, TKey>(
                TKey id,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async delete one document by a linq predicate filter.
        /// </summary>
        /// <param name="filter">The linq predicate filter.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <returns></returns>
        Task<long> DeleteAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async delete many documents.
        /// </summary>
        /// <param name="documents">The documents to delete.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <returns></returns>
        Task<long> DeleteManyAsync<TDocument, TKey>(
                IEnumerable<TDocument> documents,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async delete many documents by a linq predicate filter.
        /// </summary>
        /// <param name="filter">The linq predicate filter.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <returns></returns>
        Task<long> DeleteManyAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async drop a collection with an optional partition key.
        /// </summary>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The document type.</typeparam>
        /// <typeparam name="TKey">The primary key type.</typeparam>
        /// <returns></returns>
        Task DropCollectionAsync<TDocument, TKey>(string partitionKey = null, CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        #endregion
    }
}