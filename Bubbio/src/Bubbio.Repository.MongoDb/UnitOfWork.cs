using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Bubbio.Core.Abstraction;
using Bubbio.Core.Contracts.Enums;
using Bubbio.Core.Contracts.Events;
using Bubbio.Core.Exceptions;
using Bubbio.Core.Helpers;
using Bubbio.Core.Repository;
using Bubbio.MongoDb.Documents.Entities;
using Bubbio.MongoDb.Documents.Events;

namespace Bubbio.Repository.MongoDb
{
    public class UnitOfWork
    {
        private readonly IRepository<Parent, Guid> _parentRepository;
        private readonly IRepository<Child, Guid> _childRepository;
        private readonly IRepository<Event, Guid> _eventRepository;

        private readonly ITransitionValidator _transitionValidator;

        public UnitOfWork(
            IRepository<Parent, Guid> parentRepository,
            IRepository<Child, Guid> childRepository,
            IRepository<Event, Guid> eventRepository,
            ITransitionValidator transitionValidator)
        {
            _parentRepository = parentRepository;
            _childRepository = childRepository;
            _eventRepository = eventRepository;
            _transitionValidator = transitionValidator;
        }

        #region Create

        public async Task<bool> InsertAsync(IDocument<Guid> document, CancellationToken token = default)
        {
            switch (document)
            {
                case Parent parent:
                    return await InsertParentAsync(parent, token);
                case Child child:
                    return await InsertChildAsync(child, token);
                case Event @event:
                    return await InsertEventAsync(@event, token);
                default:
                    throw new UnsupportedTypeException(document.GetType());
            }
        }

        private async Task<bool> InsertParentAsync(Parent parent, CancellationToken token = default)
        {
            await _parentRepository.InsertAsync(parent, token);
            return true;
        }

        private async Task<bool> InsertChildAsync(Child child, CancellationToken token = default)
        {
            if (!await LinkedDocumentExistsAsync<Parent>(child.ParentId, token))
                return false;

            await _childRepository.InsertAsync(child, token);
            return true;
        }

        private async Task<bool> InsertEventAsync(Event @event, CancellationToken token = default)
        {
            if (!await LinkedDocumentExistsAsync<Child>(@event.ChildId, token))
                return false;

            if (!await IsValidTransitionAsync(@event as Sleep, token))
                return false;

            await _eventRepository.InsertAsync(@event, token);
            return true;
        }

        public async Task<bool> InsertManyAsync(IEnumerable<IDocument<Guid>> documents, CancellationToken token = default)
        {
            switch (documents)
            {
                case IEnumerable<Parent> parents:
                    return await InsertParentsAsync(parents, token);
                case IEnumerable<Child> children:
                    return await InsertChildrenAsync(children, token);
                case IEnumerable<Event> events:
                    return await InsertEventsAsync(events, token);
                default:
                    return false;
            }
        }

        private async Task<bool> InsertParentsAsync(IEnumerable<Parent> parents, CancellationToken token = default)
        {
            await _parentRepository.InsertManyAsync(parents, token);
            return true;
        }

        private async Task<bool> InsertChildrenAsync(IEnumerable<Child> children, CancellationToken token = default)
        {
            var list = children.ToList();
            if (!list.All(c => LinkedDocumentExists<Parent>(c.ParentId, token)))
                return false;

            await _childRepository.InsertManyAsync(list, token);
            return true;
        }

        private async Task<bool> InsertEventsAsync(IEnumerable<Event> events, CancellationToken token = default)
        {
            var list = events.ToList();

            if (!list.All(e => LinkedDocumentExists<Child>(e.ChildId, token)))
                return false;
            if (!await IsValidTransitionsAsync(list, token))
                return false;

            await _eventRepository.InsertManyAsync(list, token);
            return true;
        }

        #endregion

        #region Read

        #region Any

        public async Task<bool> AnyAsync<TDocument>(Expression<Func<IDocument<Guid>, bool>> predicate,
            CancellationToken token = default)
            where TDocument : IDocument<Guid>
        {
            var type = typeof(TDocument);

            if (type.IsATypeOf(typeof(Parent)))
            {
                var filter = ExpressionHelper<IDocument<Guid>, Parent>.Transform(predicate);
                return await AnyParentAsync(filter, token);
            }

            if (type.IsATypeOf(typeof(Child)))
            {
                var filter = ExpressionHelper<IDocument<Guid>, Child>.Transform(predicate);
                return await AnyChildAsync(filter, token);
            }

            if (type != typeof(Event)) throw new UnsupportedTypeException(typeof(TDocument));
            {
                var filter = ExpressionHelper<IDocument<Guid>, Event>.Transform(predicate);
                return await AnyEventAsync(filter, token);
            }

        }

        private async Task<bool> AnyParentAsync(Expression<Func<Parent, bool>> predicate,
            CancellationToken token = default)
        {
            return await _parentRepository.AnyAsync(predicate, token);
        }

        private async Task<bool> AnyChildAsync(Expression<Func<Child, bool>> predicate,
            CancellationToken token = default)
        {
            return await _childRepository.AnyAsync(predicate, token);
        }

        private async Task<bool> AnyEventAsync(Expression<Func<Event, bool>> predicate,
            CancellationToken token = default)
        {
            return await _eventRepository.AnyAsync(predicate, token);
        }

        #endregion

        #region GetById

        public async Task<IDocument<Guid>> GetAsync<TDocument>(Guid id, CancellationToken token = default)
            where TDocument : IDocument<Guid>
        {
            var type = typeof(TDocument);

            if (type.IsATypeOf(typeof(Parent)))
                return await GetParentAsync(id, token);
            if (type.IsATypeOf(typeof(Child)))
                return await GetChildAsync(id, token);
            if (type.IsATypeOf(typeof(Event)))
                return await GetEventAsync(id, token);
            throw new UnsupportedTypeException(type);
        }

        private async Task<Parent> GetParentAsync(Guid id, CancellationToken token = default)
        {
            return await _parentRepository.GetAsync(id, token);
        }

        private async Task<Child> GetChildAsync(Guid id, CancellationToken token = default)
        {
            return await _childRepository.GetAsync(id, token);
        }

        private async Task<Event> GetEventAsync(Guid id, CancellationToken token = default)
        {
            return await _eventRepository.GetAsync(id, token);
        }

        #endregion

        #region GetByDocument

        public async Task<IDocument<Guid>> GetAsync(IDocument<Guid> document, CancellationToken token = default)
        {
            switch (document)
            {
                case Parent parent:
                    return await GetParentAsync(parent, token);
                case Child child:
                    return await GetChildAsync(child, token);
                case Event @event:
                    return await GetEventAsync(@event, token);
                default:
                    throw new UnsupportedTypeException(document.GetType());
            }
        }

        private async Task<Parent> GetParentAsync(Parent parent, CancellationToken token = default)
        {
            return await _parentRepository.GetAsync(parent, token);
        }

        private async Task<Child> GetChildAsync(Child child, CancellationToken token = default)
        {
            return await _childRepository.GetAsync(child, token);
        }

        public async Task<Event> GetEventAsync(Event @event, CancellationToken token = default)
        {
            return await _eventRepository.GetAsync(@event, token);
        }

        #endregion

        #region GetByPredicate

        public async Task<IDocument<Guid>> GetAsync<TDocument>(Expression<Func<IDocument<Guid>, bool>> predicate,
                CancellationToken token = default)
            where TDocument : IDocument<Guid>
        {
            var type = typeof(TDocument);

            if (type.IsATypeOf(typeof(Parent)))
            {
                var filter = ExpressionHelper<IDocument<Guid>, Parent>.Transform(predicate);
                return await GetParentAsync(filter, token);
            }

            if (type.IsATypeOf(typeof(Child)))
            {
                var filter = ExpressionHelper<IDocument<Guid>, Child>.Transform(predicate);
                return await GetChildAsync(filter, token);
            }

            if (type != typeof(Event)) throw new UnsupportedTypeException(type);
            {
                var filter = ExpressionHelper<IDocument<Guid>, Event>.Transform(predicate);
                return await GetEventAsync(filter, token);
            }
        }

        private async Task<Parent> GetParentAsync(Expression<Func<Parent, bool>> predicate,
            CancellationToken token = default)
        {
            return await _parentRepository.GetAsync(predicate, token);
        }

        private async Task<Child> GetChildAsync(Expression<Func<Child, bool>> predicate,
            CancellationToken token = default)
        {
            return await _childRepository.GetAsync(predicate, token);
        }

        private async Task<Event> GetEventAsync(Expression<Func<Event, bool>> predicate,
            CancellationToken token = default)
        {
            return await _eventRepository.GetAsync(predicate, token);
        }

        #endregion

        #region GetMany

        public async Task<IEnumerable<IDocument<Guid>>> GetManyAsync<TDocument>(
                Expression<Func<IDocument<Guid>, bool>> predicate,
                CancellationToken token = default)
            where TDocument : IDocument<Guid>
        {
            var type = typeof(TDocument);

            if (type.IsATypeOf(typeof(Parent)))
            {
                var filter = ExpressionHelper<IDocument<Guid>, Parent>.Transform(predicate);
                return await GetParentsAsync(filter, token);
            }

            if (type.IsATypeOf(typeof(Child)))
            {
                var filter = ExpressionHelper<IDocument<Guid>, Child>.Transform(predicate);
                return await GetChildrenAsync(filter, token);
            }

            if (type != typeof(Event)) throw new UnsupportedTypeException(type);
            {
                var filter = ExpressionHelper<IDocument<Guid>, Event>.Transform(predicate);
                return await GetEventsAsync(filter, token);
            }
        }

        private async Task<IEnumerable<Parent>> GetParentsAsync(Expression<Func<Parent, bool>> predicate,
            CancellationToken token = default)
        {
            return await _parentRepository.GetManyAsync(predicate, token);
        }

        private async Task<IEnumerable<Child>> GetChildrenAsync(Expression<Func<Child, bool>> predicate,
            CancellationToken token = default)
        {
            return await _childRepository.GetManyAsync(predicate, token);
        }

        private async Task<IEnumerable<Event>> GetEventsAsync(Expression<Func<Event, bool>> predicate,
            CancellationToken token = default)
        {
            return await _eventRepository.GetManyAsync(predicate, token);
        }

        #endregion

        #region GetManyPaged

        public async Task<IEnumerable<IDocument<Guid>>> GetManyAsync<TDocument>(
            Expression<Func<IDocument<Guid>, bool>> predicate, int skip = 0, int take = 50,
            CancellationToken token = default)
        {
            var type = typeof(TDocument);

            if (type.IsATypeOf(typeof(Parent)))
            {
                var filter = ExpressionHelper<IDocument<Guid>, Parent>.Transform(predicate);
                return await GetParentsAsync(filter, skip, take, token);
            }

            if (type.IsATypeOf(typeof(Child)))
            {
                var filter = ExpressionHelper<IDocument<Guid>, Child>.Transform(predicate);
                return await GetChildrenAsync(filter, skip, take, token);
            }

            if (type != typeof(Event)) throw new UnsupportedTypeException(type);
            {
                var filter = ExpressionHelper<IDocument<Guid>, Event>.Transform(predicate);
                return await GetEventsAsync(filter, skip, take, token);
            }
        }

        private async Task<IEnumerable<Parent>> GetParentsAsync(Expression<Func<Parent, bool>> predicate, int skip = 0,
            int take = 50, CancellationToken token = default)
        {
            return await _parentRepository.GetManyAsync(predicate, skip, take, token);
        }

        private async Task<IEnumerable<Child>> GetChildrenAsync(Expression<Func<Child, bool>> predicate, int skip = 0,
            int take = 50, CancellationToken token = default)
        {
            return await _childRepository.GetManyAsync(predicate, skip, take, token);
        }

        private async Task<IEnumerable<Event>> GetEventsAsync(Expression<Func<Event, bool>> predicate, int skip = 0,
            int take = 50, CancellationToken token = default)
        {
            return await _eventRepository.GetManyAsync(predicate, skip, take, token);
        }

        #endregion

        #region CountAll

        public async Task<long> CountAsync<TDocument>(CancellationToken token = default)
            where TDocument : IDocument<Guid>
        {
            var type = typeof(TDocument);

            if (type.IsATypeOf(typeof(Parent)))
                return await CountParentsAsync(token);
            if (type.IsATypeOf(typeof(Child)))
                return await CountChildrenAsync(token);
            if (type.IsATypeOf(typeof(Event)))
                return await CountEventsAsync(token);

            throw new UnsupportedTypeException(type);
        }

        private async Task<long> CountParentsAsync(CancellationToken token = default)
        {
            return await _parentRepository.CountAsync(token);
        }

        private async Task<long> CountChildrenAsync(CancellationToken token = default)
        {
            return await _childRepository.CountAsync(token);
        }

        private async Task<long> CountEventsAsync(CancellationToken token = default)
        {
            return await _eventRepository.CountAsync(token);
        }

        #endregion

        #region CountByPredicate

        public async Task<long> CountAsync<TDocument>(Expression<Func<IDocument<Guid>, bool>> predicate,
                CancellationToken token = default)
            where TDocument : IDocument<Guid>
        {
            var type = typeof(TDocument);

            if (type.IsATypeOf(typeof(Parent)))
            {
                var filter = ExpressionHelper<IDocument<Guid>, Parent>.Transform(predicate);
                return await CountParentsAsync(filter, token);
            }

            if (type.IsATypeOf(typeof(Child)))
            {
                var filter = ExpressionHelper<IDocument<Guid>, Child>.Transform(predicate);
                return await CountChildrenAsync(filter, token);
            }

            if (type != typeof(Event)) throw new UnsupportedTypeException(type);
            {
                var filter = ExpressionHelper<IDocument<Guid>, Event>.Transform(predicate);
                return await CountEventsAsync(filter, token);
            }
        }

        private async Task<long> CountParentsAsync(Expression<Func<Parent, bool>> predicate,
            CancellationToken token = default)
        {
            return await _parentRepository.CountAsync(predicate, token);
        }

        private async Task<long> CountChildrenAsync(Expression<Func<Child, bool>> predicate,
            CancellationToken token = default)
        {
            return await _childRepository.CountAsync(predicate, token);
        }

        private async Task<long> CountEventsAsync(Expression<Func<Event, bool>> predicate,
            CancellationToken token = default)
        {
            return await _eventRepository.CountAsync(predicate, token);
        }

        #endregion

        #region ProjectOne

        public async Task<TProjection> ProjectOneAsync<TDocument, TProjection>(
                Expression<Func<IDocument<Guid>, bool>> predicate,
                Expression<Func<IDocument<Guid>, TProjection>> projection, CancellationToken token = default)
                where TDocument : IDocument<Guid>
            where TProjection : class
        {
            var type = typeof(TDocument);

            if (type.IsATypeOf(typeof(Parent)))
            {
                var filter = ExpressionHelper<IDocument<Guid>, Parent>.Transform(predicate);
                var proj = ExpressionHelper<IDocument<Guid>, Parent>.Transform(projection);
                return await ProjectParentAsync(filter, proj, token);
            }

            if (type.IsATypeOf(typeof(Child)))
            {
                var filter = ExpressionHelper<IDocument<Guid>, Child>.Transform(predicate);
                var proj = ExpressionHelper<IDocument<Guid>, Child>.Transform(projection);
                return await ProjectChildAsync(filter, proj, token);
            }

            if (type != typeof(Event)) throw new UnsupportedTypeException(type);
            {
                var filter = ExpressionHelper<IDocument<Guid>, Event>.Transform(predicate);
                var proj = ExpressionHelper<IDocument<Guid>, Event>.Transform(projection);
                return await ProjectEventAsync(filter, proj, token);
            }
        }

        private async Task<TProjection> ProjectParentAsync<TProjection>(Expression<Func<Parent, bool>> predicate,
            Expression<Func<Parent, TProjection>> projection, CancellationToken token = default)
            where TProjection : class
        {
            return await _parentRepository.ProjectAsync(predicate, projection, token);
        }

        private async Task<TProjection> ProjectChildAsync<TProjection>(Expression<Func<Child, bool>> predicate,
            Expression<Func<Child, TProjection>> projection, CancellationToken token = default)
            where TProjection : class
        {
            return await _childRepository.ProjectAsync(predicate, projection, token);
        }

        private async Task<TProjection> ProjectEventAsync<TProjection>(Expression<Func<Event, bool>> predicate,
            Expression<Func<Event, TProjection>> projection, CancellationToken token = default)
            where TProjection : class
        {
            return await _eventRepository.ProjectAsync(predicate, projection, token);
        }

        #endregion

        #region ProjectMany

        public async Task<IEnumerable<TProjection>> ProjectManyAsync<TDocument, TProjection>(
                Expression<Func<IDocument<Guid>, bool>> predicate,
                Expression<Func<IDocument<Guid>, TProjection>> projection, CancellationToken token = default)
            where TDocument : IDocument<Guid>
            where TProjection : class
        {
            var type = typeof(TDocument);

            if (type.IsATypeOf(typeof(Parent)))
            {
                var filter = ExpressionHelper<IDocument<Guid>, Parent>.Transform(predicate);
                var proj = ExpressionHelper<IDocument<Guid>, Parent>.Transform(projection);
                return await ProjectParentsAsync(filter, proj, token);
            }

            if (type.IsATypeOf(typeof(Child)))
            {
                var filter = ExpressionHelper<IDocument<Guid>, Child>.Transform(predicate);
                var proj = ExpressionHelper<IDocument<Guid>, Child>.Transform(projection);
                return await ProjectChildrenAsync(filter, proj, token);
            }

            if (type != typeof(Event)) throw new UnsupportedTypeException(type);
            {
                var filter = ExpressionHelper<IDocument<Guid>, Event>.Transform(predicate);
                var proj = ExpressionHelper<IDocument<Guid>, Event>.Transform(projection);
                return await ProjectEventsAsync(filter, proj, token);
            }
        }

        private async Task<IEnumerable<TProjection>> ProjectParentsAsync<TProjection>(
            Expression<Func<Parent, bool>> predicate, Expression<Func<Parent, TProjection>> projection,
            CancellationToken token = default)
            where TProjection : class
        {
            return await _parentRepository.ProjectManyAsync(predicate, projection, token);
        }

        private async Task<IEnumerable<TProjection>> ProjectChildrenAsync<TProjection>(
            Expression<Func<Child, bool>> predicate, Expression<Func<Child, TProjection>> projection,
            CancellationToken token = default)
            where TProjection : class
        {
            return await _childRepository.ProjectManyAsync(predicate, projection, token);
        }

        private async Task<IEnumerable<TProjection>> ProjectEventsAsync<TProjection>(
            Expression<Func<Event, bool>> predicate, Expression<Func<Event, TProjection>> projection,
            CancellationToken token = default)
            where TProjection : class
        {
            return await _eventRepository.ProjectManyAsync(predicate, projection, token);
        }

        #endregion

        #endregion

        #region Update

        #region UpdateByDocument

        public async Task<bool> UpdateAsync(IDocument<Guid> updated, CancellationToken token = default)
        {
            switch (updated)
            {
                case Parent parent:
                    return await UpdateParentAsync(parent, token);
                case Child child:
                    return await UpdateChildAsync(child, token);
                case Event @event:
                    return await UpdateEventAsync(@event, token);
                default:
                    throw new UnsupportedTypeException(updated.GetType());
            }
        }

        private async Task<bool> UpdateParentAsync(Parent updated, CancellationToken token = default)
        {
            return await _parentRepository.UpdateAsync(updated, token);
        }

        private async Task<bool> UpdateChildAsync(Child updated, CancellationToken token = default)
        {
            return await _childRepository.UpdateAsync(updated, token);
        }

        private async Task<bool> UpdateEventAsync(Event updated, CancellationToken token = default)
        {
            return await _eventRepository.UpdateAsync(updated, token);
        }

        #endregion

        #region UpdateByField

        public async Task<bool> UpdateAsync<TField>(IDocument<Guid> toUpdate,
            Expression<Func<IDocument<Guid>, TField>> selector, TField value, CancellationToken token = default)
        {
            switch (toUpdate)
            {
                case Parent parent:
                    var parentSelect = ExpressionHelper<IDocument<Guid>, Parent>.Transform(selector);
                    return await UpdateParentAsync(parent, parentSelect, value, token);
                case Child child:
                    var childSelect = ExpressionHelper<IDocument<Guid>, Child>.Transform(selector);
                    return await UpdateChildAsync(child, childSelect, value, token);
                case Event @event:
                    var eventSelect = ExpressionHelper<IDocument<Guid>, Event>.Transform(selector);
                    return await UpdateEventAsync(@event, eventSelect, value, token);
                default:
                    throw new UnsupportedTypeException(toUpdate.GetType());
            }
        }

        private async Task<bool> UpdateParentAsync<TField>(Parent toUpdate,
            Expression<Func<Parent, TField>> selector, TField value, CancellationToken token = default)
        {
            return await _parentRepository.UpdateAsync(toUpdate, selector, value, token);
        }

        private async Task<bool> UpdateChildAsync<TField>(Child toUpdate,
            Expression<Func<Child, TField>> selector, TField value, CancellationToken token = default)
        {
            return await _childRepository.UpdateAsync(toUpdate, selector, value, token);
        }

        private async Task<bool> UpdateEventAsync<TField>(Event toUpdate,
            Expression<Func<Event, TField>> selector, TField value, CancellationToken token = default)
        {
            return await _eventRepository.UpdateAsync(toUpdate, selector, value, token);
        }

        #endregion

        #region UpdateByPredicate

        public async Task<bool> UpdateAsync<TDocument, TField>(Expression<Func<IDocument<Guid>, bool>> predicate,
                Expression<Func<IDocument<Guid>, TField>> selector, TField value, CancellationToken token = default)
            where TDocument : IDocument<Guid>
        {
            var type = typeof(TDocument);

            if (type.IsATypeOf(typeof(Parent)))
            {
                var filter = ExpressionHelper<IDocument<Guid>, Parent>.Transform(predicate);
                var select = ExpressionHelper<IDocument<Guid>, Parent>.Transform(selector);
                return await UpdateParentAsync(filter, select, value, token);
            }

            if (type.IsATypeOf(typeof(Child)))
            {
                var filter = ExpressionHelper<IDocument<Guid>, Child>.Transform(predicate);
                var select = ExpressionHelper<IDocument<Guid>, Child>.Transform(selector);
                return await UpdateChildAsync(filter, select, value, token);
            }

            if (type != typeof(Event)) throw new UnsupportedTypeException(type);
            {
                var filter = ExpressionHelper<IDocument<Guid>, Event>.Transform(predicate);
                var select = ExpressionHelper<IDocument<Guid>, Event>.Transform(selector);
                return await UpdateEventAsync(filter, select, value, token);
            }
        }

        private async Task<bool> UpdateParentAsync<TField>(Expression<Func<Parent, bool>> predicate,
            Expression<Func<Parent, TField>> selector, TField value, CancellationToken token = default)
        {
            return await _parentRepository.UpdateAsync(predicate, selector, value, token);
        }

        private async Task<bool> UpdateChildAsync<TField>(Expression<Func<Child, bool>> predicate,
            Expression<Func<Child, TField>> selector, TField value, CancellationToken token = default)
        {
            return await _childRepository.UpdateAsync(predicate, selector, value, token);
        }

        private async Task<bool> UpdateEventAsync<TField>(Expression<Func<Event, bool>> predicate,
            Expression<Func<Event, TField>> selector, TField value, CancellationToken token = default)
        {
            return await _eventRepository.UpdateAsync(predicate, selector, value, token);
        }

        #endregion

        #endregion

        #region Delete

        #region DeleteById

        public async Task<long> DeleteAsync<TDocument>(Guid id, bool cascade = default,
                CancellationToken token = default)
            where TDocument : IDocument<Guid>
        {
            var type = typeof(TDocument);

            if (type.IsATypeOf(typeof(Parent)))
                return await DeleteParentAsync(id, cascade, token);
            if (type.IsATypeOf(typeof(Child)))
                return await DeleteChildAsync(id, cascade, token);
            if (type.IsATypeOf(typeof(Event)))
                return await DeleteEventAsync(id, token);

            throw new UnsupportedTypeException(type);
        }

        private async Task<long> DeleteParentAsync(Guid id, bool cascade = default, CancellationToken token = default)
        {
            if (!cascade)
                return await _parentRepository.DeleteAsync(id, token);

            var count = await DeleteManyChildrenAsync(c => c.ParentId.Equals(id), true, token);
            return count + await _parentRepository.DeleteAsync(id, token);
        }

        private async Task<long> DeleteChildAsync(Guid id, bool cascade = default, CancellationToken token = default)
        {
            if (!cascade)
                return await _childRepository.DeleteAsync(id, token);

            var count = await DeleteManyEventsAsync(e => e.ChildId.Equals(id), token);
            return count + await _childRepository.DeleteAsync(id, token);
        }

        private async Task<long> DeleteEventAsync(Guid id, CancellationToken token = default)
        {
            return await _eventRepository.DeleteAsync(id, token);
        }

        #endregion

        #region DeleteByDocument

        public async Task<long> DeleteAsync(IDocument<Guid> document, bool cascade = default,
            CancellationToken token = default)
        {
            switch (document)
            {
                case Parent parent:
                    return await DeleteParentAsync(parent, cascade, token);
                case Child child:
                    return await DeleteChildAsync(child, cascade, token);
                case Event @event:
                    return await DeleteEventAsync(@event, token);
                default:
                    throw new UnsupportedTypeException(document.GetType());
            }
        }

        private async Task<long> DeleteParentAsync(Parent parent, bool cascade = default,
            CancellationToken token = default)
        {
            if (!cascade)
                return await _parentRepository.DeleteAsync(parent, token);

            var count = await DeleteManyChildrenAsync(c => c.ParentId.Equals(parent.Id), true, token);
            return count + await _parentRepository.DeleteAsync(parent, token);
        }

        private async Task<long> DeleteChildAsync(Child child, bool cascade = default,
            CancellationToken token = default)
        {
            if (!cascade)
                return await _childRepository.DeleteAsync(child, token);

            var count = await DeleteManyEventsAsync(e => e.ChildId.Equals(child.Id), token);
            return count + await _childRepository.DeleteAsync(child, token);
        }

        private async Task<long> DeleteEventAsync(Event @event, CancellationToken token = default)
        {
            return await _eventRepository.DeleteAsync(@event, token);
        }

        #endregion

        #region DeleteManyByDocument

        public async Task<long> DeleteManyAsync(IEnumerable<IDocument<Guid>> documents, bool cascade = default,
            CancellationToken token = default)
        {
            switch (documents)
            {
                case IEnumerable<Parent> parents:
                    return await DeleteManyParentsAsync(parents, cascade, token);
                case IEnumerable<Child> children:
                    return await DeleteManyChildrenAsync(children, cascade, token);
                case IEnumerable<Event> events:
                    return await DeleteManyEventsAsync(events, token);
                default:
                    throw new UnsupportedTypeException(documents.GetType());
            }
        }

        private async Task<long> DeleteManyParentsAsync(IEnumerable<Parent> parents, bool cascade = default,
            CancellationToken token = default)
        {
            if (!cascade)
                return await _parentRepository.DeleteManyAsync(parents, token);

            var parentList = parents.ToList();

            var children = parentList
                .Select(async p => await _childRepository.GetManyAsync(c => c.ParentId.Equals(p.Id), token));

            var count = await DeleteManyChildrenAsync(children.SelectMany(c => c.Result), true, token);
            return count + await _parentRepository.DeleteManyAsync(parentList, token);
        }

        private async Task<long> DeleteManyChildrenAsync(IEnumerable<Child> children, bool cascade = default,
            CancellationToken token = default)
        {
            if (!cascade)
                return await _childRepository.DeleteManyAsync(children, token);

            var childrenList = children.ToList();

            var events = childrenList
                .Select(async c => await _eventRepository.GetManyAsync(e => e.ChildId.Equals(c.Id), token));

            var count = await DeleteManyEventsAsync(events.SelectMany(e => e.Result), token);
            return count + await _childRepository.DeleteManyAsync(childrenList, token);
        }

        private async Task<long> DeleteManyEventsAsync(IEnumerable<Event> events, CancellationToken token = default)
        {
            return await _eventRepository.DeleteManyAsync(events, token);
        }

        #endregion

        #region DeleteManyByPredicate

        public async Task<long> DeleteManyAsync<TDocument>(Expression<Func<IDocument<Guid>, bool>> predicate,
            bool cascade = default, CancellationToken token = default)
        {
            var type = typeof(TDocument);

            if (type.IsATypeOf(typeof(Parent)))
            {
                var filter = ExpressionHelper<IDocument<Guid>, Parent>.Transform(predicate);
                return await DeleteManyParentsAsync(filter, cascade, token);
            }

            if (type.IsATypeOf(typeof(Child)))
            {
                var filter = ExpressionHelper<IDocument<Guid>, Child>.Transform(predicate);
                return await DeleteManyChildrenAsync(filter, cascade, token);
            }

            if (!type.IsATypeOf(typeof(Event))) throw new UnsupportedTypeException(type);
            {
                var filter = ExpressionHelper<IDocument<Guid>, Event>.Transform(predicate);
                return await DeleteManyEventsAsync(filter, token);
            }
        }

        private async Task<long> DeleteManyParentsAsync(Expression<Func<Parent, bool>> predicate, bool cascade = default,
            CancellationToken token = default)
        {
            if (!cascade)
                return await _parentRepository.DeleteManyAsync(predicate, token);

            var parents = await _parentRepository.GetManyAsync(predicate, token);
            return await DeleteManyParentsAsync(parents, true, token);
        }

        private async Task<long> DeleteManyChildrenAsync(Expression<Func<Child, bool>> predicate,
            bool cascade = default, CancellationToken token = default)
        {
            if (!cascade)
                return await _childRepository.DeleteManyAsync(predicate, token);

            var children = await _childRepository.GetManyAsync(predicate, token);
            return await DeleteManyChildrenAsync(children, true, token);
        }

        private async Task<long> DeleteManyEventsAsync(Expression<Func<Event, bool>> predicate,
            CancellationToken token = default)
        {
            return await _eventRepository.DeleteManyAsync(predicate, token);
        }

        #endregion

        #endregion

        #region ValidateTransition

        private async Task<bool> IsValidTransitionAsync(IEvent @event, CancellationToken token = default)
        {
            if (!(@event is ITransitionEvent current))
                return true;

            var previous = (ITransitionEvent) await _eventRepository
                .GetLastAsync(
                    e => e.ChildId.Equals(@event.ChildId) &&
                         e.EventType.Equals(EventType.Sleep),
                    e => e.Timestamp,
                    token);

            return _transitionValidator.IsValid(previous, current);
        }

        private async Task<bool> IsValidTransitionsAsync(IEnumerable<IEvent> events, CancellationToken token = default)
        {
            var sleepsToSave = events
                .Where(e => e.EventType.Equals(EventType.Sleep))
                .OrderBy(e => e.Timestamp)
                .GroupBy(g => g.ChildId)
                .ToList();

            if (!sleepsToSave.Any())
                return true;

            var isValid = true;

            foreach (var group in sleepsToSave)
            {
                var previous = default(ITransitionEvent);

                if (await _eventRepository
                    .GetLastAsync(
                        e => e.ChildId.Equals(group.Key) &&
                             e.EventType.Equals(EventType.Sleep),
                        e => e.Timestamp,
                        token) is ITransitionEvent lastSleep)
                {
                    previous = lastSleep;
                }

                foreach (var @event in group)
                {
                    var current = (ITransitionEvent) @event;
                    if (!_transitionValidator.IsValid(previous, current))
                    {
                        isValid = false;
                        break;
                    }
                    previous = current;
                }

                if (!isValid)
                    break;
            }

            return isValid;
        }

        #endregion

        #region LinkedDocumentExists

        private async Task<bool> LinkedDocumentExistsAsync<TLink>(Guid id, CancellationToken token = default)
        {
            if (id.IsEmpty())
                return false;

            var type = typeof(TLink);

            if (type.IsATypeOf(typeof(Parent)))
                if (await _parentRepository.AnyAsync(p => p.Id.Equals(id), token))
                    return true;

            if (!type.IsATypeOf(typeof(Child)))
                return false;

            return await _childRepository.AnyAsync(c => c.Id.Equals(id), token);
        }

        private bool LinkedDocumentExists<TLink>(Guid id, CancellationToken token = default)
        {
            if (id.IsEmpty())
                return false;

            var type = typeof(TLink);

            if (type.IsATypeOf(typeof(Parent)))
                if (_parentRepository.AnyAsync(p => p.Id.Equals(id), token).Result)
                    return true;

            if (!type.IsATypeOf(typeof(Child)))
                return false;

            return _childRepository.AnyAsync(c => c.Id.Equals(id), token).Result;
        }

        #endregion
    }
}