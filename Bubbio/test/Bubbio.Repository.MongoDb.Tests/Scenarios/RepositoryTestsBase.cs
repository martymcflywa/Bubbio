using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Bubbio.Core.Repository;
using Bubbio.MongoDb.Interfaces;
using FluentAssertions;

namespace Bubbio.Repository.MongoDb.Tests.Scenarios
{
    public abstract class RepositoryTestsBase<TDocument, TKey, TProject>
        where TDocument : IDocument<TKey>
        where TKey : IEquatable<TKey>
        where TProject : class
    {
        private readonly IRepository<TDocument, TKey> _repository;
        private TDocument _document;
        private IEnumerable<TDocument> _documents;
        private TProject _projectedDocument;
        private IEnumerable<TProject> _projectedDocuments;
        private bool _foundAny;
        private long _count;
        private long _deleted;

        protected RepositoryTestsBase(IMongoDbRepository mongoDb, string partitionKey)
        {
            _repository = new Repository<TDocument, TKey>(mongoDb, partitionKey);
        }

        protected async Task RepositoryIsEmpty()
        {
            await _repository.DropAsync();
        }

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
            await _repository.InsertAsync(document);

        protected async Task RepositoryAddsMany(IEnumerable<TDocument> documents) =>
            await _repository.InsertManyAsync(documents);

        #endregion

        #region Read

        protected async Task RepositoryGetsOneById(TKey id) =>
            _document = await _repository.GetAsync(id);

        protected async Task RepositoryGetsOneByPredicate(Expression<Func<TDocument, bool>> predicate) =>
            _document = await _repository.GetAsync(predicate);

        protected async Task RepositoryGetsMany(Expression<Func<TDocument, bool>> predicate) =>
            _documents = await _repository.GetManyAsync(predicate, default(CancellationToken));

        protected async Task RepositoryGetsPaginatedMany(Expression<Func<TDocument, bool>> predicate,
                int skip, int take) =>
            _documents = await _repository.GetManyAsync(predicate, skip, take);

        protected async Task RepositoryFindsAny(Expression<Func<TDocument, bool>> predicate) =>
            _foundAny = await _repository.AnyAsync(predicate);

        protected async Task RepositoryGetsCountForCollection() =>
            _count = await _repository.CountAsync();

        protected async Task RepositoryGetsCountByFilter(Expression<Func<TDocument, bool>> predicate) =>
            _count = await _repository.CountAsync(predicate);

        protected async Task RepositoryProjectsOne(Expression<Func<TDocument, bool>> predicate,
                Expression<Func<TDocument, TProject>> projection) =>
            _projectedDocument = await _repository.ProjectAsync(predicate, projection);

        protected async Task RepositoryProjectsMany(Expression<Func<TDocument, bool>> predicate,
                Expression<Func<TDocument, TProject>> projection) =>
            _projectedDocuments = await _repository.ProjectManyAsync(predicate, projection);

        #endregion

        #region Update

        protected async Task RepositoryIsUpdatedBy(TDocument updated)
        {
            await _repository.UpdateAsync(updated);
            _document = await _repository.GetAsync(updated);
        }

        protected async Task RepositoryIsUpdatedBy<TField>(TDocument toUpdate,
            Expression<Func<TDocument, TField>> selector, TField value)
        {
            await _repository.UpdateAsync(toUpdate, selector, value);
            _document = await _repository.GetAsync(toUpdate);
        }

        protected async Task RepositoryIsUpdatedBy<TField>(Expression<Func<TDocument, bool>> predicate,
            Expression<Func<TDocument, TField>> selector, TField value)
        {
            await _repository.UpdateAsync(predicate, selector, value);
            _document = await _repository.GetAsync(predicate);
        }

        #endregion

        #region Delete

        protected async Task RepositoryDeletesOne(TDocument document) =>
            _deleted = await _repository.DeleteAsync(document);

        protected async Task RepositoryDeletesOne(TKey id) =>
            _deleted = await _repository.DeleteAsync(id);

        protected async Task RepositoryDeletesOne(Expression<Func<TDocument, bool>> predicate) =>
            _deleted = await _repository.DeleteAsync(predicate);

        protected async Task RepositoryDeletesMany(IEnumerable<TDocument> documents) =>
            _deleted = await _repository.DeleteManyAsync(documents);

        protected async Task RepositoryDeletesMany(Expression<Func<TDocument, bool>> predicate) =>
            _deleted = await _repository.DeleteManyAsync(predicate);

        #endregion

        #region Assert

        protected async Task RepositoryHas(long expected)
        {
            await RepositoryGetsCountForCollection();
            _count.Should().Be(expected);
        }

        protected void DocumentIsFound(TDocument expected) =>
            _document.Should().BeEquivalentTo(expected);

        protected void DocumentsAreFound(IEnumerable<TDocument> expected) =>
            _documents.Should().BeEquivalentTo(expected);

        protected void DocumentIsNotFound() =>
            _document.Should().BeNull();

        protected void DocumentsAreNotFound() =>
            _documents.Should().BeEmpty();

        protected void RepositoryFoundSomething(bool expected) =>
            _foundAny.Should().Be(expected);

        protected void RepositoryCounted(long expected) =>
            _count.Should().Be(expected);

        protected void RepositoryDeleted(long expected) =>
            _deleted.Should().Be(expected);

        protected void DocumentIsProjected(TProject expected) =>
            _projectedDocument.Should().BeEquivalentTo(expected);

        protected void DocumentNotProjected() =>
            _projectedDocument.Should().BeNull();

        protected void DocumentsAreProjected(IEnumerable<TProject> expected) =>
            _projectedDocuments.Should().BeEquivalentTo(expected);

        protected void DocumentsNotProjected() =>
            _projectedDocuments.Should().BeEmpty();

        protected void DocumentIsUpdated(TDocument expected) =>
            _document.Should().BeEquivalentTo(expected);

        protected void DocumentNotUpdated() =>
            _document.Should().BeNull();

        #endregion
    }
}