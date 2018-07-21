using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bubbio.Core.Exceptions;
using Bubbio.Core.Repository;
using Bubbio.MongoDb.Documents.Constants;
using Bubbio.MongoDb.Documents.Entities;
using Bubbio.MongoDb.Documents.Events;
using Bubbio.MongoDb.Interfaces;
using Bubbio.Tests.Core.Examples;
using FluentAssertions;

namespace Bubbio.Repository.MongoDb.Tests.Scenarios
{
    public abstract class ParentUnitOfWorkTestsBase
    {
        private readonly IRepository<Parent, Guid> _parentRepository;
        private readonly UnitOfWork _unitOfWork;
        private Parent _parent;
        private IEnumerable<Parent> _parents;
        private TestProjection _projectedParent;
        private IEnumerable<TestProjection> _projectedParents;
        private long _count;
        private bool _updated;
        private long _deleted;

        protected ParentUnitOfWorkTestsBase(IMongoDbRepository mongoDb)
        {
            _parentRepository = new Repository<Parent, Guid>(mongoDb, Partitions.Parents.ToString());
            _unitOfWork = new UnitOfWork(_parentRepository, default(Repository<Child, Guid>),
                default(Repository<Event, Guid>));
        }

        protected async Task RepositoryIsEmpty()
        {
            await _parentRepository.DropAsync();
        }

        protected async Task RepositoryContains(Parent parent)
        {
            await RepositoryIsEmpty();
            await _parentRepository.InsertAsync(parent);
        }

        protected async Task RepositoryContains(IEnumerable<Parent> parents)
        {
            await RepositoryIsEmpty();
            await _parentRepository.InsertManyAsync(parents);
        }

        #region Create

        protected async Task ParentIsSaved(string first, string last, string middle = null) =>
            await _unitOfWork.SaveParentAsync(first, last, middle);

        protected async Task ParentsAreSaved(IEnumerable<Parent> parents) =>
            await _unitOfWork.SaveParentsAsync(parents);

        #endregion

        #region Read

        protected async Task GetOneParentById(Guid id)
        {
            try
            {
                _parent = await _unitOfWork.GetParentAsync(id);
            }
            catch (DocumentNotFoundException<Guid>) {}
        }

        protected async Task GetOneParentByDocument(Parent parent)
        {
            try
            {
                _parent = await _unitOfWork.GetParentAsync(parent);
            }
            catch (DocumentNotFoundException<Guid>) {}
        }

        protected async Task GetOneParentByPredicate(Expression<Func<Parent, bool>> predicate)
        {
            try
            {
                _parent = await _unitOfWork.GetParentAsync(predicate);
            }
            catch (DocumentNotFoundException<Guid>) {}
            catch (InvalidOperationException) {}
        }

        protected async Task GetManyParents(Expression<Func<Parent, bool>> predicate)
        {
            try
            {
                _parents = await _unitOfWork.GetParentsAsync(predicate);
            }
            catch (DocumentNotFoundException<Guid>) {}
        }

        protected async Task GetManyParentsPaged(Expression<Func<Parent, bool>> predicate, int skip, int take)
        {
            try
            {
                _parents = await _unitOfWork.GetParentsPagedAsync(predicate, skip, take);
            }
            catch (DocumentNotFoundException<Guid>) {}
        }

        protected async Task CountAllParents() =>
            _count = await _unitOfWork.CountParentsAsync();

        protected async Task CountParentsByPredicate(Expression<Func<Parent, bool>> predicate) =>
            _count = await _unitOfWork.CountParentsAsync(predicate);

        protected async Task ProjectOneParent(Expression<Func<Parent, bool>> predicate,
            Expression<Func<Parent, TestProjection>> projection)
        {
            try
            {
                _projectedParent = await _unitOfWork.ProjectParentAsync(predicate, projection);
            }
            catch (DocumentNotFoundException<Guid>) {}
            catch (InvalidOperationException) {}
        }

        protected async Task ProjectManyParents(Expression<Func<Parent, bool>> predicate,
            Expression<Func<Parent, TestProjection>> projection)
        {
            try
            {
                _projectedParents = await _unitOfWork.ProjectParentsAsync(predicate, projection);
            }
            catch (DocumentNotFoundException<Guid>) {}
        }

        #endregion

        #region Update

        protected async Task UpdateOneParentByDocument(Parent updated)
        {
            _updated = await _unitOfWork.UpdateParentAsync(updated);
            await GetOneParentByDocument(updated);
        }

        protected async Task UpdateOneParentByFieldSelector<TField>(Parent toUpdate, Expression<Func<Parent, TField>> selector,
            TField value)
        {
            _updated = await _unitOfWork.UpdateParentAsync(toUpdate, selector, value);
            await GetOneParentByDocument(toUpdate);
        }

        protected async Task UpdateOneParentByPredicateFieldSelector<TField>(Expression<Func<Parent, bool>> predicate,
            Expression<Func<Parent, TField>> selector, TField value)
        {
            try
            {
                _updated = await _unitOfWork.UpdateParentAsync(predicate, selector, value);
                await GetOneParentByPredicate(predicate);
            }
            catch (ManyDocumentsFoundException) {}
        }

        #endregion

        #region Delete

        protected async Task DeleteOneParentById(Guid id)
        {
            try
            {
                _deleted = await _unitOfWork.DeleteParentAsync(id);
            }
            catch (DocumentNotFoundException<Guid>) {}
        }

        protected async Task DeleteOneParentByDocument(Parent parent)
        {
            try
            {
                _deleted = await _unitOfWork.DeleteParentAsync(parent);
            }
            catch (DocumentNotFoundException<Guid>) {}
        }

        protected async Task DeleteOneParentByPredicate(Expression<Func<Parent, bool>> predicate)
        {
            try
            {
                _deleted = await _unitOfWork.DeleteParentAsync(predicate);
            }
            catch (DocumentNotFoundException<Guid>) {}
            catch (ManyDocumentsFoundException) {}
        }

        protected async Task DeleteManyParentsByDocument(IEnumerable<Parent> parents)
        {
            try
            {
                _deleted = await _unitOfWork.DeleteManyParentsAsync(parents);
            }
            catch (DocumentNotFoundException<Guid>) {}
        }

        protected async Task DeleteManyParentsByPredicate(Expression<Func<Parent, bool>> predicate)
        {
            try
            {
                _deleted = await _unitOfWork.DeleteManyParentsAsync(predicate);
            }
            catch (DocumentNotFoundException<Guid>) {}
        }

        #endregion

        #region Assert

        protected async Task RepositoryHas(long expected)
        {
            await CountAllParents();
            _count.Should().Be(expected);
        }

        protected async Task RepositoryHas(Parent expected) =>
            (await _parentRepository.GetAsync(expected))
                .Should().BeEquivalentTo(expected);

        protected async Task RepositoryHas(Expression<Func<Parent, bool>> predicate) =>
            (await _unitOfWork.AnyParentAsync(predicate))
                .Should().Be(true);

        protected void ParentIsFound(Parent expected) =>
            _parent.Should().BeEquivalentTo(expected);

        protected void ParentNotFound() =>
            _parent.Should().BeNull();

        protected void ParentsAreFound(IEnumerable<Parent> expected) =>
            _parents.Should().BeEquivalentTo(expected);

        protected void ParentsNotFound() =>
            _parents.Should().BeNullOrEmpty();

        protected void CountedParents(long expected) =>
            _count.Should().Be(expected);

        protected void ParentIsProjected(TestProjection expected) =>
            _projectedParent.Should().BeEquivalentTo(expected);

        protected void ParentNotProjected() =>
            _projectedParent.Should().BeNull();

        protected void ParentsAreProjected(IEnumerable<TestProjection> expected) =>
            _projectedParents.Should().BeEquivalentTo(expected);

        protected void ParentsNotProjected() =>
            _projectedParents.Should().BeNullOrEmpty();

        protected void ParentIsUpdated(bool expected) =>
            _updated.Should().Be(expected);

        protected void ParentNotUpdated() =>
            _parent.Should().BeNull();

        protected void ParentsDeleted(long expected) =>
            _deleted.Should().Be(expected);

        #endregion
    }
}