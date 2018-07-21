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

namespace Bubbio.Repository.MongoDb
{
    public class ParentUnitOfWork : INamedEntityUnitOfWork<Parent, Guid>
    {
        private readonly IRepository<Parent, Guid> _repository;

        public ParentUnitOfWork(IRepository<Parent, Guid> repository)
        {
            _repository = repository;
        }

        #region Create

        /// <inheritdoc />
        public async Task SaveAsync(string first, string last, string middle = null, Guid foreignId = default,
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
            await _repository.InsertAsync(parent, token);        }

        /// <inheritdoc />
        public async Task SaveAsync(IEnumerable<Parent> entities, CancellationToken token = default)
        {
            var documents = entities.Select(p => new Parent {Name = p.Name});
            await _repository.InsertManyAsync(documents, token);
        }

        #endregion

        #region Read

        /// <inheritdoc />
        public async Task<Parent> GetAsync(Guid id, CancellationToken token = default)
        {
            if (!await AnyAsync(p => p.Id.Equals(id), token))
                throw new DocumentNotFoundException<Guid>(typeof(Parent), id);
            return await _repository.GetAsync(id, token);
        }

        /// <inheritdoc />
        public async Task<Parent> GetAsync(Parent entity, CancellationToken token = default)
        {
            if (!await AnyAsync(p => p.Id.Equals(entity.Id), token))
                throw new DocumentNotFoundException<Guid>(entity.GetType(), entity.Id);
            return await _repository.GetAsync(entity, token);
        }

        /// <inheritdoc />
        public async Task<Parent> GetAsync(Expression<Func<Parent, bool>> predicate, CancellationToken token = default)
        {
            if (!await AnyAsync(predicate, token))
                throw new DocumentNotFoundException<Guid>(typeof(Parent));
            return await _repository.GetAsync(predicate, token);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Parent>> GetManyAsync(Expression<Func<Parent, bool>> predicate,
            CancellationToken token = default)
        {
            if (!await AnyAsync(predicate, token))
                throw new DocumentNotFoundException<Guid>(typeof(IEnumerable<Parent>));
            return await _repository.GetManyAsync(predicate, token);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Parent>> GetManyAsync(Expression<Func<Parent, bool>> predicate, int skip = 0,
            int take = 50, CancellationToken token = default)
        {
            if (!await AnyAsync(predicate, token))
                throw new DocumentNotFoundException<Guid>(typeof(IEnumerable<Parent>));
            return await _repository.GetManyAsync(predicate, skip, take, token);
        }

        /// <inheritdoc />
        public async Task<bool> AnyAsync(Expression<Func<Parent, bool>> predicate, CancellationToken token = default)
        {
            return await _repository.AnyAsync(predicate, token);
        }

        /// <inheritdoc />
        public async Task<long> CountAsync(CancellationToken token = default)
        {
            return await _repository.CountAsync(token);
        }

        /// <inheritdoc />
        public async Task<long> CountAsync(Expression<Func<Parent, bool>> predicate, CancellationToken token = default)
        {
            return await _repository.CountAsync(predicate, token);
        }

        /// <inheritdoc />
        public async Task<TProjection> ProjectAsync<TProjection>(Expression<Func<Parent, bool>> predicate,
                Expression<Func<Parent, TProjection>> projection, CancellationToken token = default)
            where TProjection : class
        {
            if (!await AnyAsync(predicate, token))
                throw new DocumentNotFoundException<Guid>(typeof(Parent));
            return await _repository.ProjectAsync(predicate, projection, token);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TProjection>> ProjectManyAsync<TProjection>(
            Expression<Func<Parent, bool>> predicate,
            Expression<Func<Parent, TProjection>> projection, CancellationToken token = default)
            where TProjection : class
        {
            if (!await AnyAsync(predicate, token))
                throw new DocumentNotFoundException<Guid>(typeof(IEnumerable<Parent>));
            return await _repository.ProjectManyAsync(predicate, projection, token);
        }

        #endregion

        #region Update

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(Parent updated, CancellationToken token = default)
        {
            return await _repository.UpdateAsync(updated, token);
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync<TField>(Parent toUpdate, Expression<Func<Parent, TField>> selector,
            TField value, CancellationToken token = default)
        {
            return await _repository.UpdateAsync(toUpdate, selector, value, token);
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync<TField>(Expression<Func<Parent, bool>> predicate,
            Expression<Func<Parent, TField>> selector, TField value, CancellationToken token = default)
        {
            if (await CountAsync(predicate, token) > 1)
                throw new ManyDocumentsFoundException(RepositoryOperation.Update, typeof(Parent));
            return await _repository.UpdateAsync(predicate, selector, value, token);
        }

        #endregion

        #region Delete

        /// <inheritdoc />
        public async Task<long> DeleteAsync(Guid id, CancellationToken token = default)
        {
            var deleted = await _repository.DeleteAsync(id, token);

            if (deleted == 0)
                throw new DocumentNotFoundException<Guid>(typeof(Parent), id);

            return deleted;
        }

        /// <inheritdoc />
        public async Task<long> DeleteAsync(Parent entity, CancellationToken token = default)
        {
            var deleted = await _repository.DeleteAsync(entity, token);

            if (deleted == 0)
                throw new DocumentNotFoundException<Guid>(typeof(Parent), entity.Id);

            return deleted;
        }

        /// <inheritdoc />
        public async Task<long> DeleteAsync(Expression<Func<Parent, bool>> predicate, CancellationToken token = default)
        {
            var count = await CountAsync(predicate, token);

            if (count == 0)
                throw new DocumentNotFoundException<Guid>(typeof(Parent));
            if (count > 1)
                throw new ManyDocumentsFoundException(RepositoryOperation.Delete, typeof(Parent));

            return await _repository.DeleteAsync(predicate, token);
        }

        /// <inheritdoc />
        public async Task<long> DeleteManyAsync(IEnumerable<Parent> entities, CancellationToken token = default)
        {
            var deleted = await _repository.DeleteManyAsync(entities, token);

            if (deleted == 0)
                throw new DocumentNotFoundException<Guid>(typeof(Parent));

            return deleted;
        }

        /// <inheritdoc />
        public async Task<long> DeleteManyAsync(Expression<Func<Parent, bool>> predicate,
            CancellationToken token = default)
        {
            var deleted = await _repository.DeleteManyAsync(predicate, token);

            if (deleted == 0)
                throw new DocumentNotFoundException<Guid>(typeof(Parent));

            return deleted;
        }

        #endregion
    }
}