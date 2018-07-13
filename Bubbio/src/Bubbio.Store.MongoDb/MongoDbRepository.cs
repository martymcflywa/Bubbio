using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Bubbio.Core.Helpers;
using Bubbio.Store.MongoDb.Abstractions;
using Bubbio.Store.MongoDb.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bubbio.Store.MongoDb
{
    public class MongoDbRepository : IMongoDbRepository
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        private readonly IMongoDbContext _dbContext;

        #region Constructors

        public MongoDbRepository(IMongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public MongoDbRepository(MongoUrl url)
        {
            _dbContext = new MongoDbContext(url);
        }

        public MongoDbRepository(IMongoDatabase database)
        {
            _dbContext = new MongoDbContext(database);
        }

        public MongoDbRepository(string connectionString, string databaseName)
        {
            _dbContext = new MongoDbContext(connectionString, databaseName);
        }

        #endregion

        #region Read

        /// <inheritdoc />
        public async Task<TDocument> FindAsync<TDocument, TKey>(
                TKey id,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await HandlePartition<TDocument, TKey>(partitionKey)
                .Find(doc => doc.Id.Equals(id))
                .FirstOrDefaultAsync(token);
        }

        /// <inheritdoc />
        public async Task<TDocument> FindAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await HandlePartition<TDocument, TKey>(partitionKey)
                .Find(filter)
                .FirstOrDefaultAsync(token);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TDocument>> FindManyAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await HandlePartition<TDocument, TKey>(partitionKey)
                .Find(filter)
                .ToListAsync(token);
        }

        /// <inheritdoc />
        public IFindFluent<TDocument, TDocument> GetCursor<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return HandlePartition<TDocument, TKey>(partitionKey)
                .Find(filter);
        }

        /// <inheritdoc />
        public async Task<bool> AnyAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await HandlePartition<TDocument, TKey>(partitionKey)
                       .CountDocumentsAsync(filter, cancellationToken: token) > 0;
        }

        /// <inheritdoc />
        public async Task<long> CountAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await HandlePartition<TDocument, TKey>(partitionKey)
                .CountDocumentsAsync(filter, cancellationToken: token);
        }

        /// <inheritdoc />
        public async Task<long> CountAsync<TDocument, TKey>(
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await HandlePartition<TDocument, TKey>(partitionKey)
                .CountDocumentsAsync(new BsonDocument(), cancellationToken: token);
        }

        #endregion

        #region Create

        /// <inheritdoc />
        public async Task AddAsync<TDocument, TKey>(TDocument document, CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            await HandlePartition<TDocument, TKey>(document)
                .InsertOneAsync(document, cancellationToken: token);
        }

        /// <inheritdoc />
        public async Task AddAsync<TDocument, TKey>(IEnumerable<TDocument> documents, CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var enumerable = documents.ToList();

            if (!enumerable.Any())
                return;

            await HandlePartition<TDocument, TKey>(enumerable.FirstOrDefault())
                .InsertManyAsync(enumerable, cancellationToken: token);
        }

        #endregion

        #region Update

        public async Task<bool> UpdateOneAsync<TDocument, TKey>(TDocument updated, CancellationToken token = default) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateOneAsync<TDocument, TKey, TField>(TDocument toUpdate, Expression<Func<TDocument, TField>> field, TField value,
            CancellationToken token = default) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateOneAsync<TDocument, TKey, TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value,
            string partitionKey = null, CancellationToken token = default) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Delete

        public async Task<long> DeleteOneAsync<TDocument, TKey>(TDocument document, CancellationToken token = default) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public async Task<long> DeleteOneAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null, CancellationToken token = default) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public async Task<long> DeleteManyAsync<TDocument, TKey>(IEnumerable<TDocument> documents, CancellationToken token = default) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        public async Task<long> DeleteManyAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null, CancellationToken token = default) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Projection

        public async Task<TProject> ProjectOneAsync<TDocument, TKey, TProject>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProject>> projection, string partitionKey = null,
            CancellationToken token = default) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey> where TProject : class
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TProject>> ProjectManyAsync<TDocument, TKey, TProject>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TProject>> projection, string partitionKey = null,
            CancellationToken token = default) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey> where TProject : class
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Pagination

        public async Task<IEnumerable<TDocument>> GetPaginatedAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, int take = 50, int skip = 0, string partitionKey = null,
            CancellationToken token = default) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Grouping

        public async Task<IEnumerable<TProject>> GroupByAsync<TDocument, TKey, TGroup, TProject>(Expression<Func<TDocument, TGroup>> selector, Expression<Func<IGrouping<TGroup, TDocument>, TProject>> projection,
            string partitionKey = null, CancellationToken token = default) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey> where TProject : class, new()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TProject>> GroupByAsync<TDocument, TKey, TGroup, TProject>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TGroup>> selector, Expression<Func<IGrouping<TGroup, TDocument>, TProject>> projection,
            string partitionKey = null, CancellationToken token = default) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey> where TProject : class, new()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Utility

        /// <inheritdoc />
        public IMongoCollection<TDocument> HandlePartition<TDocument, TKey>(string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return partitionKey.IsEmpty()
                ? GetCollection<TDocument, TKey>()
                : GetCollection<TDocument, TKey>(partitionKey);
        }

        /// <inheritdoc />
        public IMongoCollection<TDocument> HandlePartition<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            if (document is IPartitionDocument<TKey> partitionedDocument)
                return GetCollection<TDocument, TKey>(partitionedDocument.PartitionKey);

            return GetCollection<TDocument, TKey>();
        }

        /// <inheritdoc />
        public IMongoCollection<TDocument> GetCollection<TDocument, TKey>(string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return partitionKey.IsEmpty()
                ? _dbContext.GetCollection<TDocument, TKey>()
                : _dbContext.GetCollection<TDocument, TKey>(partitionKey);
        }

        #endregion
    }
}