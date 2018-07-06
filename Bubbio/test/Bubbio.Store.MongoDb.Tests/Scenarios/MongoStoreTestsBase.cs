using System.Collections.Generic;
using System.Linq;
using Bubbio.Core.Store;
using MongoDB.Driver;
using Xunit;

namespace Bubbio.Store.MongoDb.Tests.Scenarios
{
    public class MongoStoreTestsBase<TEntity, TKey>
        where TEntity : IEntity<TKey>
    {
        private IStore<TEntity, TKey> _store;
        private TEntity _entity;
        private IEnumerable<TEntity> _entities;

        private readonly MongoUrl _url = new MongoUrl("mongodb://localhost/test");
        private const string CollectionName = "test";

        protected void StoreIsEmpty()
        {
            InitCollection();
        }

        protected void StoreContains(TEntity entity)
        {
            InitCollection();
            StoreInserts(entity);
        }

        protected void StoreInserts(TEntity entity) =>
            _store.InsertAsync(entity).Wait();

        protected void StoreInserts(IEnumerable<TEntity> entities) =>
            _store.InsertAsync(entities).Wait();

        protected void StoreRetrievesOne(TKey id) =>
            _entity = _store.GetAsync(id).Result.SingleOrDefault();

        protected void StoreRetrievesOne(TEntity entity) =>
            _entity = _store.GetAsync(entity).Result.SingleOrDefault();

        protected void StoreRetrievesMany(TKey id) =>
            _entities = _store.GetAsync(id).Result;

        protected void StoreHas(long count) =>
            Assert.Equal(count, _store.CountAsync().Result);

        protected void StoreHas(TEntity expected) =>
            Assert.True(expected.Id.Equals(_entity.Id));

        protected void StoreHas(IEnumerable<TEntity> expected) =>
            Assert.Equal(expected, _entities);

        protected void EntityExists() =>
            Assert.True(_entity != null);

        protected void EntitiesExist() =>
            Assert.NotEmpty(_entities);

        private void Clear() =>
            _store.DeleteAllAsync().Wait();

        protected void InitCollection()
        {
            _store = new MongoStore<TEntity, TKey>(_url, CollectionName);
            Clear();
        }
    }
}