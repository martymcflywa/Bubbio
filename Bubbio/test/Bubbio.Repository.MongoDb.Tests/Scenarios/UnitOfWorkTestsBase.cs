using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Bubbio.Core.Exceptions;
using Bubbio.Core.Repository;
using Bubbio.Domain.Validators;
using Bubbio.MongoDb;
using Bubbio.MongoDb.Documents.Constants;
using Bubbio.MongoDb.Documents.Entities;
using Bubbio.MongoDb.Documents.Events;
using Bubbio.Tests.Core;
using Bubbio.Tests.Core.Examples;
using FluentAssertions;
using MongoDB.Driver;

namespace Bubbio.Repository.MongoDb.Tests.Scenarios
{
    public abstract class UnitOfWorkTestsBase
    {
        private readonly IRepository<Parent, Guid> _parentRepository;
        private readonly IRepository<Child, Guid> _childRepository;
        private readonly IRepository<Event, Guid> _eventRepository;

        private readonly UnitOfWork _unitOfWork;

        private IDocument<Guid> _document;
        private IEnumerable<IDocument<Guid>> _documents;

        private TestProjection _projection;
        private IEnumerable<TestProjection> _projections;

        private bool _any;
        private long _count;
        private bool _updated;
        private long _deleted;

        protected UnitOfWorkTestsBase()
        {
            var mongoDb = new MongoDbRepository(new MongoUrl(TestConstants.UnitOfWorkUrl));
            _parentRepository = new Repository<Parent, Guid>(mongoDb, Partitions.Parents.ToString());
            _childRepository = new Repository<Child, Guid>(mongoDb, Partitions.Children.ToString());
            _eventRepository = new Repository<Event, Guid>(mongoDb, Partitions.Events.ToString());
            _unitOfWork = new UnitOfWork(
                _parentRepository,
                _childRepository,
                _eventRepository,
                new TransitionValidator());
        }

        #region Setup

        protected async Task RepositoriesAreEmpty()
        {
            await _parentRepository.DropAsync();
            await _childRepository.DropAsync();
            await _eventRepository.DropAsync();
        }

        protected async Task RepositoryContains(IDocument<Guid> document)
        {
            await RepositoriesAreEmpty();

            if (document == null)
                return;

            switch (document)
            {
                case Parent parent:
                    await _parentRepository.InsertAsync(parent);
                    break;
                case Child child:
                    await _childRepository.InsertAsync(child);
                    break;
                case Event @event:
                    await _eventRepository.InsertAsync(@event);
                    break;
            }
        }

        protected async Task RepositoryContains(IEnumerable<IDocument<Guid>> documents)
        {
            await RepositoriesAreEmpty();

            if (documents == null)
                return;

            foreach (var document in documents)
            {
                switch (document)
                {
                    case Parent parent:
                        await _parentRepository.InsertAsync(parent);
                        break;
                    case Child child:
                        await _childRepository.InsertAsync(child);
                        break;
                    case Event @event:
                        await _eventRepository.InsertAsync(@event);
                        break;
                }
            }
        }

        #endregion

        #region Create

        protected async Task InsertOne(IDocument<Guid> document)
        {
            try
            {
                await _unitOfWork.InsertAsync(document);
            }
            catch (InvalidForeignIdException) {}
            catch (InvalidTransitionEventException) {}
        }

        protected async Task InsertMany(IEnumerable<IDocument<Guid>> documents)
        {
            try
            {
                await _unitOfWork.InsertManyAsync(documents);
            }
            catch (InvalidForeignIdException) {}
            catch (InvalidTransitionEventException) {}
        }

        #endregion

        #region Read

        protected async Task Any(Type type, Expression<Func<IDocument<Guid>, bool>> predicate)
        {
            if (typeof(Parent).IsAssignableFrom(type))
                _any = await _unitOfWork.AnyAsync<Parent>(predicate);
            if (typeof(Child).IsAssignableFrom(type))
                _any = await _unitOfWork.AnyAsync<Child>(predicate);
            if (typeof(Event).IsAssignableFrom(type))
                _any = await _unitOfWork.AnyAsync<Event>(predicate);
        }

        protected async Task GetOne(Type type, Guid id)
        {
            try
            {
                if (typeof(Parent).IsAssignableFrom(type))
                    _document = await _unitOfWork.GetAsync<Parent>(id);
                if (typeof(Child).IsAssignableFrom(type))
                    _document = await _unitOfWork.GetAsync<Child>(id);
                if (typeof(Event).IsAssignableFrom(type))
                    _document = await _unitOfWork.GetAsync<Event>(id);
            }
            catch (DocumentNotFoundException) {}
            catch (InvalidOperationException) {}
        }

        protected async Task GetOne(IDocument<Guid> document)
        {
            try
            {
                _document = await _unitOfWork.GetAsync(document);
            }
            catch (DocumentNotFoundException) {}
            catch (InvalidOperationException) {}
        }

        protected async Task GetOne(Type type, Expression<Func<IDocument<Guid>, bool>> predicate)
        {
            try
            {
                if (typeof(Parent).IsAssignableFrom(type))
                    _document = await _unitOfWork.GetAsync<Parent>(predicate);
                if (typeof(Child).IsAssignableFrom(type))
                    _document = await _unitOfWork.GetAsync<Child>(predicate);
                if (typeof(Event).IsAssignableFrom(type))
                    _document = await _unitOfWork.GetAsync<Event>(predicate);
            }
            catch (DocumentNotFoundException) {}
            catch (InvalidOperationException) {}
        }

        protected async Task GetMany(Type type, Expression<Func<IDocument<Guid>, bool>> predicate)
        {
            try
            {
                if (typeof(Parent).IsAssignableFrom(type))
                    _documents = await _unitOfWork.GetManyAsync<Parent>(predicate, default);
                if (typeof(Child).IsAssignableFrom(type))
                    _documents = await _unitOfWork.GetManyAsync<Child>(predicate, default);
                if (typeof(Event).IsAssignableFrom(type))
                    _documents = await _unitOfWork.GetManyAsync<Event>(predicate, default);
            }
            catch (DocumentNotFoundException) {}
        }

        protected async Task GetManyPaged(Type type, Expression<Func<IDocument<Guid>, bool>> predicate, int skip,
            int take)
        {
            try
            {
                if (typeof(Parent).IsAssignableFrom(type))
                    _documents = await _unitOfWork.GetManyAsync<Parent>(predicate, skip, take);
                if (typeof(Child).IsAssignableFrom(type))
                    _documents = await _unitOfWork.GetManyAsync<Child>(predicate, skip, take);
                if (typeof(Event).IsAssignableFrom(type))
                    _documents = await _unitOfWork.GetManyAsync<Event>(predicate, skip, take);
            }
            catch (DocumentNotFoundException) {}
        }

        protected async Task Count(Type type)
        {
            if (typeof(Parent).IsAssignableFrom(type))
                _count = await _unitOfWork.CountAsync<Parent>();
            if (typeof(Child).IsAssignableFrom(type))
                _count = await _unitOfWork.CountAsync<Child>();
            if (typeof(Event).IsAssignableFrom(type))
                _count = await _unitOfWork.CountAsync<Event>();
        }

        protected async Task Count(Type type, Expression<Func<IDocument<Guid>, bool>> predicate)
        {
            if (typeof(Parent).IsAssignableFrom(type))
                _count = await _unitOfWork.CountAsync<Parent>(predicate);
            if (typeof(Child).IsAssignableFrom(type))
                _count = await _unitOfWork.CountAsync<Child>(predicate);
            if (typeof(Event).IsAssignableFrom(type))
                _count = await _unitOfWork.CountAsync<Event>(predicate);
        }

        protected async Task ProjectOne(Type type, Expression<Func<IDocument<Guid>, bool>> predicate,
            Expression<Func<IDocument<Guid>, TestProjection>> projection)
        {
            try
            {
                if (typeof(Parent).IsAssignableFrom(type))
                    _projection = await _unitOfWork.ProjectOneAsync<Parent, TestProjection>(predicate, projection);
                if (typeof(Child).IsAssignableFrom(type))
                    _projection = await _unitOfWork.ProjectOneAsync<Child, TestProjection>(predicate, projection);
                if (typeof(Event).IsAssignableFrom(type))
                    _projection = await _unitOfWork.ProjectOneAsync<Event, TestProjection>(predicate, projection);
            }
            catch (DocumentNotFoundException) {}
            catch (InvalidOperationException) {}
        }

        protected async Task ProjectMany(Type type, Expression<Func<IDocument<Guid>, bool>> predicate,
            Expression<Func<IDocument<Guid>, TestProjection>> projection)
        {
            try
            {
                if (typeof(Parent).IsAssignableFrom(type))
                    _projections = await _unitOfWork.ProjectManyAsync<Parent, TestProjection>(predicate, projection);
                if (typeof(Child).IsAssignableFrom(type))
                    _projections = await _unitOfWork.ProjectManyAsync<Child, TestProjection>(predicate, projection);
                if (typeof(Event).IsAssignableFrom(type))
                    _projections = await _unitOfWork.ProjectManyAsync<Event, TestProjection>(predicate, projection);
            }
            catch (DocumentNotFoundException) {}
        }

        #endregion

        #region Update

        protected async Task UpdateOne(IDocument<Guid> updated)
        {
            try
            {
                _updated = await _unitOfWork.UpdateAsync(updated);
            }
            catch (InvalidOperationException) {}
        }

        protected async Task UpdateOne<TField>(IDocument<Guid> toUpdate,
            Expression<Func<IDocument<Guid>, TField>> selector, TField value)
        {
            _updated = await _unitOfWork.UpdateAsync(toUpdate, selector, value);
        }

        protected async Task UpdateOne<TField>(Type type, Expression<Func<IDocument<Guid>, bool>> predicate,
                Expression<Func<IDocument<Guid>, TField>> selector, TField value)
        {
            try
            {
                if (typeof(Parent).IsAssignableFrom(type))
                    _updated = await _unitOfWork.UpdateAsync<Parent, TField>(predicate, selector, value);
                if (typeof(Child).IsAssignableFrom(type))
                    _updated = await _unitOfWork.UpdateAsync<Child, TField>(predicate, selector, value);
                if (typeof(Event).IsAssignableFrom(type))
                    _updated = await _unitOfWork.UpdateAsync<Event, TField>(predicate, selector, value);
            }
            catch (InvalidOperationException) {}
        }

        #endregion

        #region Delete

        protected async Task DeleteOne(Type type, Guid id, bool cascade = default)
        {
            if (typeof(Parent).IsAssignableFrom(type))
                _deleted = await _unitOfWork.DeleteAsync<Parent>(id, cascade);
            if (typeof(Child).IsAssignableFrom(type))
                _deleted = await _unitOfWork.DeleteAsync<Child>(id, cascade);
            if (typeof(Event).IsAssignableFrom(type))
                _deleted = await _unitOfWork.DeleteAsync<Event>(id, cascade);
        }

        protected async Task DeleteOne(IDocument<Guid> document, bool cascade = default) =>
            _deleted = await _unitOfWork.DeleteAsync(document, cascade);

        protected async Task DeleteMany(IEnumerable<IDocument<Guid>> documents, bool cascade = default) =>
            _deleted = await _unitOfWork.DeleteManyAsync(documents, cascade);

        protected async Task DeleteMany(Type type, Expression<Func<IDocument<Guid>, bool>> predicate,
            bool cascade = default)
        {
            if (typeof(Parent).IsAssignableFrom(type))
                _deleted = await _unitOfWork.DeleteManyAsync<Parent>(predicate, cascade);
            if (typeof(Child).IsAssignableFrom(type))
                _deleted = await _unitOfWork.DeleteManyAsync<Child>(predicate, cascade);
            if (typeof(Event).IsAssignableFrom(type))
                _deleted = await _unitOfWork.DeleteManyAsync<Event>(predicate, cascade);
        }

        #endregion

        #region Assert

        protected void AnyFound(bool expected) =>
            _any.Should().Be(expected);

        protected async Task RepositoryHas(Type type, long expected)
        {
            await Count(type);
            _count.Should().Be(expected);
        }

        protected async Task RepositoryHas(IDocument<Guid> expected)
        {
            switch (expected)
            {
                case Parent parent:
                    await GetOne(parent);
                    (_document as Parent).Should()
                        .BeEquivalentTo(expected, config => config.Excluding(p => p.Modified));
                    break;
                case Child child:
                    await GetOne(child);
                    (_document as Child).Should()
                        .BeEquivalentTo(expected, config => config.Excluding(c => c.Modified));
                    break;
                case Event @event:
                    await GetOne(@event);
                    (_document as Event).Should()
                        .BeEquivalentTo(expected, config => config.Excluding(e => e.Modified));
                    break;
            }
        }

        protected void DocumentFound(IDocument<Guid> expected)
        {
            switch (expected)
            {
                case Parent _:
                    (_document as Parent).Should().BeEquivalentTo(expected);
                    break;
                case Child _:
                    (_document as Child).Should().BeEquivalentTo(expected);
                    break;
                case Event _:
                    (_document as Event).Should().BeEquivalentTo(expected);
                    break;
            }
        }

        protected void DocumentNotFound() =>
            _document.Should().BeNull();

        protected void DocumentsFound(IEnumerable<IDocument<Guid>> expected)
        {
            switch (expected)
            {
                case IEnumerable<Parent> _:
                    (_documents as IEnumerable<Parent>).Should().BeEquivalentTo(expected);
                    break;
                case IEnumerable<Child> _:
                    (_documents as IEnumerable<Child>).Should().BeEquivalentTo(expected);
                    break;
                case IEnumerable<Event> _:
                    (_documents as IEnumerable<Event>).Should().BeEquivalentTo(expected);
                    break;
            }
        }

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

        protected async Task ModifiedUpdated(IDocument<Guid> document)
        {
            await GetOne(document);
            _document.Modified.Should().NotBe(document.Modified);
        }

        protected async Task ModifiedNotUpdated(IDocument<Guid> document)
        {
            await GetOne(document);
            _document.Modified.Should().Be(document.Modified);
        }

        #endregion
    }
}