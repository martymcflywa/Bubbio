using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Bubbio.Store.MongoDb.Models;

namespace Bubbio.Store.MongoDb.Abstractions
{
    /// <summary>
    /// Define crud functionality.
    /// </summary>
    public interface IMutateRepository
    {
        #region Create

        /// <summary>
        /// Async add one document to the collection.
        /// </summary>
        /// <param name="document">The document to add.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <returns></returns>
        Task AddAsync<TDocument, TKey>(
                TDocument document,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async add collection of documents to the collection.
        /// </summary>
        /// <param name="documents">The collection of documents to add.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <returns></returns>
        Task AddAsync<TDocument, TKey>(
                IEnumerable<TDocument> documents,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        #endregion

        #region Update

        /// <summary>
        /// Async update one document in mongodb.
        /// </summary>
        /// <param name="updated">The updated document.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <returns></returns>
        Task<bool> UpdateOneAsync<TDocument, TKey>(
                TDocument updated,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async update one document's selected field with given value.
        /// </summary>
        /// <param name="toUpdate">The document to update.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of selected field.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <typeparam name="TField">The type of field to update.</typeparam>
        /// <returns></returns>
        Task<bool> UpdateOneAsync<TDocument, TKey, TField>(
                TDocument toUpdate,
                Expression<Func<TDocument, TField>> field,
                TField value,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async update one document which matches a given linq predicate filter,
        /// by updating selected field with given value.
        /// </summary>
        /// <param name="filter">Linq predicate filter.</param>
        /// <param name="field">The field selector.</param>
        /// <param name="value">The new value of selected field.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <typeparam name="TField">The type of field to update.</typeparam>
        /// <returns></returns>
        Task<bool> UpdateOneAsync<TDocument, TKey, TField>(
                Expression<Func<TDocument, bool>> filter,
                Expression<Func<TDocument, TField>> field,
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
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <returns>The number of documents deleted.</returns>
        Task<long> DeleteOneAsync<TDocument, TKey>(TDocument document, CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async delete one document matching linq predicate filter.
        /// </summary>
        /// <param name="filter">Linq predicate filter.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <returns>The number of documents deleted.</returns>
        Task<long> DeleteOneAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async delete a collection of documents.
        /// </summary>
        /// <param name="documents">The documents to delete.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <returns></returns>
        Task<long> DeleteManyAsync<TDocument, TKey>(
                IEnumerable<TDocument> documents,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        /// <summary>
        /// Async delete one or more documents matching linq predicate filter.
        /// </summary>
        /// <param name="filter">Linq predicate filter.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <returns>The number of documents deleted.</returns>
        Task<long> DeleteManyAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        #endregion

        #region Projection

        /// <summary>
        /// Async project one document matching linq predicate filter.
        /// </summary>
        /// <param name="filter">Linq predicate filter.</param>
        /// <param name="projection">Linq projection expression.</param>
        /// <param name="partitionKey">Optional partion key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of partition key.</typeparam>
        /// <typeparam name="TProject">The type of projection.</typeparam>
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
        /// Async project many documents matching linq predicate filter.
        /// </summary>
        /// <param name="filter">Linq predicate filter.</param>
        /// <param name="projection">Linq projection expression.</param>
        /// <param name="partitionKey">Optional partion key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of partition key.</typeparam>
        /// <typeparam name="TProject">The type of projection.</typeparam>
        /// <returns></returns>
        Task<IEnumerable<TProject>> ProjectManyAsync<TDocument, TKey, TProject>(
                Expression<Func<TDocument, bool>> filter,
                Expression<Func<TDocument, TProject>> projection,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProject : class;

        #endregion

        #region Pagination

        /// <summary>
        /// Async get paginated collection of documents matching linq predicate filter.
        /// </summary>
        /// <param name="filter">Linq predicate filter.</param>
        /// <param name="take">Number of documents to take, default is 50.</param>
        /// <param name="skip">Number of documents to skip, default is 0.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <returns></returns>
        Task<IEnumerable<TDocument>> GetPaginatedAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                int take = 50,
                int skip = 0,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>;

        #endregion

        #region Grouping

        /// <summary>
        /// Async group a collection of documents given a grouping criteria,
        /// return a list of projected documents.
        /// </summary>
        /// <param name="selector">The grouping criteria.</param>
        /// <param name="projection">The projected group result.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <typeparam name="TGroup">The type of grouping criteria.</typeparam>
        /// <typeparam name="TProject">The type of projected group result.</typeparam>
        /// <returns></returns>
        Task<IEnumerable<TProject>> GroupByAsync<TDocument, TKey, TGroup, TProject>(
                Expression<Func<TDocument, TGroup>> selector,
                Expression<Func<IGrouping<TGroup, TDocument>, TProject>> projection,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProject : class, new();

        /// <summary>
        /// Async group a collection of documents matching linq predicate filter,
        /// given a grouping criteria, returning a dictionary of grouped documents,
        /// with keys having the different values of the grouping criteria.
        /// </summary>
        /// <param name="filter">Linq predicate filter.</param>
        /// <param name="selector">The grouping criteria.</param>
        /// <param name="projection">The projected group result.</param>
        /// <param name="partitionKey">Optional partition key.</param>
        /// <param name="token">Optional cancellation token.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <typeparam name="TKey">The type of primary key.</typeparam>
        /// <typeparam name="TGroup">The type of grouping criteria.</typeparam>
        /// <typeparam name="TProject">The type of projected group result.</typeparam>
        /// <returns></returns>
        Task<IEnumerable<TProject>> GroupByAsync<TDocument, TKey, TGroup, TProject>(
                Expression<Func<TDocument, bool>> filter,
                Expression<Func<TDocument, TGroup>> selector,
                Expression<Func<IGrouping<TGroup, TDocument>, TProject>> projection,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProject : class, new();

        #endregion
    }
}