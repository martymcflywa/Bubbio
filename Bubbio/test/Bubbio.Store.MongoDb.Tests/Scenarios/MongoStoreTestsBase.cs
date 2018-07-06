using System;
using System.Collections.Generic;
using System.Linq;
using Bubbio.Core.Contracts;
using Bubbio.Core.Store;
using Bubbio.Store.MongoDb.Tests.Examples;
using MongoDB.Driver;
using Xunit;

namespace Bubbio.Store.MongoDb.Tests.Scenarios
{
    public class MongoStoreTestsBase<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        private IStore<TEntity, TKey> _collection;
        private TEntity _entity;
        private IEnumerable<TEntity> _entities;

        protected void StoreIsEmpty()
        {
            InitCollection();
        }

        protected void StoreInserts(TEntity entity) =>
            _collection.InsertAsync(entity).Wait();

        protected void StoreInserts(IEnumerable<TEntity> entities) =>
            _collection.InsertAsync(entities).Wait();

        protected void StoreRetrievesOne(TKey id) =>
            _entity = _collection.GetAsync(id).Result.SingleOrDefault();

        protected void StoreRetrievesOne(TEntity entity) =>
            _entity = _collection.GetAsync(entity).Result.SingleOrDefault();

        protected void StoreRetrievesMany(TKey id) =>
            _entities = _collection.GetAsync(id).Result;

        protected void StoreHas(long count) =>
            Assert.Equal(count, _collection.CountAsync().Result);

        protected void EntityExists() =>
            Assert.True(_entity != null);

        protected void EntitiesExist() =>
            Assert.NotEmpty(_entities);

        private void Clear() =>
            _collection.DeleteAllAsync().Wait();

        protected void InitCollection()
        {
            _collection = new MongoStore<TEntity, TKey>(new MongoUrl("mongodb://localhost/test"), "test");
            Clear();
        }
    }
}