using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Bubbio.Core.Helpers;
using Bubbio.Core.Repository;
using Bubbio.MongoDb.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bubbio.MongoDb
{
    /// <inheritdoc cref="IMongoDbRepository" />
    /// <inheritdoc cref="IMongoDbRepositoryHelper" />
    /// <summary>
    /// Implementation of IRepository for MongoDb.
    /// </summary>
    public class MongoDbRepository : IMongoDbRepository, IMongoDbRepositoryHelper
    {
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
            try
            {
                return await GetCollection<TDocument, TKey>(partitionKey)
                    .Find(doc => doc.Id.Equals(id))
                    .SingleOrDefaultAsync(token);
            }
            catch (InvalidOperationException)
            {
                return default;
            }
        }

        /// <inheritdoc />
        public async Task<TDocument> FindAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            try
            {
                return await GetCollection<TDocument, TKey>(partitionKey)
                    .Find(filter)
                    .SingleOrDefaultAsync(token);
            }
            catch (InvalidOperationException)
            {
                return default;
            }
        }

        /// <inheritdoc />
        public async Task<TDocument> FindLastAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter,
                Expression<Func<TDocument, object>> orderBy, string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await GetCollection<TDocument, TKey>(partitionKey)
                .Find(filter)
                .SortByDescending(orderBy)
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
            return await GetCollection<TDocument, TKey>(partitionKey)
                .Find(filter)
                .ToListAsync(token);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TDocument>> FindManyAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                int skip = 0,
                int take = 50,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await GetCollection<TDocument, TKey>(partitionKey)
                .Find(filter)
                .Skip(skip)
                .Limit(take)
                .ToListAsync(token);
        }

        /// <inheritdoc />
        public async Task<bool> AnyAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await GetCollection<TDocument, TKey>(partitionKey)
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
            return await GetCollection<TDocument, TKey>(partitionKey)
                .CountDocumentsAsync(filter, cancellationToken: token);
        }

        /// <inheritdoc />
        public async Task<long> CountAsync<TDocument, TKey>(
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return await GetCollection<TDocument, TKey>(partitionKey)
                .CountDocumentsAsync(new BsonDocument(), cancellationToken: token);
        }

        /// <inheritdoc />
        public async Task<TProject> ProjectAsync<TDocument, TKey, TProject>(
                Expression<Func<TDocument, bool>> filter,
                Expression<Func<TDocument, TProject>> projection,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProject : class
        {
            try
            {
                return await GetCollection<TDocument, TKey>(partitionKey)
                    .Find(filter)
                    .Project(projection)
                    .SingleOrDefaultAsync(token);
            }
            catch (InvalidOperationException)
            {
                return default;
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TProject>> ProjectManyAsync<TDocument, TKey, TProject>(
                Expression<Func<TDocument, bool>> filter,
                Expression<Func<TDocument, TProject>> projection,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
            where TProject : class
        {
            return await GetCollection<TDocument, TKey>(partitionKey)
                .Find(filter)
                .Project(projection)
                .ToListAsync(token);
        }

        #endregion

        #region Create

        /// <inheritdoc />
        public async Task AddAsync<TDocument, TKey>(
                TDocument document,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            await GetCollection<TDocument, TKey>(document, partitionKey)
                .InsertOneAsync(document, cancellationToken: token);
        }

        /// <inheritdoc />
        public async Task AddAsync<TDocument, TKey>(
                IEnumerable<TDocument> documents,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var enumerable = documents.ToList();

            if (!enumerable.Any())
                return;

            await GetCollection<TDocument, TKey>(enumerable.FirstOrDefault(), partitionKey)
                .InsertManyAsync(enumerable, cancellationToken: token);
        }

        #endregion

        #region Update

        /// <inheritdoc />
        public async Task<bool> UpdateAsync<TDocument, TKey>(
                TDocument updated,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            updated.Modified = DateTimeOffset.UtcNow;
            var result = await GetCollection<TDocument, TKey>(updated, partitionKey)
                .ReplaceOneAsync(
                    d => d.Id.Equals(updated.Id),
                    updated,
                    new UpdateOptions {IsUpsert = true},
                    token);

            return result.ModifiedCount > 0;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync<TDocument, TKey, TField>(
                TDocument toUpdate,
                Expression<Func<TDocument, TField>> selector,
                TField value,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var task = Builders<TDocument>
                .Update
                .Set(selector, value)
                .Set(d => d.Modified, DateTimeOffset.UtcNow);

            var result = await GetCollection<TDocument, TKey>(toUpdate, partitionKey)
                .UpdateOneAsync(d => d.Id.Equals(toUpdate.Id), task, cancellationToken: token);

            return result.ModifiedCount > 0;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync<TDocument, TKey, TField>(
                Expression<Func<TDocument, bool>> filter,
                Expression<Func<TDocument, TField>> selector,
                TField value,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            if (await CountAsync<TDocument, TKey>(filter, partitionKey, token) > 1)
                return false;

            var task = Builders<TDocument>
                .Update
                .Set(selector, value)
                .Set(d => d.Modified, DateTimeOffset.UtcNow);

            var result = await GetCollection<TDocument, TKey>(partitionKey)
                .UpdateOneAsync(filter, task, cancellationToken: token);

            return result.ModifiedCount > 0;
        }

        #endregion

        #region Delete

        /// <inheritdoc />
        public async Task<long> DeleteAsync<TDocument, TKey>(
                TDocument document,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var result = await GetCollection<TDocument, TKey>(document, partitionKey)
                .FindOneAndDeleteAsync(d => d.Id.Equals(document.Id),
                    default, token);

            return result == null ? 0 : 1;
        }

        /// <inheritdoc />
        public async Task<long> DeleteAsync<TDocument, TKey>(
                TKey id,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var result = await GetCollection<TDocument, TKey>(partitionKey)
                .FindOneAndDeleteAsync(d => d.Id.Equals(id), default, token);

            return result == null ? 0 : 1;
        }

        /// <inheritdoc />
        public async Task<long> DeleteManyAsync<TDocument, TKey>(
                IEnumerable<TDocument> documents,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var list = documents.ToList();
            if (!list.Any())
                return 0;

            var idsToDelete = list.Select(d => d.Id).ToList();
            return (await GetCollection<TDocument, TKey>(list.FirstOrDefault(), partitionKey)
                    .DeleteManyAsync(d => idsToDelete.Contains(d.Id), token))
                .DeletedCount;
        }

        /// <inheritdoc />
        public async Task<long> DeleteManyAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return (await GetCollection<TDocument, TKey>(partitionKey)
                    .DeleteManyAsync(filter, token))
                .DeletedCount;
        }

        /// <inheritdoc />
        public async Task DropCollectionAsync<TDocument, TKey>(
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var collection = GetCollection<TDocument, TKey>(partitionKey);
            await collection.Database.DropCollectionAsync(collection.CollectionNamespace.CollectionName, token);
        }

        #endregion

        #region IRepositoryHelper

        /// <inheritdoc />
        public IMongoCollection<TDocument> GetCollection<TDocument, TKey>(string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return partitionKey.IsEmpty()
                ? ResolvePartition<TDocument, TKey>()
                : ResolvePartition<TDocument, TKey>(partitionKey);
        }

        /// <inheritdoc />
        public IMongoCollection<TDocument> GetCollection<TDocument, TKey>(
                TDocument document,
                string partitionKey = null)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            if (!partitionKey.IsEmpty())
                return ResolvePartition<TDocument, TKey>(partitionKey);

            if (document is IPartitionDocument<TKey> partitionedDocument)
                return ResolvePartition<TDocument, TKey>(partitionedDocument.PartitionKey);

            return ResolvePartition<TDocument, TKey>();
        }

        /// <inheritdoc />
        public IMongoCollection<TDocument> ResolvePartition<TDocument, TKey>(string partitionKey = null)
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