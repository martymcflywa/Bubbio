using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Bubbio.Core.Repository;
using Bubbio.MongoDb.Interfaces;

namespace Bubbio.Repository.MongoDb
{
    public class Repository<TDocument, TKey> : IRepository<TDocument, TKey>
        where TDocument : IDocument<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly IMongoDbRepository _mongoDb;
        private readonly string _partitionKey;

        public Repository(IMongoDbRepository mongoDb, string partitionKey = null)
        {
            _mongoDb = mongoDb;
            _partitionKey = partitionKey;
        }

        #region Create

        public async Task InsertAsync(TDocument entity, CancellationToken token = default)
        {
            await _mongoDb.AddAsync<TDocument, TKey>(entity, _partitionKey, token);
        }

        public async Task InsertManyAsync(IEnumerable<TDocument> entities, CancellationToken token = default)
        {
            await _mongoDb.AddAsync<TDocument, TKey>(entities, _partitionKey, token);
        }

        #endregion

        #region Read

        public async Task<TDocument> GetAsync(TKey id, CancellationToken token = default)
        {
            return await _mongoDb.FindAsync<TDocument, TKey>(id, _partitionKey, token);
        }

        public async Task<TDocument> GetAsync(TDocument document, CancellationToken token = default)
        {
            return await GetAsync(document.Id, token);
        }

        public async Task<TDocument> GetAsync(Expression<Func<TDocument, bool>> predicate,
            CancellationToken token = default)
        {
            return await _mongoDb.FindAsync<TDocument, TKey>(predicate, _partitionKey, token);
        }

        public async Task<IEnumerable<TDocument>> GetManyAsync(Expression<Func<TDocument, bool>> predicate,
            CancellationToken token = default)
        {
            return await _mongoDb.FindManyAsync<TDocument, TKey>(predicate, _partitionKey, token);
        }

        public async Task<IEnumerable<TDocument>> GetManyAsync(Expression<Func<TDocument, bool>> predicate,
            int skip = 0, int take = 50, CancellationToken token = default)
        {
            return await _mongoDb.FindManyAsync<TDocument, TKey>(predicate, skip, take, _partitionKey, token);
        }

        public async Task<bool> AnyAsync(Expression<Func<TDocument, bool>> filter, CancellationToken token = default)
        {
            return await _mongoDb.AnyAsync<TDocument, TKey>(filter, _partitionKey, token);
        }

        public async Task<long> CountAsync(CancellationToken token = default)
        {
            return await _mongoDb.CountAsync<TDocument, TKey>(_partitionKey, token);
        }

        public async Task<long> CountAsync(Expression<Func<TDocument, bool>> predicate,
            CancellationToken token = default)
        {
            return await _mongoDb.CountAsync<TDocument, TKey>(predicate, _partitionKey, token);
        }

        public async Task<TProjection> ProjectAsync<TProjection>(Expression<Func<TDocument, bool>> predicate,
                Expression<Func<TDocument, TProjection>> projection, CancellationToken token = default)
            where TProjection : class
        {
            return await _mongoDb.ProjectAsync<TDocument, TKey, TProjection>(predicate, projection, _partitionKey,
                token);
        }

        public async Task<IEnumerable<TProjection>> ProjectManyAsync<TProjection>(
                Expression<Func<TDocument, bool>> predicate, Expression<Func<TDocument, TProjection>> projection,
                CancellationToken token = default)
            where TProjection : class
        {
            return await _mongoDb.ProjectManyAsync<TDocument, TKey, TProjection>(predicate, projection, _partitionKey,
                token);
        }

        #endregion

        #region Update

        public async Task<bool> UpdateAsync(TDocument updated, CancellationToken token = default)
        {
            return await _mongoDb.UpdateAsync<TDocument, TKey>(updated, _partitionKey, token);
        }

        public async Task<bool> UpdateAsync<TField>(TDocument toUpdate, Expression<Func<TDocument, TField>> selector,
            TField value, CancellationToken token = default)
        {
            return await _mongoDb.UpdateAsync<TDocument, TKey, TField>(toUpdate, selector, value, _partitionKey, token);
        }

        public async Task<bool> UpdateAsync<TField>(Expression<Func<TDocument, bool>> predicate,
            Expression<Func<TDocument, TField>> selector, TField value, CancellationToken token = default)
        {
            return await _mongoDb.UpdateAsync<TDocument, TKey, TField>(predicate, selector, value, _partitionKey,
                token);
        }

        #endregion

        #region Delete

        public async Task<long> DeleteAsync(TDocument entity, CancellationToken token = default)
        {
            return await _mongoDb.DeleteAsync<TDocument, TKey>(entity, _partitionKey, token);
        }

        public async Task<long> DeleteAsync(TKey id, CancellationToken token = default)
        {
            return await _mongoDb.DeleteAsync<TDocument, TKey>(id, _partitionKey, token);
        }

        public async Task<long> DeleteManyAsync(IEnumerable<TDocument> entities, CancellationToken token = default)
        {
            return await _mongoDb.DeleteManyAsync<TDocument, TKey>(entities, _partitionKey, token);
        }

        public async Task<long> DeleteManyAsync(Expression<Func<TDocument, bool>> predicate,
            CancellationToken token = default)
        {
            return await _mongoDb.DeleteManyAsync<TDocument, TKey>(predicate, _partitionKey, token);
        }

        public async Task DropAsync(CancellationToken token = default)
        {
            await _mongoDb.DropCollectionAsync<TDocument, TKey>(_partitionKey, token);
        }

        #endregion
    }
}