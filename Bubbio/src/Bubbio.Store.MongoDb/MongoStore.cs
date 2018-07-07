using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
            var client = new MongoClient(Settings(url));
            var database = client.GetDatabase(url.DatabaseName);
            _collection = database.GetCollection<TEntity>(collectionName);
        }

        private static MongoClientSettings Settings(MongoUrl url) =>
            new MongoClientSettings
            {
                ApplicationName = Assembly.GetCallingAssembly().GetName().Name,
                GuidRepresentation = GuidRepresentation.Standard,
                Server = url.Server
            };

        #region IStore<TEntity, TKey>

        public async Task InsertAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            await _collection.InsertManyAsync(entities);
        }

        public async Task<IEnumerable<TEntity>> GetAsync(TKey id)
        {
            var result = await _collection.FindAsync(e => e.Id.Equals(id));
            return result.ToEnumerable();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(TEntity entity)
        {
            var result = await _collection.FindAsync(
                e => e.Id.Equals(entity.Id));

            return result.ToEnumerable();
        }

        public Task<IEnumerable<TEntity>> GetAsync(Func<TEntity, bool> predicate)
        {
            return Task.FromResult(_collection.AsQueryable()
                .Where(predicate)
                .AsEnumerable());
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

        public IEnumerator<TEntity> GetEnumerator() => _collection.AsQueryable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public Type ElementType => _collection.AsQueryable().ElementType;
        public Expression Expression => _collection.AsQueryable().Expression;
        public IQueryProvider Provider => _collection.AsQueryable().Provider;

        #endregion

    }
}