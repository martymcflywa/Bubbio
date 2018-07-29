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

        #region Setup

        protected async Task RepositoryIsEmpty()
        {
            await _repository.DropAsync();
        }

        protected async Task RepositoryContains(TDocument document)
        {
            await RepositoryIsEmpty();
            await InsertOne(document);
        }

        protected async Task RepositoryContains(IEnumerable<TDocument> documents)
        {
            await RepositoryIsEmpty();
            await InsertMany(documents);
        }

        #endregion

        #region Create

        protected async Task InsertOne(TDocument document) =>
            await _repository.InsertAsync(document);

        protected async Task InsertMany(IEnumerable<TDocument> documents) =>
            await _repository.InsertManyAsync(documents);

        #endregion

        #region Read

        protected async Task GetOne(TKey id)
        {
            try
            {
                _document = await _repository.GetAsync(id);
            }
            catch (InvalidOperationException) {}
        }

        protected async Task GetOne(TDocument document)
        {
            try
            {
                _document = await _repository.GetAsync(document);
            }
            catch (InvalidOperationException) {}
        }

        protected async Task GetOne(Expression<Func<TDocument, bool>> predicate)
        {
            try
            {
                _document = await _repository.GetAsync(predicate);
            }
            catch (InvalidOperationException) {}
        }

        protected async Task GetMany(Expression<Func<TDocument, bool>> predicate) =>
            _documents = await _repository.GetManyAsync(predicate, default(CancellationToken));

        protected async Task GetMany(Expression<Func<TDocument, bool>> predicate,
                int skip, int take) =>
            _documents = await _repository.GetManyAsync(predicate, skip, take);

        protected async Task Any(Expression<Func<TDocument, bool>> predicate) =>
            _foundAny = await _repository.AnyAsync(predicate);

        protected async Task Count() =>
            _count = await _repository.CountAsync();

        protected async Task Count(Expression<Func<TDocument, bool>> predicate) =>
            _count = await _repository.CountAsync(predicate);

        protected async Task ProjectOne(Expression<Func<TDocument, bool>> predicate,
            Expression<Func<TDocument, TProject>> projection)
        {
            try
            {
                _projectedDocument = await _repository.ProjectAsync(predicate, projection);
            }
            catch (InvalidOperationException) {}
        }

        protected async Task ProjectMany(Expression<Func<TDocument, bool>> predicate,
                Expression<Func<TDocument, TProject>> projection) =>
            _projectedDocuments = await _repository.ProjectManyAsync(predicate, projection);

        #endregion

        #region Update

        protected async Task UpdateOne(TDocument updated)
        {
            await _repository.UpdateAsync(updated);
            _document = await _repository.GetAsync(updated);
        }

        protected async Task UpdateOne<TField>(TDocument toUpdate,
            Expression<Func<TDocument, TField>> selector, TField value)
        {
            try
            {
                await _repository.UpdateAsync(toUpdate, selector, value);
                _document = await _repository.GetAsync(toUpdate);
            }
            catch (InvalidOperationException) {}
        }

        protected async Task UpdateOne<TField>(Expression<Func<TDocument, bool>> predicate,
            Expression<Func<TDocument, TField>> selector, TField value)
        {
            try
            {
                await _repository.UpdateAsync(predicate, selector, value);
                _document = await _repository.GetAsync(predicate);
            }
            catch (InvalidOperationException) {}
        }

        #endregion

        #region Delete

        protected async Task DeleteOne(TKey id) =>
            _deleted = await _repository.DeleteAsync(id);

        protected async Task DeleteOne(TDocument document) =>
            _deleted = await _repository.DeleteAsync(document);

        protected async Task DeleteMany(IEnumerable<TDocument> documents) =>
            _deleted = await _repository.DeleteManyAsync(documents);

        protected async Task DeleteMany(Expression<Func<TDocument, bool>> predicate) =>
            _deleted = await _repository.DeleteManyAsync(predicate);

        #endregion

        #region Assert

        protected async Task RepositoryHas(long expected)
        {
            await Count();
            _count.Should().Be(expected);
        }

        protected void DocumentFound(TDocument expected) =>
            _document.Should().BeEquivalentTo(expected);

        protected void DocumentsFound(IEnumerable<TDocument> expected) =>
            _documents.Should().BeEquivalentTo(expected);

        protected void DocumentNotFound() =>
            _document.Should().BeNull();

        protected void DocumentsNotFound() =>
            _documents.Should().BeEmpty();

        protected void FoundAny(bool expected) =>
            _foundAny.Should().Be(expected);

        protected void Counted(long expected) =>
            _count.Should().Be(expected);

        protected void Deleted(long expected) =>
            _deleted.Should().Be(expected);

        protected void OneProjected(TProject expected) =>
            _projectedDocument.Should().BeEquivalentTo(expected);

        protected void OneNotProjected() =>
            _projectedDocument.Should().BeNull();

        protected void ManyProjected(IEnumerable<TProject> expected) =>
            _projectedDocuments.Should().BeEquivalentTo(expected);

        protected void ManyNotProjected() =>
            _projectedDocuments.Should().BeEmpty();

        protected void DocumentUpdated(TDocument expected) =>
            _document.Should().BeEquivalentTo(expected, config => config.Excluding(d => d.Modified));

        protected void DocumentNotUpdated() =>
            _document.Should().BeNull();

        #endregion
    }
}