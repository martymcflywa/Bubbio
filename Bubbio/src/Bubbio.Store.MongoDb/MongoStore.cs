using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bubbio.Core.Store;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bubbio.Store.MongoDb
{
    public class MongoStore<TEntity, TKey> : IStore<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {

        private readonly IMongoCollection<TEntity> _collection;

        public MongoStore(MongoUrl url, string collectionName)
        {
            var client = new MongoClient(url);
            var database = client.GetDatabase(url.DatabaseName);
            _collection = database.GetCollection<TEntity>(collectionName);
        }

        #region IStore<TEntity, TKey>

        public async Task InsertAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            await _collection.InsertManyAsync(entities);
        }

        public Task<IEnumerable<TEntity>> GetAsync(TKey id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountAsync()
        {
            return _collection.CountDocumentsAsync(new BsonDocument());
        }

        public async Task DeleteAllAsync()
        {
            await _collection.DeleteManyAsync(new BsonDocument());
        }

        #endregion

        #region IQueryable<TKey>

        public IEnumerator<TEntity> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType { get; }
        public Expression Expression { get; }
        public IQueryProvider Provider { get; }

        #endregion

    }
}