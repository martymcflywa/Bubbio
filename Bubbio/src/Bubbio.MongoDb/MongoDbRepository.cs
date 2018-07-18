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
    /// <inheritdoc cref="IRepository" />
    /// <inheritdoc cref="IRepositoryHelper" />
    /// <summary>
    /// Implementation of IRepository for MongoDb.
    /// </summary>
    public class MongoDbRepository : IRepository, IRepositoryHelper
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
            return await GetCollection<TDocument, TKey>(partitionKey)
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
            return await GetCollection<TDocument, TKey>(partitionKey)
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
        public async Task<TProject> ProjectOneAsync<TDocument, TKey, TProject>(
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
                .FirstOrDefaultAsync(token);
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
        public async Task AddAsync<TDocument, TKey>(TDocument document, CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            await GetCollection<TDocument, TKey>(document)
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

            await GetCollection<TDocument, TKey>(enumerable.FirstOrDefault())
                .InsertManyAsync(enumerable, cancellationToken: token);
        }

        #endregion

        #region Update

        /// <inheritdoc />
        public async Task<bool> UpdateOneAsync<TDocument, TKey>(
                TDocument updated,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var result = await GetCollection<TDocument, TKey>(updated)
                .ReplaceOneAsync(
                    d => d.Id.Equals(updated.Id),
                    updated,
                    new UpdateOptions {IsUpsert = true},
                    token);

            return result.ModifiedCount.Equals(1);
        }

        /// <inheritdoc />
        public async Task<bool> UpdateOneAsync<TDocument, TKey, TField>(
                TDocument toUpdate,
                Expression<Func<TDocument, TField>> selector,
                TField value,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var task = Builders<TDocument>.Update.Set(selector, value);
            var result = await GetCollection<TDocument, TKey>(toUpdate)
                .UpdateOneAsync(d => d.Id.Equals(toUpdate.Id), task, cancellationToken: token);

            return result.ModifiedCount.Equals(1);
        }

        /// <inheritdoc />
        public async Task<bool> UpdateOneAsync<TDocument, TKey, TField>(
                Expression<Func<TDocument, bool>> filter,
                Expression<Func<TDocument, TField>> selector,
                TField value,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var task = Builders<TDocument>.Update.Set(selector, value);
            var result = await GetCollection<TDocument, TKey>(partitionKey)
                .UpdateOneAsync(filter, task, cancellationToken: token);

            return result.ModifiedCount.Equals(1);
        }

        #endregion

        #region Delete

        /// <inheritdoc />
        public async Task<long> DeleteOneAsync<TDocument, TKey>(
                TDocument document,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return (await GetCollection<TDocument, TKey>(document)
                    .DeleteOneAsync(d => d.Id.Equals(document.Id), token))
                .DeletedCount;
        }

        /// <inheritdoc />
        public async Task<long> DeleteOneAsync<TDocument, TKey>(
                Expression<Func<TDocument, bool>> filter,
                string partitionKey = null,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            return (await GetCollection<TDocument, TKey>(partitionKey)
                    .DeleteOneAsync(filter, token))
                .DeletedCount;
        }

        /// <inheritdoc />
        public async Task<long> DeleteManyAsync<TDocument, TKey>(
                IEnumerable<TDocument> documents,
                CancellationToken token = default)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
            var enumerable = documents.ToList();
            if (!enumerable.Any())
                return 0;

            var idsToDelete = enumerable.Select(d => d.Id).ToList();
            return (await GetCollection<TDocument, TKey>(enumerable.FirstOrDefault())
                    .DeleteManyAsync(d => idsToDelete.Contains(d.Id), token))
                .DeletedCount;
        }

        /// <inheritdoc />
        public async Task<long> DeleteManyAsync<TDocument, TKey>(Expression<Func<TDocument, bool>> filter, string partitionKey = null, CancellationToken token = default) where TDocument : IDocument<TKey> where TKey : IEquatable<TKey>
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
        public IMongoCollection<TDocument> GetCollection<TDocument, TKey>(TDocument document)
            where TDocument : IDocument<TKey>
            where TKey : IEquatable<TKey>
        {
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