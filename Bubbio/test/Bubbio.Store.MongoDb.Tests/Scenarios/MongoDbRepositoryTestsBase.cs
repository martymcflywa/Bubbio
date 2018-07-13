using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bubbio.Store.MongoDb.Abstractions;
using Bubbio.Store.MongoDb.Models;
using MongoDB.Driver;
using Xunit;

namespace Bubbio.Store.MongoDb.Tests.Scenarios
{
    public abstract class MongoDbRepositoryTestsBase<TDocument, TKey>
        where TDocument : IDocument<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly IMongoDbContext _dbContext;
        private readonly IMongoDbRepository _repository;
        private readonly MongoUrl _url = new MongoUrl("mongodb://localhost/test");

        private TDocument _document;
        private IEnumerable<TDocument> _documents;
        private IFindFluent<TDocument, TDocument> _cursor;
        private bool _foundAny;
        private long _count;

        protected MongoDbRepositoryTestsBase()
        {
            _dbContext = new MongoDbContext(_url);
            _repository = new MongoDbRepository(_dbContext);
        }

        protected async Task RepositoryIsEmpty()
        {
            await _dbContext.DropCollectionAsync<TDocument, TKey>();
        }

        #region Read

        protected async Task RepositoryRetrievesOneById(TKey id) =>
            _document = await _repository.FindAsync<TDocument, TKey>(id);

        protected async Task RepositoryRetrievesOneByFilter(Expression<Func<TDocument, bool>> filter) =>
            _document = await _repository.FindAsync<TDocument, TKey>(filter);

        protected async Task RepositoryRetrievesManyByFilter(Expression<Func<TDocument, bool>> filter) =>
            _documents = await _repository.FindManyAsync<TDocument, TKey>(filter);

        protected void RepositoryRetrievesCursor(Expression<Func<TDocument, bool>> filter) =>
            _cursor = _repository.GetCursor<TDocument, TKey>(filter);

        protected async Task RepositoryRetrievesAny(Expression<Func<TDocument, bool>> filter) =>
            _foundAny = await _repository.AnyAsync<TDocument, TKey>(filter);

        protected async Task RepositoryRetrievesCountByFilter(Expression<Func<TDocument, bool>> filter) =>
            _count = await _repository.CountAsync<TDocument, TKey>(filter);

        protected async Task<long> RepositoryRetrievesCountForCollection() =>
            await _repository.CountAsync<TDocument, TKey>();

        #endregion

        #region Create

        protected async Task RepositoryContains(TDocument document)
        {
            await RepositoryIsEmpty();
            await RepositoryAddsOne(document);
        }

        protected async Task RepositoryContains(IEnumerable<TDocument> documents)
        {
            await RepositoryIsEmpty();
            await RepositoryAddsMany(documents);
        }

        protected async Task RepositoryAddsOne(TDocument document) =>
            await _repository.AddAsync<TDocument, TKey>(document);

        protected async Task RepositoryAddsMany(IEnumerable<TDocument> documents) =>
            await _repository.AddAsync<TDocument, TKey>(documents);

        #endregion

        #region Assert

        protected async Task RepositoryHas(int expected) =>
            Assert.Equal(expected, await RepositoryRetrievesCountForCollection());

        protected void RepositoryHas(TDocument expected) =>
            Assert.True(expected.Id.Equals(_document.Id));

        protected void RepositoryHas(IEnumerable<TDocument> expected)
        {
            var e = expected.ToArray();
            var a = _documents.ToArray();
            Assert.Equal(e.Length, a.Length);

            for (var i = 0; i < e.Length; i++)
            {
                Assert.True(e[i].Id.Equals(a[i].Id));
                Assert.Equal(e[i].GetType(), a[i].GetType());
            }
        }

        protected void RepositoryFoundAny(bool expected) =>
            Assert.Equal(expected, _foundAny);

        protected void RepositoryCounted(long expected) =>
            Assert.Equal(expected, _count);

        #endregion
    }
}