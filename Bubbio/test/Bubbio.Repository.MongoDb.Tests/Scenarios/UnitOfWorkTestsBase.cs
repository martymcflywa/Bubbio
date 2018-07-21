using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Bubbio.Core.Exceptions;
using Bubbio.Core.Repository;
using Bubbio.Tests.Core.Examples;
using FluentAssertions;

namespace Bubbio.Repository.MongoDb.Tests.Scenarios
{
    public abstract class UnitOfWorkTestsBase<TDocument, TKey>
        where TDocument : IDocument<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly IRepository<TDocument, TKey> _repository;
        private readonly IUnitOfWork<TDocument, TKey> _unitOfWork;

        private TDocument _document;
        private IEnumerable<TDocument> _documents;
        private TestProjection _projection;
        private IEnumerable<TestProjection> _projections;
        private long _count;
        private bool _updated;
        private long _deleted;

        protected UnitOfWorkTestsBase(IRepository<TDocument, TKey> repository, IUnitOfWork<TDocument, TKey> unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        protected async Task RepositoryIsEmpty()
        {
            await _repository.DropAsync();
        }

        protected async Task RepositoryContains(TDocument document)
        {
            await RepositoryIsEmpty();
            await _repository.InsertAsync(document);
        }

        protected async Task RepositoryContains(IEnumerable<TDocument> documents)
        {
            await RepositoryIsEmpty();
            await _repository.InsertManyAsync(documents);
        }

        #region Create

        protected async Task SaveOne(TDocument document) =>
            await _unitOfWork.SaveAsync(document);

        protected async Task SaveMany(IEnumerable<TDocument> documents) =>
            await _unitOfWork.SaveAsync(documents);

        #endregion

        #region Read

        protected async Task GetOne(TKey id)
        {
            try
            {
                _document = await _unitOfWork.GetAsync(id);
            }
            catch (DocumentNotFoundException<TKey>) {}
        }

        protected async Task GetOne(TDocument document)
        {
            try
            {
                _document = await _unitOfWork.GetAsync(document);
            }
            catch (DocumentNotFoundException<TKey>) {}
        }

        protected async Task GetOne(Expression<Func<TDocument, bool>> predicate)
        {
            try
            {
                _document = await _unitOfWork.GetAsync(predicate);
            }
            catch (DocumentNotFoundException<TKey>) {}
            catch (InvalidOperationException) {}
        }

        protected async Task GetMany(Expression<Func<TDocument, bool>> predicate)
        {
            try
            {
                _documents = await _unitOfWork.GetManyAsync(predicate, default(CancellationToken));
            }
            catch (DocumentNotFoundException<TKey>) {}
        }

        protected async Task GetManyPaged(Expression<Func<TDocument, bool>> predicate, int skip, int take)
        {
            try
            {
                _documents = await _unitOfWork.GetManyAsync(predicate, skip, take);
            }
            catch (DocumentNotFoundException<TKey>) {}
        }

        protected async Task Count() =>
            _count = await _unitOfWork.CountAsync();

        protected async Task Count(Expression<Func<TDocument, bool>> predicate) =>
            _count = await _unitOfWork.CountAsync(predicate);

        protected async Task ProjectOne(Expression<Func<TDocument, bool>> predicate,
            Expression<Func<TDocument, TestProjection>> projection)
        {
            try
            {
                _projection = await _unitOfWork.ProjectAsync(predicate, projection);
            }
            catch (DocumentNotFoundException<TKey>) {}
            catch (InvalidOperationException) {}
        }

        protected async Task ProjectMany(Expression<Func<TDocument, bool>> predicate,
            Expression<Func<TDocument, TestProjection>> projection)
        {
            try
            {
                _projections = await _unitOfWork.ProjectManyAsync(predicate, projection);
            }
            catch (DocumentNotFoundException<TKey>) {}
        }

        #endregion

        #region Update

        protected async Task Update(TDocument updated)
        {
            _updated = await _unitOfWork.UpdateAsync(updated);
            await GetOne(updated);
        }

        protected async Task Update<TField>(TDocument toUpdate, Expression<Func<TDocument, TField>> selector,
            TField value)
        {
            _updated = await _unitOfWork.UpdateAsync(toUpdate, selector, value);
            await GetOne(toUpdate);
        }

        protected async Task Update<TField>(Expression<Func<TDocument, bool>> predicate,
            Expression<Func<TDocument, TField>> selector, TField value)
        {
            try
            {
                _updated = await _unitOfWork.UpdateAsync(predicate, selector, value);
                await GetOne(predicate);
            }
            catch (ManyDocumentsFoundException) {}
        }

        #endregion

        #region Delete

        protected async Task Delete(TKey id)
        {
            try
            {
                _deleted = await _unitOfWork.DeleteAsync(id);
            }
            catch (DocumentNotFoundException<TKey>) {}
        }

        protected async Task Delete(TDocument document)
        {
            try
            {
                _deleted = await _unitOfWork.DeleteAsync(document);
            }
            catch (DocumentNotFoundException<TKey>) {}
        }

        protected async Task Delete(Expression<Func<TDocument, bool>> predicate)
        {
            try
            {
                _deleted = await _unitOfWork.DeleteAsync(predicate);
            }
            catch (DocumentNotFoundException<TKey>) {}
            catch (ManyDocumentsFoundException) {}
        }

        protected async Task DeleteMany(IEnumerable<TDocument> documents)
        {
            try
            {
                _deleted = await _unitOfWork.DeleteManyAsync(documents);
            }
            catch (DocumentNotFoundException<TKey>) {}
        }

        protected async Task DeleteMany(Expression<Func<TDocument, bool>> predicate)
        {
            try
            {
                _deleted = await _unitOfWork.DeleteManyAsync(predicate);
            }
            catch (DocumentNotFoundException<TKey>) {}
        }

        #endregion

        #region Assert

        protected async Task RepositoryHas(long expected)
        {
            await Count();
            _count.Should().Be(expected);
        }

        protected async Task RepositoryHas(TDocument expected) =>
            (await _repository.GetAsync(expected))
                .Should().BeEquivalentTo(expected);

        protected async Task RepositoryHas(Expression<Func<TDocument, bool>> predicate) =>
            (await _unitOfWork.AnyAsync(predicate))
                .Should().Be(true);

        protected void DocumentIsFound(TDocument expected) =>
            _document.Should().BeEquivalentTo(expected);

        protected void DocumentNotFound() =>
            _document.Should().BeNull();

        protected void DocumentsAreFound(IEnumerable<TDocument> expected) =>
            _documents.Should().BeEquivalentTo(expected);

        protected void DocumentsNotFound() =>
            _documents.Should().BeNullOrEmpty();

        protected void DocumentsCounted(long expected) =>
            _count.Should().Be(expected);

        protected void DocumentProjected(TestProjection expected) =>
            _projection.Should().BeEquivalentTo(expected);

        protected void DocumentNotProjected() =>
            _projection.Should().BeNull();

        protected void DocumentsProjected(IEnumerable<TestProjection> expected) =>
            _projections.Should().BeEquivalentTo(expected);

        protected void DocumentsNotProjected() =>
            _projections.Should().BeNullOrEmpty();

        protected void DocumentUpdated(bool expected) =>
            _updated.Should().Be(expected);

        protected void DocumentsDeleted(long expected) =>
            _deleted.Should().Be(expected);

        #endregion
    }
}