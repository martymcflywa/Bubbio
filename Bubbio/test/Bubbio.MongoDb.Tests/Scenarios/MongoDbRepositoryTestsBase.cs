using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bubbio.Core.Repository;
using Bubbio.MongoDb.Interfaces;
using Bubbio.Tests.Core;
using FluentAssertions;
using MongoDB.Driver;

namespace Bubbio.MongoDb.Tests.Scenarios
{
    public abstract class MongoDbRepositoryTestsBase<TDocument, TKey, TProject>
        where TDocument : IPartitionDocument<TKey>
        where TKey : IEquatable<TKey>
        where TProject : class
    {
        private readonly IMongoDbRepository _repository;
        private readonly MongoUrl _url = new MongoUrl(TestConstants.MongoDbUrl);
        private readonly string _partitionKey;

        private TDocument _document;
        private IEnumerable<TDocument> _documents;
        private TProject _projectedDocument;
        private IEnumerable<TProject> _projectedDocuments;
        private bool _foundAny;
        private long _count;
        private long _deleted;

        protected MongoDbRepositoryTestsBase(string partitionKey = null)
        {
            _repository = new MongoDbRepository(new MongoDbContext(_url));
            _partitionKey = partitionKey;
        }

        protected async Task RepositoryIsEmpty()
        {
            await _repository.DropCollectionAsync<TDocument, TKey>(_partitionKey);
        }

        #region Read

        protected async Task RepositoryRetrievesOneById(TKey id) =>
            _document = await _repository.FindAsync<TDocument, TKey>(id, _partitionKey);

        protected async Task RepositoryRetrievesOneByFilter(Expression<Func<TDocument, bool>> filter) =>
            _document = await _repository.FindAsync<TDocument, TKey>(filter, _partitionKey);

        protected async Task RepositoryRetrievesManyByFilter(Expression<Func<TDocument, bool>> filter) =>
            _documents = await _repository.FindManyAsync<TDocument, TKey>(filter, _partitionKey);

        protected async Task RepositoryRetrievesPaginated(
            Expression<Func<TDocument, bool>> filter,
            int skip,
            int take)
        {
            _documents = await _repository.FindManyAsync<TDocument, TKey>(filter, skip, take, _partitionKey);
        }

        protected async Task RepositoryRetrievesAny(Expression<Func<TDocument, bool>> filter) =>
            _foundAny = await _repository.AnyAsync<TDocument, TKey>(filter, _partitionKey);

        protected async Task RepositoryRetrievesCountByFilter(Expression<Func<TDocument, bool>> filter) =>
            _count = await _repository.CountAsync<TDocument, TKey>(filter, _partitionKey);

        private async Task<long> RepositoryRetrievesCountForCollection() =>
            await _repository.CountAsync<TDocument, TKey>(_partitionKey);

        protected async Task RepositoryProjectsOne(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, TProject>> projection)
        {
            _projectedDocument = await _repository.ProjectAsync<TDocument, TKey, TProject>(
                filter, projection, _partitionKey);
        }

        protected async Task RepositoryProjectsMany(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, TProject>> projection)
        {
            _projectedDocuments = await _repository.ProjectManyAsync<TDocument, TKey, TProject>(
                filter, projection, _partitionKey);
        }

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

        #region Update

        protected async Task RepositoryIsUpdatedBy(TDocument updated)
        {
            await _repository.UpdateAsync<TDocument, TKey>(updated);
            _document = await _repository.FindAsync<TDocument, TKey>(updated.Id, _partitionKey);
        }

        protected async Task RepositoryIsUpdatedBy<TField>(
            TDocument toUpdate,
            Expression<Func<TDocument, TField>> field,
            TField value)
        {
            await _repository.UpdateAsync<TDocument, TKey, TField>(toUpdate, field, value);
            _document = await _repository.FindAsync<TDocument, TKey>(toUpdate.Id, _partitionKey);
        }

        protected async Task RepositoryIsUpdatedBy<TField>(
            Expression<Func<TDocument, bool>> filter,
            Expression<Func<TDocument, TField>> field,
            TField value)
        {
            await _repository.UpdateAsync<TDocument, TKey, TField>(filter, field, value);
            _document = await _repository.FindAsync<TDocument, TKey>(filter, _partitionKey);
        }

        #endregion

        #region Delete

        protected async Task RepositoryDeletesOne(TDocument document)
        {
            _deleted = await _repository.DeleteAsync<TDocument, TKey>(document);
        }

        protected async Task RepositoryDeletesMany(IEnumerable<TDocument> documents)
        {
            _deleted = await _repository.DeleteManyAsync<TDocument, TKey>(documents);
        }

        protected async Task RepositoryDeletesMany(Expression<Func<TDocument, bool>> filter)
        {
            _deleted = await _repository.DeleteManyAsync<TDocument, TKey>(filter, _partitionKey);
        }

        #endregion

        #region Assert

        protected async Task RepositoryHas(long expected) =>
            (await RepositoryRetrievesCountForCollection())
                .Should().Be(expected);

        // TODO: this is brittle for update tests, feed it a better expected.
        protected void RepositoryHas(TDocument expected) =>
            _document.Id.Should().BeEquivalentTo(expected.Id);

        protected void RepositoryHas(IEnumerable<TDocument> expected) =>
            _documents.Should().BeEquivalentTo(expected);

        protected void RepositoryFoundAny(bool expected) =>
            _foundAny.Should().Be(expected);

        protected void RepositoryCounted(long expected) =>
            _count.Should().Be(expected);

        protected void RepositoryDeleted(long expected) =>
            _deleted.Should().Be(expected);

        protected void DocumentIsProjected(TProject expected) =>
            _projectedDocument.Should().BeEquivalentTo(expected);

        protected void DocumentsAreProjected(IEnumerable<TProject> expected) =>
            _projectedDocuments.Should().BeEquivalentTo(expected);

        #endregion
    }
}