using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Bubbio.Core.Exceptions;
using Bubbio.Core.Helpers;
using Bubbio.Core.Repository;
using Bubbio.MongoDb.Documents.Entities;

namespace Bubbio.Repository.MongoDb
{
    public class ChildUnitOfWork : IUnitOfWork<Child, Guid>
    {
        private readonly IRepository<Child, Guid> _repository;

        public ChildUnitOfWork(IRepository<Child, Guid> repository)
        {
            _repository = repository;
        }

        #region Create

        public async Task SaveAsync(Child entity, CancellationToken token = default)
        {
            if (entity.ParentId.IsEmpty())
                throw new InvalidForeignIdException<Guid>(typeof(Child), typeof(Parent));

            await _repository.InsertAsync(entity, token);
        }

        /// <inheritdoc />
        public async Task SaveAsync(IEnumerable<Child> entities, CancellationToken token = default)
        {
            var enumerable = entities.ToList();

            if (enumerable.Any(c => c.ParentId.IsEmpty()))
                throw new InvalidForeignIdException<Guid>(typeof(Child), typeof(Parent));

            await _repository.InsertManyAsync(enumerable, token);
        }

        #endregion

        #region Read

        /// <inheritdoc />
        public async Task<Child> GetAsync(Guid id, CancellationToken token = default)
        {
            if (!await AnyAsync(p => p.Id.Equals(id), token))
                throw new DocumentNotFoundException<Guid>(typeof(Child), id);
            return await _repository.GetAsync(id, token);
        }

        /// <inheritdoc />
        public async Task<Child> GetAsync(Child entity, CancellationToken token = default)
        {
            if (!await AnyAsync(p => p.Id.Equals(entity.Id), token))
                throw new DocumentNotFoundException<Guid>(entity.GetType(), entity.Id);
            return await _repository.GetAsync(entity, token);
        }

        /// <inheritdoc />
        public async Task<Child> GetAsync(Expression<Func<Child, bool>> predicate, CancellationToken token = default)
        {
            if (!await AnyAsync(predicate, token))
                throw new DocumentNotFoundException<Guid>(typeof(Child));
            return await _repository.GetAsync(predicate, token);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Child>> GetManyAsync(Expression<Func<Child, bool>> predicate,
            CancellationToken token = default)
        {
            if (!await AnyAsync(predicate, token))
                throw new DocumentNotFoundException<Guid>(typeof(IEnumerable<Child>));
            return await _repository.GetManyAsync(predicate, token);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Child>> GetManyAsync(Expression<Func<Child, bool>> predicate, int skip = 0,
            int take = 50, CancellationToken token = default)
        {
            if (!await AnyAsync(predicate, token))
                throw new DocumentNotFoundException<Guid>(typeof(IEnumerable<Child>));
            return await _repository.GetManyAsync(predicate, skip, take, token);
        }

        /// <inheritdoc />
        public async Task<bool> AnyAsync(Expression<Func<Child, bool>> predicate, CancellationToken token = default)
        {
            return await _repository.AnyAsync(predicate, token);
        }

        /// <inheritdoc />
        public async Task<long> CountAsync(CancellationToken token = default)
        {
            return await _repository.CountAsync(token);
        }

        /// <inheritdoc />
        public async Task<long> CountAsync(Expression<Func<Child, bool>> predicate, CancellationToken token = default)
        {
            return await _repository.CountAsync(predicate, token);
        }

        /// <inheritdoc />
        public async Task<TProjection> ProjectAsync<TProjection>(Expression<Func<Child, bool>> predicate,
                Expression<Func<Child, TProjection>> projection, CancellationToken token = default)
            where TProjection : class
        {
            if (!await AnyAsync(predicate, token))
                throw new DocumentNotFoundException<Guid>(typeof(Child));
            return await _repository.ProjectAsync(predicate, projection, token);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TProjection>> ProjectManyAsync<TProjection>(
                Expression<Func<Child, bool>> predicate, Expression<Func<Child, TProjection>> projection,
                CancellationToken token = default)
            where TProjection : class
        {
            if (!await AnyAsync(predicate, token))
                throw new DocumentNotFoundException<Guid>(typeof(IEnumerable<Child>));
            return await _repository.ProjectManyAsync(predicate, projection, token);
        }

        #endregion

        #region Update

        /// <inheritdoc />
        public async Task<bool> UpdateAsync(Child updated, CancellationToken token = default)
        {
            return await _repository.UpdateAsync(updated, token);
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync<TField>(Child toUpdate, Expression<Func<Child, TField>> selector,
            TField value, CancellationToken token = default)
        {
            return await _repository.UpdateAsync(toUpdate, selector, value, token);
        }

        /// <inheritdoc />
        public async Task<bool> UpdateAsync<TField>(Expression<Func<Child, bool>> predicate,
            Expression<Func<Child, TField>> selector, TField value, CancellationToken token = default)
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
        public async Task<long> DeleteAsync(Child entity, CancellationToken token = default)
        {
            var deleted = await _repository.DeleteAsync(entity, token);

            if (deleted == 0)
                throw new DocumentNotFoundException<Guid>(typeof(Parent), entity.Id);

            return deleted;
        }

        /// <inheritdoc />
        public async Task<long> DeleteAsync(Expression<Func<Child, bool>> predicate, CancellationToken token = default)
        {
            var count = await CountAsync(predicate, token);

            if (count == 0)
                throw new DocumentNotFoundException<Guid>(typeof(Parent));
            if (count > 1)
                throw new ManyDocumentsFoundException(RepositoryOperation.Delete, typeof(Parent));

            return await _repository.DeleteAsync(predicate, token);
        }

        /// <inheritdoc />
        public async Task<long> DeleteManyAsync(IEnumerable<Child> entities, CancellationToken token = default)
        {
            var deleted = await _repository.DeleteManyAsync(entities, token);

            if (deleted == 0)
                throw new DocumentNotFoundException<Guid>(typeof(Parent));

            return deleted;
        }

        /// <inheritdoc />
        public async Task<long> DeleteManyAsync(Expression<Func<Child, bool>> predicate, CancellationToken token = default)
        {
            var deleted = await _repository.DeleteManyAsync(predicate, token);

            if (deleted == 0)
                throw new DocumentNotFoundException<Guid>(typeof(Parent));

            return deleted;
        }

        #endregion
    }
}