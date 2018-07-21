using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Bubbio.Core.Contracts;
using Bubbio.Core.Exceptions;
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

        public UnitOfWork(
            IRepository<Parent, Guid> parentRepository,
            IRepository<Child, Guid> childRepository,
            IRepository<Event, Guid> eventRepository)
        {
            _parentRepository = parentRepository;
            _childRepository = childRepository;
            _eventRepository = eventRepository;
        }

        #region ParentRepository

        #region Create

        public async Task SaveParentAsync(string first, string last, string middle = null,
            CancellationToken token = default)
        {
            var parent = new Parent
            {
                Name = new Name
                {
                    First = first,
                    Middle = middle,
                    Last = last
                }
            };
            await _parentRepository.InsertAsync(parent, token);
        }

        public async Task SaveParentsAsync(IEnumerable<IParent> parents, CancellationToken token = default)
        {
            var documents = parents.Select(p => new Parent {Name = p.Name});
            await _parentRepository.InsertManyAsync(documents, token);
        }

        #endregion

        #region Read

        public async Task<Parent> GetParentAsync(Guid id, CancellationToken token = default)
        {
            if (!await AnyParentAsync(p => p.Id.Equals(id), token))
                throw new DocumentNotFoundException<Guid>(typeof(Parent), id);
            return await _parentRepository.GetAsync(id, token);
        }

        public async Task<Parent> GetParentAsync(Parent parent, CancellationToken token = default)
        {
            if (!await AnyParentAsync(p => p.Id.Equals(parent.Id), token))
                throw new DocumentNotFoundException<Guid>(parent.GetType(), parent.Id);
            return await _parentRepository.GetAsync(parent, token);
        }

        public async Task<Parent> GetParentAsync(Expression<Func<Parent, bool>> predicate,
            CancellationToken token = default)
        {
            if (!await AnyParentAsync(predicate, token))
                throw new DocumentNotFoundException<Guid>(typeof(Parent));
            return await _parentRepository.GetAsync(predicate, token);
        }

        public async Task<IEnumerable<Parent>> GetParentsAsync(Expression<Func<Parent, bool>> predicate,
            CancellationToken token = default)
        {
            if (!await AnyParentAsync(predicate, token))
                throw new DocumentNotFoundException<Guid>(typeof(IEnumerable<Parent>));
            return await _parentRepository.GetManyAsync(predicate, token);
        }

        public async Task<IEnumerable<Parent>> GetParentsPagedAsync(Expression<Func<Parent, bool>> predicate, int skip,
            int take, CancellationToken token = default)
        {
            if (!await AnyParentAsync(predicate, token))
                throw new DocumentNotFoundException<Guid>(typeof(IEnumerable<Parent>));
            return await _parentRepository.GetManyAsync(predicate, skip, take, token);
        }

        public async Task<bool> AnyParentAsync(Expression<Func<Parent, bool>> predicate,
            CancellationToken token = default)
        {
            return await _parentRepository.AnyAsync(predicate, token);
        }

        public async Task<long> CountParentsAsync(CancellationToken token = default)
        {
            return await _parentRepository.CountAsync(token);
        }

        public async Task<long> CountParentsAsync(Expression<Func<Parent, bool>> predicate,
            CancellationToken token = default)
        {
            return await _parentRepository.CountAsync(predicate, token);
        }

        public async Task<TProjection> ProjectParentAsync<TProjection>(Expression<Func<Parent, bool>> predicate,
                Expression<Func<Parent, TProjection>> projection, CancellationToken token = default)
            where TProjection : class
        {
            if (!await AnyParentAsync(predicate, token))
                throw new DocumentNotFoundException<Guid>(typeof(Parent));
            return await _parentRepository.ProjectAsync(predicate, projection, token);
        }

        public async Task<IEnumerable<TProjection>> ProjectParentsAsync<TProjection>(
                Expression<Func<Parent, bool>> predicate, Expression<Func<Parent, TProjection>> projection,
                CancellationToken token = default)
            where TProjection : class
        {
            if (!await AnyParentAsync(predicate, token))
                throw new DocumentNotFoundException<Guid>(typeof(IEnumerable<Parent>));
            return await _parentRepository.ProjectManyAsync(predicate, projection, token);
        }

        #endregion

        #region Update

        public async Task<bool> UpdateParentAsync(Parent updated, CancellationToken token = default)
        {
            return await _parentRepository.UpdateAsync(updated, token);
        }

        public async Task<bool> UpdateParentAsync<TField>(Parent toUpdate, Expression<Func<Parent, TField>> selector,
            TField value, CancellationToken token = default)
        {
            return await _parentRepository.UpdateAsync(toUpdate, selector, value, token);
        }

        public async Task<bool> UpdateParentAsync<TField>(Expression<Func<Parent, bool>> predicate,
            Expression<Func<Parent, TField>> selector, TField value, CancellationToken token = default)
        {
            if (await CountParentsAsync(predicate, token) > 1)
                throw new ManyDocumentsFoundException(RepositoryOperation.Update, typeof(Parent));
            return await _parentRepository.UpdateAsync(predicate, selector, value, token);
        }

        #endregion

        #region Delete

        public async Task<long> DeleteParentAsync(Guid id, CancellationToken token = default)
        {
            var deleted = await _parentRepository.DeleteAsync(id, token);

            if (deleted == 0)
                throw new DocumentNotFoundException<Guid>(typeof(Parent), id);

            return deleted;
        }

        public async Task<long> DeleteParentAsync(Parent parent, CancellationToken token = default)
        {
            var deleted = await _parentRepository.DeleteAsync(parent, token);

            if (deleted == 0)
                throw new DocumentNotFoundException<Guid>(typeof(Parent), parent.Id);

            return deleted;
        }

        public async Task<long> DeleteParentAsync(Expression<Func<Parent, bool>> predicate, CancellationToken token = default)
        {
            var count = await CountParentsAsync(predicate, token);

            if (count == 0)
                throw new DocumentNotFoundException<Guid>(typeof(Parent));
            if (count > 1)
                throw new ManyDocumentsFoundException(RepositoryOperation.Delete, typeof(Parent));

            return await _parentRepository.DeleteAsync(predicate, token);
        }

        public async Task<long> DeleteManyParentsAsync(IEnumerable<Parent> parents, CancellationToken token = default)
        {
            var deleted = await _parentRepository.DeleteManyAsync(parents, token);

            if (deleted == 0)
                throw new DocumentNotFoundException<Guid>(typeof(Parent));

            return deleted;
        }

        public async Task<long> DeleteManyParentsAsync(Expression<Func<Parent, bool>> predicate,
            CancellationToken token = default)
        {
            var deleted = await _parentRepository.DeleteManyAsync(predicate, token);

            if (deleted == 0)
                throw new DocumentNotFoundException<Guid>(typeof(Parent));

            return deleted;
        }

        #endregion

        #endregion
    }
}